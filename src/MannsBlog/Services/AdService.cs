using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MannsBlog.Models;

namespace MannsBlog.Services
{
    public class AdService
    {
        private IConfiguration _config;
        private readonly ILogger<AdService> _logger;

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
          @"<div class=""card-text""><small>If you liked this article, so <a target=""_blank"" href=""https:www.buymeacoffee.com/PE0y8DF""><img src=""/img/misc/buymeacoffee.jpg"" alt=""Buy me a coffee"" width=""12%""></a>  :-).</small></div>"
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
          @"<div class=""card-text""><small>If you liked this article, so <a target=""_blank"" href=""https:www.buymeacoffee.com/PE0y8DF""><img src=""/img/misc/buymeacoffee.jpg"" alt=""Buy me a coffee"" width=""12%""></a>  :-).</small></div>"
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