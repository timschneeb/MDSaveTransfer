using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.Nice.Values;
using Assets.Scripts.PeroTools.Nice.Variables;
using MDSaveTransfer.Converter.Report;
using MDSaveTransfer.Data;
using Newtonsoft.Json.Linq;
using UnityEngine.Experimental.PlayerLoop;
using String = Assets.Scripts.PeroTools.Nice.Values.String;

namespace MDSaveTransfer.Converter;

public partial class SwitchSaveConverter
{
    private readonly string[] _ignoredKeys = {
        "Password",
        "UserID",
        "PlayerName",
        "IsSync",
        "BoughtDLC",
        "TipNewAccountSystem",
        "TipTapAccount",
        "HasPurchaseUnlockAll",
        "NSUploadFailData",
        "IsBanShown",
        "userUid",
        "NsReleaseVersion",
        "Region"
    };

    private readonly string[] _ignoredRoots =
    {
        "Task"
    };
    
    /** Special case: List is wrapped by Ref */
    private readonly string[] _hasListInRef =
    {
        "easy_pass",
        "hard_pass",
        "master_pass"
    };
    
    /** Special case: IData members in List are wrapped by Ref */
    private readonly string[] _hasListIDataMembersInRef =
    {
        "RoleSkinSettings",
        "RewardItems",
        "highest",
    };
    
    /** Special case: Wrap C#-List in Ref instead of List wrapper */
    private readonly string[] _hasDirectRefToList =
    {
        "achievements",
        "Bulletins",
        "RoleSkinSettings",
    };

    private ConversionReport Report { get; } = new();
    
    /**
     * <summary>Skip over nested 'Constance' objects</summary>
     * <remarks>
     * The 'Constance' type is used to encapsulate other values in the save game format.
     * It's probably a misspelling of constant by the devs
     * </remarks>
     */
    private static object? TraverseConstance(object? head, string? debugContext)
    {
        while (head is Constance c) 
            head = c.Result;
        return head;
    }
    
    /**
     * <summary>Reattach the correct number 'Constance' object wrappers</summary>
     * <remarks>
     * 'SelectedAlbumIdx' has (probably mistakenly) two layers of 'Constance' wrappers.
     * Because of this, we need to make sure to scan for nested objects.
     * </remarks>
     */
    private static object? ReattachConstance(object? constance, object? value)
    {
        if (constance == null)
            return new Constance { Result = value };
        
        var next = constance;
        while (next is Constance c)
        {
            next = c.Result;
        }

        if (next is Constance last)
        {
            last.Result = value;
        }

        return constance;
    }
    
    /**
     * Process next JSON token to merge into SaveGame
     */
    private object? MergeNext(JToken item, object? targetItem, string parentKey, MergeStrategy strategy, string debugContext, bool forceRefType = false)
    {
        object? MergePrimitive<TStd,TSerialized>(object? constance) where TSerialized : IValue
        {
            // Special case: Primitive values in StageAchievement use Ref 
            if (debugContext.Contains(nameof(SaveData.StageAchievement) + "/"))
            {
                forceRefType = true;
            }
            
            // Skip wrappers
            var traversed = TraverseConstance(constance, debugContext);
            var targetType = traversed?.GetType();   
            // If value doesn't exist in SaveGame yet:
            if (traversed == null)
            {
                // Create value
                var newValue = (IValue)Activator.CreateInstance(forceRefType ? typeof(Ref) : typeof(TSerialized), null)!;
                newValue.Result = item.Value<TStd>() ?? throw new InvalidOperationException();
                traversed = newValue;
            }
            // Else if value type matches expected ones:
            else if (targetType == typeof(TSerialized) || targetType == typeof(Ref))
            {
                // Overwrite primitive value
                ((IValue)traversed).Result = item.Value<TStd>() ?? throw new InvalidOperationException();
            }
            else
            {
                // Unexpected type
                Report.Add(Error.UnsupportedPcType, $"{debugContext} ({item.Type.ToString()})");
            }
            
            // Restore wrappers
            return ReattachConstance(constance, traversed);
        }
        
        switch (item.Type)
        {
            case JTokenType.Object:
                targetItem ??= new Dictionary<string, IVariable>();
                var target = targetItem as Dictionary<string, IVariable>;

                foreach (var (key, value) in item.Value<JObject>() ?? new JObject())
                {
                    if(value == null || _ignoredKeys.Contains(key))
                        continue;

                    if (target == null)
                    {
                        // target already existed but cast failed
                        Report.Add(Error.UnsupportedPcType, $"{debugContext}/{key} (object)");
                    }
                    // TODO remove duplicated code below
                    else if (!target.ContainsKey(key))
                    {
                        // Key missing, let's create it recursively
                        
                        // Note: _dataObjectUid nor new array entries nor some StageAchievement subkeys should not be regarded 
                        if(key != "_dataObjectUid" && !debugContext.EndsWith("[]")) 
                            if(debugContext != "StageAchievement" && !StartsWithNumberRegex().IsMatch(key))
                                Report.Add(Information.NotPresentInPcSave, $"{debugContext}/{key}");
                        
                        var newTarget = MergeNext(value, null, key, strategy, $"{debugContext}/{key}", forceRefType);
                        if(newTarget is IVariable v)
                            target[key] = v;
                        else if(newTarget != null)
                            Report.Add(Error.UnexpectedTypeAfterCreation, $"{debugContext}/{key}");
                    }
                    else
                    {
                        // Key existing, let's merge it recursively
                        var updated = MergeNext(value, target[key], key, strategy, $"{debugContext}/{key}", forceRefType);
                        if (updated is IVariable variable)
                            target[key] = variable;
                        else if(updated != null)
                            Report.Add(Warning.UnexpectedTypeExisting, $"{debugContext}/{key}");
                    }
                }
                return target;
            case JTokenType.Array: 
                var traversed = TraverseConstance(targetItem, debugContext);
                if (traversed == null || true /* TODO merge support */)
                {
                    // Create new list
                    IList? newInternalList = null;
                    
                    foreach (var entry in item.Value<JArray>() ?? new JArray())
                    {
                        var value = new object(); 
                        switch (entry.Type)
                        {
                            case JTokenType.Integer:
                                value = entry.Value<int>();
                                break;
                            case JTokenType.Float:
                                value = entry.Value<float>();
                                break;
                            case JTokenType.String:
                                value = entry.Value<string>();
                                break;
                            case JTokenType.Boolean:
                                value = entry.Value<bool>();
                                break;
                            case JTokenType.Object:
                                // Recursively create object
                                var raw = MergeNext(entry, null, parentKey, strategy, $"{debugContext}[]", _hasListIDataMembersInRef.Contains(parentKey));
                                var newTarget = TraverseConstance(raw, $"{debugContext}[]");

                                newInternalList ??= new List<IData>();
                                
                                // Special rule: _dataObjectUid corresponds to IData.Uid
                                if (entry.Type == JTokenType.Object && newTarget is Dictionary<string, IVariable> fields)
                                {
                                    var uidField = fields.FirstOrDefault(x => x.Key == "_dataObjectUid");
                                    var uid = (uidField.Value?.Result as String)?.Result as string;
                                    fields.Remove("_dataObjectUid");
                                    value = new Assets.Scripts.PeroTools.Nice.Datas.Data
                                    {
                                        Uid = uid,
                                        Fields = fields
                                    };
                                }
                                break;
                            default:
                                Report.Add(Error.UnsupportedJsonArrayMemberType, $"{debugContext}[] ({entry.Type.ToString()})");
                                break;
                        }
                        
                        if (newInternalList == null)
                        {
                            // Auto-detect list type and instantiate
                            var constructedListType = typeof(List<>).MakeGenericType(value!.GetType());
                            newInternalList = (IList)Activator.CreateInstance(constructedListType)!;
                        }

                        // Add created value
                        if (value != null)
                            newInternalList.Add(value);
                        else
                            Report.Add(Error.ArrayItemFailed, $"{debugContext}[]");
                    }
                    
                    // No array members
                    if(newInternalList == null)
                        return null;

                    // Package final list and handle special cases
                    if (_hasDirectRefToList.Contains(parentKey))
                        return new Constance { Result = new Ref { Result = newInternalList} };
                    
                    var list = new List { Result = newInternalList };
                    return _hasListInRef.Contains(parentKey) 
                        ? new Constance { Result = new Ref { Result = list } }
                        : new Constance { Result = list };
                }
                else
                { 
                    // TODO implement array merging
                    Console.Error.WriteLine($"Error: '{debugContext}/[]' NOT IMPLEMENTED");
                }

                return null;
                break;
            case JTokenType.Integer:
                return MergePrimitive<int, Integer>(targetItem);
            case JTokenType.Float:
                return MergePrimitive<float, Float>(targetItem);
            case JTokenType.String:
                return MergePrimitive<string, String>(targetItem);
            case JTokenType.Boolean:
                return MergePrimitive<bool, Boolen>(targetItem);
            case JTokenType.Null:
                Report.Add(Information.JsonNullType, debugContext);
                return null;
            default:
                Report.Add(Error.UnsupportedJsonType, $"{debugContext} ({item.Type.ToString()})");
                return null;
        }
    }
    
    /**
     * Merge Switch save game (JSON format) with SaveData using a specific merge strategy
     */
    public ConversionReport MergeWith(SaveData saveData, byte[] jsonBytes, MergeStrategy strategy)
    {
        // Remove 3-byte binary file header
        var jsonString = Encoding.ASCII.GetString(jsonBytes[3..]);
        var root = JObject.Parse(jsonString);
        var targetProperties = typeof(SaveData).GetProperties();
        
        foreach (var (key, value) in root)
        {
            if (_ignoredRoots.Contains(key))
                continue;
                
            var subTreeProperty = targetProperties.SingleOrDefault(x => x.Name == key);
            if (subTreeProperty != null)
            {
                Console.WriteLine($"Merging root key '{key}'");
                var subtreeJson = value?.Value<string>();
                if (subtreeJson == null)
                {
                    Report.Add(Error.JsonRootValueNull, key);
                    continue;
                }

                var subtree = subTreeProperty.GetValue(saveData)!;
                // Recursively merge the two save games
                var modified = MergeNext(JObject.Parse(subtreeJson), subtree, key, strategy, key);
                subTreeProperty.SetValue(saveData, modified);
            }
        }

        return Report;
    }

    [GeneratedRegex("^\\d+")]
    private static partial Regex StartsWithNumberRegex();
}