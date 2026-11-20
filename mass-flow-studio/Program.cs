using Elsa.Studio.Authentication.ElsaIdentity.BlazorWasm.Extensions;
using Elsa.Studio.Authentication.ElsaIdentity.HttpMessageHandlers;
using Elsa.Studio.Authentication.Abstractions.Models;
using Elsa.Studio.Authentication.UI.Extensions;
using Elsa.Studio.Authentication.UI.Options;
using Elsa.Studio.Authentication.OpenIdConnect.BlazorWasm.Extensions;
using Elsa.Studio.Authentication.OpenIdConnect.HttpMessageHandlers;
using Elsa.Studio.Contracts;
using Elsa.Studio.Core.BlazorWasm.Extensions;
using Elsa.Studio.Dashboard.Extensions;
using Elsa.Studio.Extensions;
using Elsa.Studio.Localization.BlazorWasm.Extensions;
using Elsa.Studio.Localization.Models;
using Elsa.Studio.Login.BlazorWasm.Extensions;
using Elsa.Studio.Login.Extensions;
using Elsa.Studio.Login.HttpMessageHandlers;
using Elsa.Studio.Models;
using Elsa.Studio.Shell;
using Elsa.Studio.Shell.Extensions;
using Elsa.Studio.Workflows.Extensions;
using Elsa.Studio.Workflows.Designer.Extensions;
using MassFlow.Studio.Authentication.UI.Extensions;
using MassFlow.Studio.Authentication.ElsaIdentity.UI.Extensions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
var configuration = builder.Configuration;
var services = builder.Services;

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.RootComponents.RegisterCustomElsaStudioElements();

var authenticationHandler = ConfigureAuthentication(services, configuration);
var backendApiConfig = new BackendApiConfig
{
    ConfigureBackendOptions = options => configuration.GetSection("Backend").Bind(options),
    ConfigureHttpClientBuilder = options => options.AuthenticationHandler = authenticationHandler
};
var localizationConfig = new LocalizationConfig
{
    ConfigureLocalizationOptions = options => configuration.GetSection("Localization").Bind(options)
};

services.AddCore();
// Override Elsa's DefaultBrandingProvider with MassFlow branding
services.AddScoped<Elsa.Studio.Branding.IBrandingProvider, MassFlow.Studio.MassFlowBrandingProvider>();
services.AddShell();
services.AddRemoteBackend(backendApiConfig);
services.AddDashboardModule(backendApiConfig);
services.AddWorkflowsModule();
services.AddLocalizationModule(localizationConfig);

var selectedAuthProvider = ConfigureStudioAuthenticationMode(services, configuration);
if (selectedAuthProvider != StudioAuthenticationProvider.ElsaLogin)
{
    services
        // 1. Register Elsa's authentication UI framework
        .AddAuthenticationUI(configuration.GetSection(LoginThemeOptions.SectionName))
        // 2. Register MassFlow login theme (overrides default Elsa theme)
        .AddMassFlowLoginTheme()
        // 3. Register MassFlow-branded login form (overrides default Elsa form label)
        .AddMassFlowElsaIdentityUI();
}

var app = builder.Build();

await app.UseElsaLocalization();

var startupTaskRunner = app.Services.GetRequiredService<IStartupTaskRunner>();
await startupTaskRunner.RunStartupTasksAsync();

await app.RunAsync();

static Type ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
{
    var authProvider = configuration["Authentication:Provider"];
    if (string.IsNullOrWhiteSpace(authProvider))
        authProvider = "ElsaIdentity";

    if (authProvider.Equals("ElsaIdentity", StringComparison.OrdinalIgnoreCase))
    {
        services.AddElsaIdentity();
        // AddElsaIdentityUI() is intentionally omitted here — MassFlow registers its own
        // branded login method component (MassFlowIdentityLoginMethod) via AddMassFlowElsaIdentityUI()
        // further down. Calling both would register Kind="elsa-identity" twice, causing
        // InvalidOperationException in LoginMethodComponentRegistry.
        return typeof(ElsaIdentityAuthenticatingApiHttpMessageHandler);
    }

    if (authProvider.Equals("OpenIdConnect", StringComparison.OrdinalIgnoreCase))
    {
        services.AddOpenIdConnectAuth(options => configuration.GetSection("Authentication:OpenIdConnect").Bind(options));
        return typeof(OidcAuthenticatingApiHttpMessageHandler);
    }

    if (authProvider.Equals("ElsaLogin", StringComparison.OrdinalIgnoreCase))
    {
        services.AddLoginModule().UseElsaIdentity();
        return typeof(AuthenticatingApiHttpMessageHandler);
    }

    throw new InvalidOperationException($"Unsupported Authentication:Provider value '{authProvider}'.");
}

static StudioAuthenticationProvider ConfigureStudioAuthenticationMode(IServiceCollection services, IConfiguration configuration)
{
    var authProvider = configuration["Authentication:Provider"];
    if (string.IsNullOrWhiteSpace(authProvider))
        authProvider = "ElsaIdentity";

    if (!Enum.TryParse<StudioAuthenticationProvider>(authProvider, true, out var selectedAuthProvider))
        throw new InvalidOperationException($"Unsupported Authentication:Provider value '{authProvider}'.");

    services.AddStudioAuthenticationMode(options => options.Provider = selectedAuthProvider);
    return selectedAuthProvider;
}
