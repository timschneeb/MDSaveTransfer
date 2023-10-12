using System.ComponentModel;

namespace MDSaveTransfer.Converter.Report;

public enum Information
{
    [Description("JSON null value skipped")]
    JsonNullType,
    [Description("Converter had to guess type of node that wasn't present in the PC save")]
    NotPresentInPcSave,
}