document.body.style.zoom = "90%";
let fg = document.querySelector("#color1");
let bg = document.querySelector("#color2");
let textcolor = document.querySelector('#color3');

let name1 = document.querySelector("#fg-code");
let name2 = document.querySelector("#bg-code");
let name3 = document.querySelector('#text-code');

let cratio = document.querySelector("#contrast-ratio");
let recommendation = document.querySelector("#recommendation");
let aitext = document.querySelector("#ai-text");

let navibar = document.querySelector('.navigation-bar');
let background = document.querySelector('.mobile-content');
let text = document.querySelectorAll('.para');
let icons = document.querySelectorAll('.icons');

const sucess_pop1 = document.querySelector("#sucesspop1");
const sucess_pop2 = document.querySelector("#sucesspop2");
const sucess_pop3 = document.querySelector("#sucesspop3");
sucess_pop1.style.display = 'none';
sucess_pop2.style.display = 'none';
sucess_pop3.style.display = 'none';

function setfgcolor(){
    navibar.style.backgroundColor = fg.value;
    name1.textContent = "Foreground=" + fg.value.toUpperCase();
}

function setbgcolor(){
    background.style.backgroundColor = bg.value;
    name2.textContent = "Background="+bg.value.toUpperCase();
    const [a, b] = get_contrast_info(bg.value, textcolor.value);
    cratio.textContent = "Contrast ratio: " + a.toFixed(1);
    recommendation.textContent = "Insight: " + b;
    
    (async () => {
        const matchingColors = await getMatchingColors(bg.value);
        if (matchingColors !== null) {
            aitext.textContent = 'AI Recommended fg:'+matchingColors;
        } else {
            aitext.textContent = "No matching colors found.";
        }
    })();
}

function settextcolor(){
    icons.forEach(function(icon) {
        icon.style.color = textcolor.value;
    });
    text.forEach(function(para) {
        para.style.color = textcolor.value;
    });
    name3.textContent = "Text="+textcolor.value.toUpperCase();
    const [a, b] = get_contrast_info(bg.value, textcolor.value);
    cratio.textContent = "Contrast ratio: " + a.toFixed(1);
    recommendation.textContent = "Insight: " + b;

    (async () => {
        const matchingColors = await getMatchingColors(textcolor.value);
        if (matchingColors !== null) {
            aitext.textContent = 'AI Recommended bg:'+matchingColors;
        } else {
            aitext.textContent = "No matching colors found.";
        }
    })();
}

aitext.addEventListener("click", function (event) {
    const clickedText = event.target.textContent;
    const recommendedColor = clickedText.split(":")[1].trim();

    if (clickedText.includes("AI Recommended fg")) {
        textcolor.value = recommendedColor;
        settextcolor();
    } else if (clickedText.includes("AI Recommended bg")) {
        bg.value = recommendedColor;
        setbgcolor();
    }
});

function displaybadge(element) {
    element.style.display = 'inline';

    // Hide the badge after 1 second
    setTimeout(function () {
        element.style.display = 'none';
    }, 600);
}

function copytc(){
    navigator.clipboard.writeText(textcolor.value);
    displaybadge(sucess_pop3);
}
function copybg(){
    navigator.clipboard.writeText(bg.value);
    displaybadge(sucess_pop2);
}

function copynv(){
    navigator.clipboard.writeText(fg.value);
    displaybadge(sucess_pop1);
}