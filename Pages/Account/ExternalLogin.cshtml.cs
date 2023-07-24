using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace FirstWeb.Pages.Account;


public class ExternalLoginModel : PageModel
{
    public IActionResult OnGetAsync(string provider, string returnUrl = null)
    {
        // Request a redirect to the external login provider.
        var redirectUrl = Url.Page("./ExternalLoginCallback", pageHandler: "Callback", values: new { returnUrl });
        var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        return new ChallengeResult(provider, properties);
    }
}