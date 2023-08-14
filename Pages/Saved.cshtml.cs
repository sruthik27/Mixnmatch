using FirstWeb.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FirstWeb.Pages;

[Authorize]
public class saved : PageModel
{
    private readonly string _connectionString;
    private readonly MongoDbData _mongoDbData;
    private readonly FirebaseData _FirebaseData = new();
    public int CountValue { get; set; }

    public saved(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("MongoDBConnection");
        _mongoDbData = new MongoDbData(_connectionString);
    }
    public List<MongoDbData.StoredCombo> AllDuos { get; set; }
    public List<MongoDbData.StoredTriCombo> AllTrios { get; set; }
    
    public async Task OnGet()
    {
        if (!User.Identity.IsAuthenticated)
        {
            // User is not authenticated
            TempData["Message"] = "User is not authenticated.";
            Console.WriteLine("User is not authenticated.");
        }
        else
        {
            Console.WriteLine("User authenticated");
            if (TempData["ComboType"] as string =="Duo")
            {
                string comboFg = TempData["ComboFg"] as string;
                string comboBg = TempData["ComboBg"] as string;
                if (comboFg != null && comboBg != null)
                {
                    _mongoDbData.SaveCombo(User.Identity.Name, comboFg, comboBg);
                } 
            }
            else
            {
                string comboBg = TempData["TriComboB"] as string;
                string comboText = TempData["TriComboT"] as string;
                string comboNav = TempData["TriComboN"] as string;
                if (comboText != null && comboBg != null && comboNav != null)
                {
                    _mongoDbData.SaveTriCombo(User.Identity.Name, comboBg, comboText,comboNav);
                } 
            }
            
            
        }
        CountValue = await _mongoDbData.GetCount(User.Identity.Name);
        AllDuos = await _mongoDbData.GetAllCombos(User.Identity.Name);
        AllTrios = await _mongoDbData.GetAllTriCombos(User.Identity.Name);
    }
}