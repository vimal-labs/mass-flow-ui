using Elsa.Studio.Authentication.Abstractions.Contracts;
using Elsa.Studio.Authentication.Abstractions.Models;

namespace MassFlow.Studio.Authentication.ElsaIdentity.UI.Services;

/// <summary>
/// Login method catalog for Mass Flow — same as Elsa's default but without
/// marking the method as preferred (removes the "Preferred" badge on the login page).
/// </summary>
public sealed class MassFlowIdentityLoginMethodCatalog : ILoginMethodCatalog
{
    public ValueTask<LoginMethodCatalogResult> ListAsync(CancellationToken cancellationToken = default) =>
        ValueTask.FromResult(new LoginMethodCatalogResult(
        [
            new LoginMethodDescriptor(
                "elsa-identity",
                "elsa-identity",
                "elsa-identity",
                "Mass Flow account",
                "elsa",
                0,
                false,          // IsPreferred = false → hides "Preferred" badge
                string.Empty)
        ],
        string.Empty));         // no PreferredMethodKey → no auto-preferred selection
}
