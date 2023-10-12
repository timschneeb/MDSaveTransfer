using System.Text;
using Assets.Scripts.PeroTools.Nice.Interface;
using MDSaveTransfer.Converter;
using MDSaveTransfer.Converter.Report;
using MDSaveTransfer.Utils;
using OdinSerializer;

namespace MDSaveTransfer.Data;

public class SaveData
{
    public Dictionary<string, IVariable> Account { private set; get; } = new();
    public Dictionary<string, IVariable> Achievement { private set; get; } = new();
    public Dictionary<string, IVariable> StageAchievement { private set; get; } = new();
    public Dictionary<string, IVariable> Task { private set; get; } = new();
    
    /** Deserialize binary save game data (PC version) into a new SaveData object */
    public SaveData(Stream fileContent, SaveDataFormat format)
    {
        if (format == SaveDataFormat.SimplifiedJson)
            throw new NotImplementedException("Direct Nintendo Switch save game import not implemented. Use MergeFromSwitch() instead");

        var serializerFormat = format == SaveDataFormat.Binary ? DataFormat.Binary : DataFormat.JSON;
        var config = new SerializationConfig { DebugContext = new DebugContext { ErrorHandlingPolicy = ErrorHandlingPolicy.ThrowOnErrors } };
        var ctxDe = new DeserializationContext { 
            Binder = new CompatSerializationBinder(), 
            Config = config
        };
        var root = SerializationUtility.DeserializeValueWeak(fileContent, serializerFormat, 
            new DeserializationContext { 
                Binder = new CompatSerializationBinder(), 
                Config = config
            });
        
        switch (format)
        {
            case SaveDataFormat.Binary:
                if (root is Dictionary<string, byte[]?> nestedRootObjects)
                {
                    foreach (var (rootKey, rootValue) in nestedRootObjects)
                    {
                        if (rootValue is null)
                            continue;
                        
                        using var memStream = new MemoryStream(rootValue, false);
                        var obj = SerializationUtility.DeserializeValueWeak(memStream, serializerFormat, ctxDe);
                        if (obj == null)
                        {
                            Console.Error.WriteLine($"Failed to deserialize '{rootKey}' root object");
                            continue;
                        }
                        
                        switch (rootKey)
                        {
                            case nameof(Account):
                                Account = (Dictionary<string, IVariable>)obj;
                                break;
                            case nameof(Achievement):
                                Achievement = (Dictionary<string, IVariable>)obj;
                                break;
                            case nameof(StageAchievement):
                                StageAchievement = (Dictionary<string, IVariable>)obj;
                                break;
                            case nameof(Task):
                                Task = (Dictionary<string, IVariable>)obj;
                                break;
                        }
                    }
                } 
                else throw new InvalidDataException("Failed to deserialize binary save data");
                break;
            case SaveDataFormat.TypesafeJson:
                if (root is Dictionary<string, object> rootObjects)
                {
                    foreach (var obj in rootObjects)
                    {
                        if (obj.Value is Dictionary<string, IVariable> value)
                        {
                            switch (obj.Key)
                            {
                                case nameof(Account):
                                    Account = value;
                                    break;
                                case nameof(Achievement):
                                    Achievement = value;
                                    break;
                                case nameof(StageAchievement):
                                    StageAchievement = value;
                                    break;
                                case nameof(Task):
                                    Task = value;
                                    break;
                            }
                        }
                    }
                } 
                else throw new InvalidDataException("Failed to deserialize typesafe JSON save data");
                break;
            default:
                throw new ArgumentException();
        }
    }
    
    private TRoot SerializeAll<TNested, TRoot>(Func<Dictionary<string, IVariable>, TNested> serializeWeak, Func<Dictionary<string, TNested>, TRoot> serializeRoot)
    {
        var rootObjects = new Dictionary<string, TNested>(4);
        foreach (var property in typeof(SaveData).GetProperties())
        {
            var name = property.Name;
            if (property.GetValue(this) is not Dictionary<string, IVariable> value)
            {
                Console.Error.WriteLine($"Warning: Root object '{name}' was null or invalid. Exported save may be incomplete.");
                continue;
            }

            rootObjects[name] = serializeWeak(value);
        }
                
        return serializeRoot(rootObjects);
    }

    public ConversionReport MergeFromSwitch(byte[] switchJson, MergeStrategy mergeStrategy)
    {
        return new SwitchSaveConverter().MergeWith(this, switchJson, mergeStrategy);
    }
    
    public ConversionReport MergeFromSwitch(string switchJson, MergeStrategy mergeStrategy)
    {
        return MergeFromSwitch(Encoding.ASCII.GetBytes(switchJson), mergeStrategy);
    }

    public void Export(string path, SaveDataFormat format)
    {
        using var stream = new FileStream(path, FileMode.OpenOrCreate);
        Export(stream, format);
    }
    
    public void Export(Stream stream, SaveDataFormat format)
    {
        switch (format)
        {
            case SaveDataFormat.Binary:
                var ctxRoot = new SerializationContext { Binder = new CompatSerializationBinder() };
                var binary = SerializeAll<byte[], byte[]>(
                    o => SerializationUtility.SerializeValueWeak(o, DataFormat.Binary, new SerializationContext { Binder = new CompatSerializationBinder() }),
                    o => SerializationUtility.SerializeValue(o, DataFormat.Binary, ctxRoot));
                stream.Write(binary);
                break;
            case SaveDataFormat.TypesafeJson:
                var ctxJson = new SerializationContext { Binder = new CompatSerializationBinder() };
                var debugJson = SerializeAll<object, byte[]>(
                    o => o,
                    o => SerializationUtility.SerializeValue(o, DataFormat.JSON, ctxJson));
                stream.Write(debugJson);
                break;
            case SaveDataFormat.SimplifiedJson:
                throw new NotImplementedException("Nintendo Switch save game export is not implemented");
            default:
                throw new ArgumentOutOfRangeException(nameof(format), format, null);
        }
    }

    public void DumpAsJson(string directory)
    {
        var ctx = new SerializationContext { Binder = new CompatSerializationBinder() };
        SerializeAll<string, object?>(
            o => Encoding.UTF8.GetString(SerializationUtility.SerializeValueWeak(o, DataFormat.JSON, ctx)),
            o =>
            {
                foreach (var (name, value) in o)
                {
                    File.WriteAllText($"{directory}/Dump_{name}.json", value);
                }
                return null;
            });
    }
    
    public static SaveData FromFile(string path, SaveDataFormat format)
    {
        using var stream = new FileStream(path, FileMode.Open);
        return new SaveData(stream, format);
    }
}