using Elsa.Studio.Authentication.Abstractions.ComponentProviders;
using Elsa.Studio.Authentication.Abstractions.Contracts;
using Elsa.Studio.Authentication.Abstractions.Models;
using Elsa.Studio.Authentication.ElsaIdentity.Contracts;
using Elsa.Studio.Authentication.ElsaIdentity.UI;
using Elsa.Studio.Authentication.ElsaIdentity.UI.Components;
using Elsa.Studio.Authentication.ElsaIdentity.UI.Services;
using Elsa.Studio.Contracts;
using MassFlow.Studio.Authentication.ElsaIdentity.UI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MassFlow.Studio.Authentication.ElsaIdentity.UI.Extensions;

public static class MassFlowElsaIdentityUIExtensions
{
    /// <summary>
    /// Registers MassFlow-branded Elsa Identity login UI.
    /// Replaces AddElsaIdentityUI() entirely â€” registers all the same services
    /// except uses MassFlow's login component instead of Elsa's default.
    /// </summary>
    public static IServiceCollection AddMassFlowElsaIdentityUI(this IServiceCollection services)
    {
        services.AddScoped<IFeature, ElsaIdentityUIFeature>();
        services.AddSingleton(new StudioAuthenticationProviderRegistration(StudioAuthenticationProvider.ElsaIdentity));
        services.AddScoped<ILoginMethodCatalog, MassFlowIdentityLoginMethodCatalog>();
        services.AddSingleton<ILoginMethodIconProvider, ElsaIdentityLoginMethodIconProvider>();

        // MassFlow-branded login form
        services.AddScoped<ILoginMethodComponentProvider, MassFlowIdentityLoginMethodComponentProvider>();

        // Redirect unauthorized users to /login
        services.AddScoped<IUnauthorizedComponentProvider, UnauthorizedComponentProvider<RedirectToLogin>>();

        // sessionStorage-based JWT accessor — tokens clear when tab closes,
        // so users always see the login page on a new browser session.
        services.AddScoped<IJwtAccessor, SessionStorageJwtAccessor>();

        return services;
    }
}
