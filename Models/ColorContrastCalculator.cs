namespace FirstWeb.Models;

using System;

public static class ColorContrastCalculator
{
    public static double GetContrastRatio(string color1, string color2)
    {
        // Implement the logic to convert hex code to RGB and calculate the contrast ratio
        int[] rgb1 = HexToRgb(color1);
        int[] rgb2 = HexToRgb(color2);

        double luminance1 = CalculateRelativeLuminance(rgb1);
        double luminance2 = CalculateRelativeLuminance(rgb2);

        double contrastRatio;
        if (luminance1 > luminance2)
        {
            contrastRatio = (luminance1 + 0.05) / (luminance2 + 0.05);
        }
        else
        {
            contrastRatio = (luminance2 + 0.05) / (luminance1 + 0.05);
        }

        return contrastRatio;
    }

    private static int[] HexToRgb(string hexStr)
    {
        hexStr = hexStr.Replace("#", "");
        int[] rgb = new int[3];
        rgb[0] = Convert.ToInt32(hexStr.Substring(0, 2), 16);
        rgb[1] = Convert.ToInt32(hexStr.Substring(2, 2), 16);
        rgb[2] = Convert.ToInt32(hexStr.Substring(4, 2), 16);
        return rgb;
    }

    private static double CalculateRelativeLuminance(int[] rgb)
    {
        double[] relativeLuminanceLookup = new double[256];
        for (int i = 0; i < 256; i++)
        {
            double index = i / 255.0;

            if (index < 0.03928)
            {
                relativeLuminanceLookup[i] = index / 12.92;
            }
            else
            {
                relativeLuminanceLookup[i] = Math.Pow((index + 0.055) / 1.055, 2.4);
            }
        }

        double luminance = 0.2126 * relativeLuminanceLookup[rgb[0]] +
                          0.7152 * relativeLuminanceLookup[rgb[1]] +
                          0.0722 * relativeLuminanceLookup[rgb[2]];

        return luminance;
    }

    public static string GetUsabilityComment(double contrastRatio)
    {
        string usableFor;
        if (contrastRatio < 3)
        {
            usableFor = "Not recommended";
        }
        else if (contrastRatio >= 3 && contrastRatio < 5)
        {
            usableFor = "Not bad";
        }
        else if (contrastRatio >= 5 && contrastRatio < 10)
        {
            usableFor = "Good";
        }
        else
        {
            usableFor = "Excellent";
        }
        return usableFor;
    }
}