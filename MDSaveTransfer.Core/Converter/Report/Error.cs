using System.ComponentModel;

namespace MDSaveTransfer.Converter.Report;

public enum Error
{
    [Description("Failed to deserialize value of JSON root key. Data may be incomplete.")]
    JsonRootValueNull,
    [Description("Switch JSON item has unsupported type in PC save game")]
    UnsupportedPcType,
    [Description("JSON type is unsupported")]
    UnsupportedJsonType,
    [Description("JSON array member type is unsupported")]
    UnsupportedJsonArrayMemberType,
    [Description("Failed to create missing key in PC save game")]
    UnexpectedTypeAfterCreation,
    [Description("Failed to convert an array item")]
    ArrayItemFailed,

}