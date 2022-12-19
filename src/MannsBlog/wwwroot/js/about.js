/// <reference path="site.js" />
(function ($) {
    $(document).ready(function () {

        var $openSourceList = $("#openSourceList");
        if ($openSourceList.length > 0) {
            var template = _.template(
                "<div class='card'>" +
                "<div tabindex='0' class='e-card-about' id='basic'>" +
                "   <div class='e-card-header'>" +
                "     <div class='e-card-header-caption'>" +
                "       <h4 class='e-card-header-caption-title'><a href='<%= html_url %>' target'_blank'><%= name %></a></h4>" +
                "     </div>" +
                "   </div>" +
                "   <div class='e-card-content'>" +
                "           <div><small><%= description %></small></div>" +
                "       </div>" +
                " </div>" +
                "</div>");
            $.get("https://api.github.com/users/saigkill/repos?type=owner&sort-updated")
                .then(function (result) {
                    var results = _.filter(result, function (item) {
                        return !item.fork;
                    });
                    results = _.orderBy(results, ["stargazers_count"], ["desc"]);
                    _.forEach(results, function (item) {
                        $openSourceList.append($(template(item)))
                    });
                });
        }
    });
})(jQuery);