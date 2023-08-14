﻿let fg = document.querySelector("#color1");
let bg = document.querySelector("#color2");

let para = document.querySelector(".p-3");
let card = document.querySelector(".card")

let name1 = document.querySelector("#fg-code");
let name2 = document.querySelector("#bg-code");

let cratio = document.querySelector("#contrast-ratio");
let recommendation = document.querySelector("#recommendation");

let aitext = document.querySelector("#ai-text");
    
const sucess_pop1 = document.querySelector("#sucesspop1");
const sucess_pop2 = document.querySelector("#sucesspop2");
sucess_pop1.style.display = 'none';
sucess_pop2.style.display = 'none';
function setfgcolor() {
    para.style.color = fg.value;
    name1.textContent = "Foreground=" + fg.value.toUpperCase();
    const [a, b] = get_contrast_info(fg.value, bg.value);
    cratio.textContent = "Contrast ratio: " + a.toFixed(1);
    recommendation.textContent = "Insight: " + b;

    (async () => {
        const matchingColors = await getMatchingColors(fg.value);
        if (matchingColors !== null) {
            aitext.textContent = 'AI Recommended bg:'+matchingColors;
        } else {
            aitext.textContent = "No matching colors found.";
        }
    })();
}
function setbgcolor() {
    card.style.backgroundColor = bg.value;
    name2.textContent = "Background="+bg.value.toUpperCase();
    const [a,b] = get_contrast_info(fg.value, bg.value);
    cratio.textContent ='Contrast ratio: '+a.toFixed(1);
    recommendation.textContent = 'Insight: '+b;
    (async () => {
        const matchingColors = await getMatchingColors(bg.value);
        if (matchingColors !== null) {
            aitext.textContent = 'AI Recommended fg:'+matchingColors;
        } else {
            aitext.textContent = "No matching colors found.";
        }
    })();
}

function displaybadge(element) {
    element.style.display = 'inline';

    // Hide the badge after 1 second
    setTimeout(function () {
        element.style.display = 'none';
    }, 600);
}

function copyfg(){
    navigator.clipboard.writeText(fg.value);
    displaybadge(sucess_pop1);
}
function copybg(){
    navigator.clipboard.writeText(bg.value);
    displaybadge(sucess_pop2);
}

aitext.addEventListener("click", function (event) {
    const clickedText = event.target.textContent;
    const recommendedColor = clickedText.split(":")[1].trim();

    if (clickedText.includes("AI Recommended fg")) {
        fg.value = recommendedColor;
        setfgcolor();
    } else if (clickedText.includes("AI Recommended bg")) {
        bg.value = recommendedColor;
        setbgcolor();
    }
});

function GetTextColor(bgColor) {
    var rgb = bgColor.trimStart('#');
    var r = parseInt(rgb.substring(0, 2), 16);
    var g = parseInt(rgb.substring(2, 4), 16);
    var b = parseInt(rgb.substring(4, 6), 16);
    var perceivedBrightness = (r * 299 + g * 587 + b * 114) / 1000;

    return perceivedBrightness > 125 ? "#000000" : "#FFFFFF";
}