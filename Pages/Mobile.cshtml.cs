using FirstWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FirstWeb.Pages;

public class Mobile : PageModel
{
    [BindProperty]
    public TriComboModel TriCombo { get; set; }
    
    private readonly string _connectionString; // Read the connection string from configuration or constants
    private readonly MongoDbData _mongoDbData;

    public Mobile(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("MongoDBConnection");
        _mongoDbData = new MongoDbData(_connectionString);
    }
    public void OnGet()
    {
        
    }
    public async Task<IActionResult> OnPost()
    {
        if (User.Identity.IsAuthenticated)
        {
            try
            {
                string useremail = User.Identity.Name;
                await _mongoDbData.SaveTriCombo(useremail, TriCombo.bg, TriCombo.text,TriCombo.navigation);
                // Redirect to a confirmation page or any other page you want
                return RedirectToPage("Saved");
            }
            catch (Exception ex)
            {
                // Handle exceptions or display an error message
                ModelState.AddModelError("", "Error saving color preferences: " + ex.Message);
            }
        }
        else
        {
            TempData["ComboType"] = "Trio";
            TempData["TriComboB"] = TriCombo.bg;
            TempData["TriComboT"] = TriCombo.text;
            TempData["TriComboN"] = TriCombo.navigation;
            return RedirectToPage("Authenticate");
        }
        
        
        return RedirectToPage("/Mobile");
    }
}