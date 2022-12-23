// MIT License
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

// Site-wide JavaScript

// Search Form
(function ($) {
    $("#searchForm").submit(function (e) {
        window.location = "/search/" + encodeURI($("#search").val());
        e.preventDefault();
        return false;
    });
    //var $menu = $("#menu");
    //$("#toggle-menu").on("click", function (e) {
    //  $menu.toggleClass("shown");
    //});

})(jQuery);

//document.addEventListener('DOMContentLoaded', function () {
//    // Create instances for sidebar element
//    var defaultSidebar = document.getElementById("default-sidebar").ej2_instances[0];
//    // Show and Hide the sidebar
//    document.getElementById('toggle').onclick = function () {
//        var togglebtn = document.getElementById("toggle").ej2_instances[0];
//        if (document.getElementById('toggle').classList.contains('e-active')) {
//            togglebtn.content = 'Close';
//            defaultSidebar.show();
//        } else {
//            togglebtn.content = 'Open';
//            defaultSidebar.hide();
//        }
//    }
//    // Close the sidebar
//    document.getElementById('close').onclick = function () {
//        var togglebtn = document.getElementById("toggle").ej2_instances[0];
//        defaultSidebar.hide();
//        document.getElementById('toggle').classList.remove('e-active');
//        togglebtn.content = 'Open'
//    }
//});