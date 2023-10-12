using OdinSerializer;

namespace MDSaveTransfer.Utils;

public static class BinarySaveDumper
{
    /**
     * Bare-bones binary PC save-game deserializer for debugging purposes
     */
    public static void Dump(Stream stream, int baseDepth = 1)
    {
        var reader = new BinaryDataReader(stream, new DeserializationContext());

        Type? lastNodeType = null;
        while (true)
        {
            var type = reader.PeekEntry(out var name);

            var depth = baseDepth + (type is EntryType.EndOfArray or EntryType.EndOfNode
                ? reader.CurrentNodeDepth - 1
                : reader.CurrentNodeDepth);
            var indent = string.Concat(Enumerable.Repeat('\t', depth));  
            Console.Write(indent + $"[{type.ToString()}] {name} ");

            var nodeDebugInfo = "";
            switch (type)
            {
                case EntryType.StartOfNode:
                    reader.EnterNode(out var nodeType);
                    lastNodeType = nodeType;
                    nodeDebugInfo = $"<{nodeType}>";
                    break;
                case EntryType.EndOfNode:
                    reader.ExitNode();
                    break;
                case EntryType.StartOfArray:
                    reader.EnterArray(out var arrayLength);
                    nodeDebugInfo = $"{arrayLength} items";
                    break;
                case EntryType.EndOfArray:
                    reader.ExitArray();
                    break;
                case EntryType.String:
                    reader.ReadString(out var str);
                    nodeDebugInfo = $"\"{str}\"";
                    break;
                case EntryType.Boolean:
                    reader.ReadBoolean(out var boolValue);
                    nodeDebugInfo = $"\"{boolValue.ToString()}\"";
                    break;
                case EntryType.Integer:
                    reader.ReadInt32(out var intValue);
                    nodeDebugInfo = $"\"{intValue}\"";
                    break;
                case EntryType.InternalReference:
                    reader.ReadInternalReference(out var id);
                    var value = reader.Context.GetInternalReference(id);
                    nodeDebugInfo = $"id=\"{id}\" value=\"{value}\"";
                    break;
                case EntryType.PrimitiveArray:
                    var formatter = FormatterLocator.GetFormatter(lastNodeType, reader.Context.Config.SerializationPolicy);
                    if (lastNodeType == typeof(byte[]))
                    {
                        Console.WriteLine();
                        nodeDebugInfo = null;
                    
                        var array = (byte[])Convert.ChangeType(formatter.Deserialize(reader), lastNodeType);
                        var memStream = new MemoryStream(array, false);
                        Dump(memStream, depth + 1);
                        // nodeDebugInfo = ProperBitConverter.BytesToHexString(array);
                    }
                    else
                    {
                        nodeDebugInfo = "Array type NOT IMPLEMENTED";
                    }

                    break;
                case EntryType.Null:
                    reader.ReadNull();
                    break;
                case EntryType.EndOfStream:
                    break;
                default:
                    nodeDebugInfo = "NOT IMPLEMENTED";
                    reader.SkipEntry();
                    break;
            }

            if(nodeDebugInfo != null)
                Console.WriteLine(nodeDebugInfo);

            if (type == EntryType.EndOfStream)
                break;
        }
    }
}