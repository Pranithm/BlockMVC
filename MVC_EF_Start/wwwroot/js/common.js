window.onscroll = function () { myFunction() };
console.log("scrollign caling");
var navbar = document.getElementById("navbarID");
var sticky = navbar.offsetTop;

function myFunction() {
    if (window.pageYOffset >= sticky) {
        navbar.classList.add("sticky")
    } else {
        navbar.classList.remove("sticky");
    }
}