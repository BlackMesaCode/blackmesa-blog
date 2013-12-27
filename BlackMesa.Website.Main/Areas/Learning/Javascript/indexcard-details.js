var backTiles = document.getElementsByClassName('back-tile');
var subMenus = document.getElementsByClassName("submenu");

var hideSubmenus = function () {
    for (var i = 0, max = subMenus.length; i < max; i++) {
        subMenus[i].style.display = "none";
    }
};
hideSubmenus();

var menu = document.getElementById('menu');
var menuToggle = function (e) {
    if (menu.style.display == "none") {
        menu.style.display = "flex";
    } else {
        menu.style.display = "none";
    }
};


for (var i = 0, max = backTiles.length; i < max; i++) {
    backTiles[i].onclick = function () {
        hideSubmenus();
        menuToggle();
    };


}

//indexcard-submenu toggle
document.getElementById('indexcard-submenu-tile').onclick = function (e) {
    menuToggle();
    var folderSubMenu = document.getElementById('indexcard-submenu');
    if (folderSubMenu.style.display == "none") {
        folderSubMenu.style.display = "flex";
    } else
        folderSubMenu.style.display = "none";
};