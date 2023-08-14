using FirstWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace FirstWeb.Pages.api;

[Route("api/[controller]")]
[ApiController]
public class ColorContrastController : ControllerBase
{
    // GET: api/colorcontrast/color1/color2
    [HttpGet("{color1}/{color2}")]
    public IActionResult GetColorContrastInfo(string color1, string color2)
    {
        double contrastRatio = ColorContrastCalculator.GetContrastRatio(color1, color2);
        string usableFor = ColorContrastCalculator.GetUsabilityComment(contrastRatio);

        var result = new
        {
            ContrastRatio = contrastRatio,
            Insight = usableFor
        };

        return Ok(result);
    }
}