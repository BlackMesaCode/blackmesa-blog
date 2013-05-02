
/* Disqus Comment System */

$(function() {
    var dsq = document.createElement('script');
    dsq.type = 'text/javascript';
    dsq.async = true;
    dsq.src = '//' + disqus_shortname + '.disqus.com/embed.js';
    (document.getElementsByTagName('head')[0] || document.getElementsByTagName('body')[0]).appendChild(dsq);
});




/* SyntaxHighlighter */

$(function () {

    SyntaxHighlighter.autoloader(
        'js jscript javascript            /Scripts/shBrushes/shBrushJScript.js',
        'c-sharp, csharp	                /Scripts/shBrushes/shBrushCSharp.js',
        'ps, powershell	                /Scripts/shBrushes/shBrushPowerShell.js',
        'xml, xhtml, xslt, html, xhtml	/Scripts/shBrushes/shBrushXml.js',
        'sql                              /Scripts/shBrushes/shBrushSql.js',
        'bash, shell	                    /Scripts/shBrushes/shBrushBash.js',
        'plain, text	                    /Scripts/shBrushes/shBrushPlain.js'
    );

    var elementsToBeHighlighted = SyntaxHighlighter.findElements().length,
        highlightedElements = 0;

    SyntaxHighlighter.complete = function(callback) {

        (function recountHighlightedElements() {
            setTimeout(function() {
                highlightedElements = $('.syntaxhighlighter');
                if (highlightedElements.length < elementsToBeHighlighted) {
                    recountHighlightedElements();
                } else {
                    callback();
                }
            }, 200);
        })();
    };

    SyntaxHighlighter.complete(function() {
        $(".syntaxhighlighter").parent().addClass("sh-padding-right");
    });
                
    SyntaxHighlighter.all();
});
