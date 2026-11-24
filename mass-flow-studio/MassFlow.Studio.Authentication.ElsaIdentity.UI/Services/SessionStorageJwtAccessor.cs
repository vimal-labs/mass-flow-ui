using Elsa.Studio.Authentication.ElsaIdentity.Contracts;
using Microsoft.JSInterop;

namespace MassFlow.Studio.Authentication.ElsaIdentity.UI.Services;

/// <summary>
/// JWT token accessor that uses sessionStorage instead of localStorage.
/// Tokens are cleared automatically when the browser tab or window is closed,
/// so users always see the login page when they open a new browser session.
/// </summary>
public sealed class SessionStorageJwtAccessor(IJSRuntime js) : IJwtAccessor
{
    public async ValueTask<string?> ReadTokenAsync(string name)
    {
        try
        {
            return await js.InvokeAsync<string?>("sessionStorage.getItem", name);
        }
        catch
        {
            return null;
        }
    }

    public async ValueTask WriteTokenAsync(string name, string token)
    {
        try
        {
            await js.InvokeVoidAsync("sessionStorage.setItem", name, token);
        }
        catch { }
    }

    public async ValueTask ClearTokenAsync(string name)
    {
        try
        {
            await js.InvokeVoidAsync("sessionStorage.removeItem", name);
        }
        catch { }

        // After the access token is cleared (logout), do a full page navigate to /login.
        // This reloads WASM from cache and lets the auth-init overlay cover the check,
        // so the user sees: loading splash → login page (no Authorizing flash).
        if (name == "accessToken")
        {
            try { await js.InvokeVoidAsync("eval", "window.location.replace('/login')"); }
            catch { }
        }
    }
}
