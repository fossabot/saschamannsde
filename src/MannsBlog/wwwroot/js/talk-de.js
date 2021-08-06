// talk.js
(function ($) {
    $(".talk-toggle").on("click", function () {
        var $btn = $(this);
        var $talk = $("#talk-" + $btn.attr("data-id"));
        if ($btn.text() == "Vortrag ansehen") {
            $btn.text("Vortrag verstecken");
            //loadTalk($talk);
            $talk.show(0);
        } else {
            $btn.text("Vortrag ansehen");
            $talk.hide(0);
        }
    });

    function loadTalk($talk) {
        var $iframe = $talk.find("iframe");
        if (!$iframe.is('[src]')) {
            $iframe.attr("src", $iframe.attr("data-src"));
        }
    }

    $(document).ready(function () {
        var $talk = $(".talk-container");
        if ($talk.hasClass("auto-load")) loadtalk($talk);
    });
})(jQuery);