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