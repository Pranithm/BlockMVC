// function to handle onclick ecents on the bio buttons ﻿

function bioContent(person, arrow) {
    var element = document.getElementById(person);
    var arrow = document.getElementById(arrow);
    console.log(element)
    element.classList.toggle("hide_people--Summary");
    arrow.classList.toggle("arrow-up");
    // arrow data

};

const onLoad = () => {
    let nav = document.getElementById('about_Navbar').innerHTML;
}
