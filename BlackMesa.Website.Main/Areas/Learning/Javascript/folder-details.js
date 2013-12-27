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

//create-submenu toggle
document.getElementById('create-submenu-tile').onclick = function (e) {
    menuToggle();
    var createSubMenu = document.getElementById('create-submenu');
    if (createSubMenu.style.display == "none") {
        createSubMenu.style.display = "flex";
    } else {
        createSubMenu.style.display = "none";
    }
};

//folder-submenu toggle
document.getElementById('folder-submenu-tile').onclick = function (e) {
    menuToggle();
    var folderSubMenu = document.getElementById('folder-submenu');
    if (folderSubMenu.style.display == "none") {
        folderSubMenu.style.display = "flex";
    } else
        folderSubMenu.style.display = "none";
};