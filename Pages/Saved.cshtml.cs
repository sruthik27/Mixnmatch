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
    public List<MongoDbData.StoredCombo> AllColors { get; set; }
    
    public async Task OnGet()
    {
        if (!User.Identity.IsAuthenticated)
        {
            // User is not authenticated. Add debugging code here to investigate further.
            // For example, you can use Console.WriteLine or TempData to log messages.
            TempData["Message"] = "User is not authenticated.";
            Console.WriteLine("User is not authenticated.");
        }
        else
        {
            Console.WriteLine("User authenticated");
            string comboFg = TempData["ComboFg"] as string;
            string comboBg = TempData["ComboBg"] as string;
            if (comboFg != null && comboBg != null)
            {
                _mongoDbData.SaveCombo(User.Identity.Name, comboFg, comboBg);
            }
        }
        CountValue = await _mongoDbData.GetComboCount(User.Identity.Name);
        AllColors = await _mongoDbData.GetAllCombos(User.Identity.Name);
    }
}
// Move the GetTextColor method outside
public static class ColorExtensions
{
    public static string GetTextColor(string bgColor)
    {
        // Calculate the perceived brightness of the background color
        var rgb = bgColor.TrimStart('#');
        var r = int.Parse(rgb.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        var g = int.Parse(rgb.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        var b = int.Parse(rgb.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        var perceivedBrightness = (r * 299 + g * 587 + b * 114) / 1000;

        // Determine the appropriate text color (black or white)
        return perceivedBrightness > 125 ? "#000000" : "#FFFFFF";
    }
}