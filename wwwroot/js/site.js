// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function hex_to_rgb(hex_str) {
    hex_str = hex_str.replace("#", "");
    return [
        parseInt(hex_str.slice(0, 2), 16),
        parseInt(hex_str.slice(2, 4), 16),
        parseInt(hex_str.slice(4, 6), 16)
    ];
}

function calculate_luminance(color_code) {
    let index = parseFloat(color_code) / 255;

    if (index < 0.03928) {
        return index / 12.92;
    } else {
        return Math.pow((index + 0.055) / 1.055, 2.4);
    }
}

const RELATIVE_LUMINANCE_LOOKUP = new Array(256)
    .fill(0)
    .map((_, i) => calculate_luminance(i));

function calculate_relative_luminance(rgb) {
    return (
        0.2126 * RELATIVE_LUMINANCE_LOOKUP[rgb[0]] +
        0.7152 * RELATIVE_LUMINANCE_LOOKUP[rgb[1]] +
        0.0722 * RELATIVE_LUMINANCE_LOOKUP[rgb[2]]
    );
}

function color_contrast_ratio(color1, color2) {
    const rgb1 = hex_to_rgb(color1);
    const rgb2 = hex_to_rgb(color2);

    const luminance1 = calculate_relative_luminance(rgb1);
    const luminance2 = calculate_relative_luminance(rgb2);

    let contrast_ratio;
    if (luminance1 > luminance2) {
        contrast_ratio = (luminance1 + 0.05) / (luminance2 + 0.05);
    } else {
        contrast_ratio = (luminance2 + 0.05) / (luminance1 + 0.05);
    }

    return contrast_ratio;
}

function get_contrast_info(color1, color2) {
    const contrast_ratio = color_contrast_ratio(color1, color2);
    let usable_for;

    if (contrast_ratio < 3) {
        usable_for = "Not recommended";
    } else if (contrast_ratio >= 3 && contrast_ratio < 5) {
        usable_for = "Not bad";
    } else if (contrast_ratio >= 5 && contrast_ratio < 10) {
        usable_for = "Good";
    } else if (contrast_ratio >= 7) {
        usable_for = "Excellent";
    }

    return [contrast_ratio, usable_for];
}

// Example usage:
const color1 = "#7A0000";
const color2 = "#EAFF00";
const [contrast_ratio, usability_info] = get_contrast_info(color1, color2);
// console.log(`Color Contrast Ratio: ${contrast_ratio.toFixed(1)}:1`);
// console.log("Usability Information:", usability_info);

async function getMatchingColors(inputColor) {
    const apiUrl = "https://api.huemint.com/color";

    const jsonData = {
        mode: "transformer",
        num_colors: 4,
        temperature: "1.2",
        num_results: 1,
        adjacency: ["0", "65", "45", "35", "65", "0", "35", "65", "45", "35", "0", "35", "35", "65", "35", "0"],
        palette: [inputColor, "-", "-", "-"],
    };

    try {
        const response = await fetch(apiUrl, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(jsonData),
        });

        if (response.ok) {
            const data = await response.json();
            const palettes = data.results;

            if (palettes && palettes.length > 0) {
                const matchingColors = palettes[0].palette[1];
                return matchingColors;
            } else {
                console.error("Error: No palettes found in the response.");
            }
        } else {
            console.error("Error: Unable to get a response from the API.");
        }
    } catch (error) {
        console.error("Error:", error);
    }

    return null;
}