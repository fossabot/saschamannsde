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

/// <reference path="site.js" />
(function ($) {
    $(document).ready(function () {

        var $openSourceList = $("#openSourceList");
        if ($openSourceList.length > 0) {
            var template = _.template(
                "<div class='e-card'>" +
                "<div tabindex='0' class='e-card-about' id='<%= name %>'>" +
                "   <div class='card-header'>" +
                "     <div class='e-card-header-caption'>" +
                "       <h4 class='e-card-header-caption-title'><a href='<%= html_url %>' target'_blank'><%= name %></a></h4>" +
                "     </div>" +
                "   </div>" +
                "   <div class='e-card-content font-gray'>" +
                "           <div class='font-gray'><%= description %></div>" +
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