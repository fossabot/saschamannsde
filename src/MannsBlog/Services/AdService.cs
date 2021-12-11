// Copyright (C) 2021 Sascha Manns <Sascha.Manns@outlook.de>
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using MannsBlog.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MannsBlog.Services
{
    public class AdService
    {
        private IConfiguration _config;
        private readonly ILogger<AdService> _logger;
        //private static readonly string ad1 =
        //    @"<script type=""text/javascript"" src=""https://cdnjs.buymeacoffee.com/1.0.0/button.prod.min.js"" data-name=""bmc-button"" data-slug=""PE0y8DF"" data-color=""#FFDD00"" data-emoji=""""  data-font=""Cookie"" data-text=""Buy me a coffee"" data-outline-color=""#000000"" data-font-color=""#000000"" data-coffee-color=""#ffffff"" ></script>";

        private static readonly string ad2 =
            @"<a href=""https://liberapay.com/saigkill/donate"" target=""_blank""><img alt=""Donate using Liberapay"" src=""https://liberapay.com/assets/widgets/donate.svg"" width=""80%""></a>";

        private static readonly string ad3 =
            @"<a href=""https://www.patreon.com/bePatron?u=20278446"" data-patreon-widget-type=""become-patron-button"">Become a Patron!</a><script async src=""https://c6.patreon.com/becomePatronButton.bundle.js""></script>";

        private static readonly string ad4 =
            @"<a href=""https://www.amazon.de/registry/wishlist/D75HOEQ00BDD"" target=""_blank""><img src=""/img/misc/amazon.svg"" width=""100%""/></a>";

        private static readonly string ad5 = @"<a href=""https://paypal.me/SaschaManns"" target=""_blank""><img src=""/img/misc/paypal_donate.svg"" width=""100%""/></a>";

        private static readonly string ad6 = @"<a href=""https://github.com/saigkill target=""_blank""><img src=""https://img.shields.io/github/followers/saigkill?style=social""/></a>";

        private static readonly string ad = @$"<table class=""tg""><tbody><tr><td class=""tg-01ax"">{ad2}</td><td class=""tg-01ax"">{ad3}</td><td class=""tg-01ax"">{ad4}</td><td class=""tg-01ax"">{ad5}</td><td class=""tg-01ax"">{ad6}</td></tr></tbody></table>";

        public AdService(IConfiguration config, ILogger<AdService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public HtmlString InlineAdd()
        {
            var ranges = new List<AdDateRange>()
            {
                new AdDateRange( // Fallback
                DateTime.MinValue.ToString(),
                DateTime.MaxValue.ToString(),
                ad
                )
            };
            var now = DateTime.Now;
            var ads = ranges.FirstOrDefault(r => r.Start <= now && r.End >= now);

            if (ads == null) return HtmlString.Empty;
            var item = new Random().Next(0, ads.Ads.Length);

            return new HtmlString(ads.Ads[item]);

        }

        public HtmlString SidebarAdd()
        {
            var ranges = new List<AdDateRange>()
            {
                new AdDateRange( // Fallback
                DateTime.MinValue.ToString(CultureInfo.InvariantCulture),
                DateTime.MaxValue.ToString(CultureInfo.InvariantCulture),
                  ad
                )
            };
            var now = DateTime.Now;
            var ads = ranges.FirstOrDefault(r => r.Start <= now && r.End >= now);

            if (ads == null) return HtmlString.Empty;
            var item = new Random().Next(0, ads.Ads.Length);

            return new HtmlString(ads.Ads[item]);
        }
    }
}