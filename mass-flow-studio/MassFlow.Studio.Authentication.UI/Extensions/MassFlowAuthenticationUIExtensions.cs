using Elsa.Studio.Authentication.UI.Extensions;
using Elsa.Studio.Authentication.UI.Options;
using MassFlow.Studio.Authentication.UI.Components.Themes;
using Microsoft.Extensions.DependencyInjection;

namespace MassFlow.Studio.Authentication.UI.Extensions;

public static class MassFlowAuthenticationUIExtensions
{
    public const string ThemeId = "massflow";

    /// <summary>
    /// Registers the MassFlow login theme.
    /// Call this AFTER <c>AddAuthenticationUI()</c>.
    /// </summary>
    public static IServiceCollection AddMassFlowLoginTheme(this IServiceCollection services)
    {
        // Register the MassFlow theme component
        services.AddLoginTheme<MassFlowLoginTheme>(ThemeId);

        // Set MassFlow as the selected theme
        services.Configure<LoginThemeOptions>(options =>
        {
            options.Theme = ThemeId;
        });

        return services;
    }
}
