var showAnswerTile = document.getElementById('show-answer');
var answerArea = document.getElementById('answer');
answerArea.style.display = "none";

showAnswerTile.onclick = function(e) {
    showAnswerTile.style.display = "none";
    answerArea.style.display = "block";
};