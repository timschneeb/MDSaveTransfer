using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MDSaveTransfer.Web.Shared;

/// <summary>
/// The base class for all BlazorBootstrap components.
/// </summary>
/// <remarks>
/// This class provides common functionality for all BlazorBootstrap components, such as
/// generating an ID for the component, building the class names and styles, and handling
/// custom attributes.
/// </remarks>
public partial class Callout : ComponentBase, IDisposable, IAsyncDisposable
{

    /// <summary>
    /// Predefined set of contextual colors.
    /// </summary>
    public enum CalloutType
    {
        /// <summary>
        /// No color will be applied to an element.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Danger color.
        /// </summary>
        Danger,

        /// <summary>
        /// Warning color.
        /// </summary>
        Warning,

        /// <summary>
        /// Info color.
        /// </summary>
        Info,

        /// <summary>
        /// Success color.
        /// </summary>
        Tip,
        
        /// <summary>
        /// Success color.
        /// </summary>
        Success
    }
    
    public string ToCalloutType(CalloutType type) =>
        type switch
        {
            CalloutType.Default => "",
            CalloutType.Danger => $"bb-callout-danger",
            CalloutType.Warning => $"bb-callout-warning",
            CalloutType.Info => $"bb-callout-info",
            CalloutType.Tip => $"bb-callout-success",
            CalloutType.Success => $"bb-callout-success",
            _ => ""
        };
    
    protected void BuildClasses()
    {
        ClassNames.Clear();
        ClassNames.Add("bb-callout");
        ClassNames.Add(ToCalloutType(Type));
    }

    private string GetHeading() =>
        string.IsNullOrWhiteSpace(Heading)
            ? Type switch
            {
                CalloutType.Default => "NOTE",
                CalloutType.Info => "INFO",
                CalloutType.Warning => "WARNING",
                CalloutType.Danger => "DANGER",
                CalloutType.Tip => "TIP",
                CalloutType.Success => "SUCCESS",
                _ => ""
            }
            : Heading;

    private string GetIconName() =>
        Type switch
        {
            CalloutType.Default => "bi-info-circle-fill",
            CalloutType.Info => "bi-info-circle-fill",
            CalloutType.Warning => "bi-exclamation-triangle-fill",
            CalloutType.Danger => "bi-fire",
            CalloutType.Tip => "bi-lightbulb",
            CalloutType.Success => "bi-check-circle-fill",
            _ => "bi-info-circle-fill"
        };

    private string CalloutHeadingCSSClass => "bb-callout-heading";

    /// <summary>
    /// Specifies the content to be rendered inside this.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private string heading => GetHeading();

    /// <summary>
    /// Gets or sets the callout heading.
    /// </summary>
    [Parameter]
    public string? Heading { get; set; }

    private string IconName => GetIconName();

    /// <summary>
    /// Gets or sets the callout color.
    /// </summary>
    [Parameter]
    public CalloutType Type
    {
        get => _type;
        set
        {
            _type = value;
            BuildClasses();
        }
    }


    /// <summary>
    /// Captures all the custom attributes that are not part of BlazorBootstrap component.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> Attributes { get; set; }
    

    /// <summary>
    /// Gets the built class-names based on all the rules set by the component parameters.
    /// </summary>
    public readonly List<string> ClassNames = new();

    private CalloutType _type = CalloutType.Default;
    public void Dispose()
    {
    }

    public async ValueTask DisposeAsync()
    {
    }
}