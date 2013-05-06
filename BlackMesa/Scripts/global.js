
/* Back to top */

//<![CDATA[
$(document).ready(function () {
    // hide #back-top 
    $("#back-to-top").hide();

    // fade in #back-top
    $(function () {
        $(window).scroll(function () {
            if ($(this).scrollTop() > 200) {
                $('#back-to-top').fadeIn();
            } else {
                $('#back-to-top').fadeOut();
            }
        });

        // scroll body to 0px 
        $('#back-to-top a').click(function () {
            $('body,html').animate({
                scrollTop: 0
            }, 800);
            return false;
        });
    });
});
//]]>


/* Tag Handler */

var onTagClick = function (selectedTags) {

    $('#filter-by-tag-input').empty();
    $('#filter-by-tag-input').append('<ul class="tag-input"></ul>');
    initTagHandler(selectedTags, true);
};

var sendAjaxRequest = function () {
    var data = { SelectedTags: $(".tag-input").tagHandler("getSerializedTags") };
    $.ajax({
        type: "POST",
        url: '/de/Entry/Index',
        data: data,
        success: function (htmlString) {
            $("#content").html(htmlString);
            $(".ui-autocomplete").hide();
        },
        dataType: 'html',
    });
};

var initTagHandler = function (selectedTags, initWithInitialAjax) {
    $(".tag-input").tagHandler({
        getData: { selectedTags: selectedTags },
        getURL: '/de/Tag/JsonIndex',
        afterAdd: sendAjaxRequest,
        afterDelete: sendAjaxRequest,
        allowAdd: false,
        msgNoNewTag: "Please select an existing tag from the list.",
        autocomplete: true,
        initWithInitialAjax: initWithInitialAjax,
    });

};

initTagHandler("", false);