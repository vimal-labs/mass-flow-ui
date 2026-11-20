using Elsa.Studio.Authentication.UI.Contracts;
using Elsa.Studio.Authentication.UI.Models;
using Microsoft.AspNetCore.Components;

namespace MassFlow.Studio.Authentication.UI.Components.Themes;

/// <summary>
/// Renders <see cref="MassFlowLoginTheme"/> for the MassFlow brand canvas login experience.
/// </summary>
public sealed class MassFlowLoginThemeProvider : ILoginThemeProvider
{
    public RenderFragment Render(LoginThemeContext context) => builder =>
    {
        builder.OpenComponent<MassFlowLoginTheme>(0);
        builder.AddComponentParameter(1, nameof(MassFlowLoginTheme.Context), context);
        builder.CloseComponent();
    };
}
