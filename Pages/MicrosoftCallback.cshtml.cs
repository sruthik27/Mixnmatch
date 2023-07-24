using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FirstWeb.Pages
{
    public class MicrosoftCallbackModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            // Read the user's information from the authentication callback
            var authResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (authResult?.Principal is not null)
            {
                var userEmail = authResult.Principal.FindFirstValue(ClaimTypes.Email);

                // Store the user's email in a claim for later use
                var claimsIdentity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, userEmail)
                });

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true, // You can set this based on your requirements
                    RedirectUri = "/" // Redirect to the home page after successful authentication
                };

                // Sign the user in using cookie authentication
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
            }

            // Redirect to the home page or any other page you want after authentication
            return LocalRedirect("/");
        }
    }
}