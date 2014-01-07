var showBackSideTile = document.getElementById('show-backside');
var backSideArea = document.getElementById('backside');
backSideArea.style.display = "none";

showBackSideTile.onclick = function(e) {
    showBackSideTile.style.display = "none";
    backSideArea.style.display = "block";
};