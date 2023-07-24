using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace FirstWeb.Pages.Account;


public class ExternalLoginCallbackModel : PageModel
{
    public async Task<IActionResult> OnGetAsync()
    {
        var info = await HttpContext.AuthenticateAsync();
        if (!info.Succeeded)
        {
            // Handle the error if there was one during external authentication.
            return RedirectToPage("./Login");
        }

        // The user is successfully signed in, redirect to the return URL.
        var returnUrl = info.Properties.Items["ReturnUrl"];
        return LocalRedirect(returnUrl ?? Url.Content("~/"));
    }
}