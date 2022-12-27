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

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MannsBlog.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MannsBlog.Services
{
    /// <summary>
    /// Ad Service.
    /// This part you can modify and use your own stuff.
    /// </summary>
    public class AdService
    {
        private readonly ILogger<AdService> _logger;

        private static readonly string Ad1 =
            @"<a href=""https://www.buymeacoffee.com/PE0y8DF"" target=""_blank"" rel=""noopener""><img alt=""Donate on Buymeacoffee"" src=""/img/badges/buymeacoffee.png""</a>";

        private static readonly string Ad2 =
            @"<a href=""https://liberapay.com/saigkill/donate"" target=""_blank""><img alt=""Donate using Liberapay"" src=""https://liberapay.com/assets/widgets/donate.svg"" width=""80%""></a>";

        private static readonly string Ad3 =
            @"<div class=""""><a href=""https://www.patreon.com/bePatron?u=20278446""</a><img src=""/img/badges/patreon.svg"" alt=""Donate on Patreon"" width:""10%"" /><div>";

        private static readonly string Ad4 =
            @"<a href=""https://www.amazon.de/registry/wishlist/D75HOEQ00BDD"" target=""_blank""><img src=""/img/badges/amazon.svg"" alt=""Show my Amazon Wishlist"" width=""100%""/></a>";

        private static readonly string Ad5 = @"<a href=""https://paypal.me/SaschaManns"" target=""_blank""><img src=""/img/badges/paypal_donate.svg"" alt=""Donate on Paypal"" width=""100%""/></a>";

        private static readonly string Ad6 = @"<a href=""https://github.com/saigkill target=""_blank""><img src=""https://img.shields.io/github/followers/saigkill?style=social""/></a>";

        private static readonly string Ad = @$"<table class=""tg""><tbody><tr><td class=""tg-01ax"">{Ad1}</td><td class=""tg-01ax"">{Ad2}</td><td class=""tg-01ax"">{Ad3}</td><td class=""tg-01ax"">{Ad4}</td><td class=""tg-01ax"">{Ad5}</td><td class=""tg-01ax"">{Ad6}</td></tr></tbody></table>";

        private IConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdService"/> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="logger">The logger.</param>
        public AdService(IConfiguration config, ILogger<AdService> logger)
        {
            _config = config;
            _logger = logger;
        }

        /// <summary>
        /// Ads for inline.
        /// </summary>
        /// <returns>Ad HTML-String.</returns>
        public HtmlString InlineAdd()
        {
            var ranges = new List<AdDateRange>()
            {
                new AdDateRange( // Fallback
                DateTime.MinValue.ToString(),
                DateTime.MaxValue.ToString(),
                Ad),
            };
            var now = DateTime.Now;
            var ads = ranges.FirstOrDefault(r => r.Start <= now && r.End >= now);

            if (ads == null)
            {
                return HtmlString.Empty;
            }

            var item = new Random().Next(0, ads.Ads.Length);

            return new HtmlString(ads.Ads[item]);
        }

        /// <summary>
        /// Sidebars the add.
        /// </summary>
        /// <returns>Ad HTML String.</returns>
        public HtmlString SidebarAdd()
        {
            var ranges = new List<AdDateRange>()
            {
                new AdDateRange( // Fallback
                DateTime.MinValue.ToString(CultureInfo.InvariantCulture),
                DateTime.MaxValue.ToString(CultureInfo.InvariantCulture),
                Ad),
            };
            var now = DateTime.Now;
            var ads = ranges.FirstOrDefault(r => r.Start <= now && r.End >= now);

            if (ads == null)
            {
                return HtmlString.Empty;
            }

            var item = new Random().Next(0, ads.Ads.Length);

            return new HtmlString(ads.Ads[item]);
        }
    }
}