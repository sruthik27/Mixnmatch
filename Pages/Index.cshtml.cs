using FirstWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace FirstWeb.Pages;

public class IndexModel : PageModel
{
    [BindProperty]
    public ComboModel Combo { get; set; }
    
    private readonly string _connectionString; // Read the connection string from configuration or constants
    private readonly MongoDbData _mongoDbData;
    private readonly FirebaseData _FirebaseData = new();

    public IndexModel(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("MongoDBConnection");
        _mongoDbData = new MongoDbData(_connectionString);
    }
    
    
    public IActionResult OnGet()
    {
        return Page();
    }
    
    public async Task<IActionResult> OnPost()
    {
        _FirebaseData.Increment();

        if (User.Identity.IsAuthenticated)
        {
            try
            {
                string useremail = User.Identity.Name;
                await _mongoDbData.SaveCombo(useremail, Combo.fg, Combo.bg);
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
            TempData["ComboType"] = "Duo";
            TempData["ComboFg"] = Combo.fg;
            TempData["ComboBg"] = Combo.bg;
            return RedirectToPage("Authenticate");
        }
        
        
        return RedirectToPage("/Index");
    }
}