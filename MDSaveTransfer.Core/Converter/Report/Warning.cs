using System.ComponentModel;

namespace MDSaveTransfer.Converter.Report;

public enum Warning
{
    [Description("Failed to update existing key in PC save game due to unexpected type. Update skipped.")]
    UnexpectedTypeExisting
}