using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace FirstWeb.Pages
{
    [AllowAnonymous]
    public class GoogleSignInModel : PageModel
    {
        public IActionResult OnGetAsync()
        {
            var authProperties = new AuthenticationProperties
            {
                RedirectUri = Url.Page("/GoogleCallback"), // Redirect to the GoogleCallback page after authentication
                IsPersistent = true, // You can set this based on your requirements
            };

            var options = new GoogleOptions
            {
                ClientId = "603905866597-b63csc8epjuuoaq7qnu5jma6fsgjalaq.apps.googleusercontent.com",
                ClientSecret = "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"
            };

            return Challenge(authProperties, GoogleDefaults.AuthenticationScheme, options);
        }
    }

}