using Elsa.Studio.Authentication.Abstractions.Contracts;
using MassFlow.Studio.Authentication.ElsaIdentity.UI.Components;

namespace MassFlow.Studio.Authentication.ElsaIdentity.UI.Services;

/// <summary>
/// Maps the "elsa-identity" login kind to the MassFlow-branded login form component.
/// </summary>
public sealed class MassFlowIdentityLoginMethodComponentProvider : ILoginMethodComponentProvider
{
    public string Kind => "elsa-identity";
    public Type ComponentType => typeof(MassFlowIdentityLoginMethod);
}
