using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices.JavaScript;
using MDSaveTransfer.Utils;

namespace MDSaveTransfer.Converter.Report;

public class ConversionReport
{
    public Dictionary<Error, HashSet<string>> Errors { get; } = new();
    public Dictionary<Warning, HashSet<string>> Warnings { get; } = new();
    public Dictionary<Information, HashSet<string>> Informations { get; } = new();
    
    public ConversionReport()
    {
        foreach (var member in (Error[]) Enum.GetValues(typeof(Error)))
            Errors.Add(member, new HashSet<string>());
        foreach (var member in (Warning[]) Enum.GetValues(typeof(Warning)))
            Warnings.Add(member, new HashSet<string>());
        foreach (var member in (Information[]) Enum.GetValues(typeof(Information)))
            Informations.Add(member, new HashSet<string>());
    }

    public void Add<T>(T issueType, string debugContext) where T : Enum
    {
        if(issueType is not Information)
            Console.Error.WriteLine($"{issueType.ToString()}: [{debugContext}] {issueType.GetDescription()}");
        switch (issueType)
        {
            case Error e:
                Errors[e].Add(debugContext);
                break;
            case Warning w:
                Warnings[w].Add(debugContext);
                break;
            case Information i:
                Informations[i].Add(debugContext);
                break;
            default:
                throw new ArgumentException();
        }
    }

    public int ErrorCount => Errors.Sum(x => x.Value.Count);
    public int WarningCount => Warnings.Sum(x => x.Value.Count);
    public int InformationCount => Informations.Sum(x => x.Value.Count);

    public bool HasMessages => ErrorCount > 0 || WarningCount > 0 || InformationCount > 0;
}