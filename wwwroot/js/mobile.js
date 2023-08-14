let fg = document.querySelector("#color1");
let bg = document.querySelector("#color2");
let textcolor = document.querySelector('#color3');


let navibar = document.querySelector('.navigation-bar');
let background = document.querySelector('.mobile-content');
let text = document.querySelectorAll('.para');
let icons = document.querySelectorAll('.icons');

function setfgcolor(){
    navibar.style.backgroundColor = fg.value;
}

function setbgcolor(){
    background.style.backgroundColor = bg.value;
}

function settextcolor(){
    icons.forEach(function(icon) {
        icon.style.color = textcolor.value;
    });
    text.forEach(function(para) {
        para.style.color = textcolor.value;
    });
}