var onTagClick = function (selectedTags) {

    $('#filter-by-tag').empty();
    $('#filter-by-tag').append('<ul class="tag-input"></ul>');
    initTagHandler(selectedTags, true);
};

var sendAjaxRequest = function () {
    var data = { SelectedTags: $(".tag-input").tagHandler("getSerializedTags") };
    $.ajax({
        type: "POST",
        url: '/Entry/Index',
        data: data,
        success: function (htmlString) {
            $("#entry-index-articles").html(htmlString);
            $(".ui-autocomplete").hide();
        },
        dataType: 'html',
    });
};

var initTagHandler = function (selectedTags, initWithInitialAjax) {

    $(".tag-input").tagHandler({
        getData: { selectedTags: selectedTags },
        getURL: '/Tag/JsonIndex',
        afterAdd: sendAjaxRequest,
        afterDelete: sendAjaxRequest,
        allowAdd: false,
        msgNoNewTag: "Please select an existing tag from the list.",
        autocomplete: true,
        initWithInitialAjax: initWithInitialAjax,
    });

};

initTagHandler("", false);
