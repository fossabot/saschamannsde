﻿/// <reference path="site.js" />
(function ($) {
    $(document).ready(function () {

        var $openSourceList = $("#openSourceList");
        if ($openSourceList.length > 0) {
            var template = _.template(
                "<div class='single_about single_expart'>" +
                "   <div class='card-header'>" +
                "       <a href='<%= html_url %>' target='blank'><h4 class='card-subtitle'><%= name %></h4></a>" +
                "   </div>" +
                "   <div class='card-body'>" +
                "       <div class='card-text'>" +
                "           <small><%= description %></small>" +
                "       </div>" +
                "   </div>" +
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