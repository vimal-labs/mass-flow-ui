using Elsa.Studio.Branding;
using Elsa.Studio.Branding.Models;

namespace MassFlow.Studio;

/// <summary>
/// MassFlow-branded provider — replaces Elsa's DefaultBrandingProvider.
/// </summary>
public sealed class MassFlowBrandingProvider : DefaultBrandingProvider
{
    private readonly DefaultAppBarIcons _appBarIcons = new()
    {
        ShowDocumentationLink = false,
        ShowGitHubLink = false
    };

#pragma warning disable CS0672, CS0618
    public override string AppName => "Mass Flow";
    public override string AppNameWithVersion => "Mass Flow";
    public override string AppTagline => "Build Workflows. Run Anything.";
    public override string? LogoUrl => "/img/massflow-icon.svg";
    public override string? LogoReverseUrl => "/img/massflow-icon.svg";
#pragma warning restore CS0672, CS0618

    public override DefaultAppBarIcons AppBarIcons => _appBarIcons;
}
