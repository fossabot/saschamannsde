﻿// MIT License
//
// Copyright (c) 2022 Sascha Manns
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

// video.js
(function ($) {
    $(".video-toggle").on("click", function () {
        let $btn = $(this);
        let $video = $("#video-" + $btn.attr("data-id"));
        if ($btn.text() == "Show Video") {
            $btn.text("Hide Video");
            loadVideo($video);
            $video.show(0);
        } else {
            $btn.text("Show Video");
            $video.hide(0);
        }
    });

    function loadVideo($video) {
        let $iframe = $video.find("iframe");
        if (!$iframe.is('[src]')) {
            $iframe.attr("src", $iframe.attr("data-src"));
        }
    }

    $(document).ready(function () {
        let $video = $(".video-container");
        if ($video.hasClass("auto-load")) loadVideo($video);
    });
})(jQuery);