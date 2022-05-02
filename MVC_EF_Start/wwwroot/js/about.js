function bioContent(person, arrow) {
    console.log("function for summary is clicked");
    var element = document.getElementById(person);
    var arrow = document.getElementById(arrow);
    console.log(element)
    element.classList.toggle("hide_people--Summary");
    arrow.classList.toggle("arrow-up");
    // arrow data

};

const onLoad = () => {
    console.log("hello world");
    let nav = document.getElementById('about_Navbar').innerHTML;
    // console.log(nav);
    nav = "hello guys"
}