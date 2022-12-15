﻿// Copyright (C) 2021 Sascha Manns <Sascha.Manns@outlook.de>
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
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MannsBlog.EntityFramework.Entities;
using MannsBlog.Repositories;
using MannsBlog.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WilderMinds.RssSyndication;

namespace MannsBlog.Controllers.Web
{

    [Route("")]
    public class RootController : Controller
    {
        private readonly int MyYear = DateTime.Now.Year;
        readonly int _pageSize = 15;

        private IMannsRepository _repo;
        private IMemoryCache _memoryCache;
        private ILogger<RootController> _logger;
        private IConfiguration _config;
        private AdService _adService;
        private readonly IMailService _mailService;

        public RootController(IMannsRepository repo,
                              IMailService mailService,
                              IMemoryCache memoryCache,
                              ILogger<RootController> logger,
                              AdService adService,
                              IConfiguration config)
        {
            _repo = repo;
            _memoryCache = memoryCache;
            _config = config;
            _mailService = mailService;
            _logger = logger;
            _adService = adService;
        }

        #region Main
        [HttpGet("")]
        public Task<IActionResult> Index()
        {
            return Pager(1);
        }

        [HttpGet("blog/{page:int?}")]
        public async Task<IActionResult> Pager(int page = 1)
        {
            return View("~/Views/Root/Index.cshtml", await _repo.GetStories(_pageSize, page));
        }

        [HttpGet("{year:int}/{month:int}/{day:int}/{slug}")]
        public async Task<IActionResult> Story(int year, int month, int day, string slug)
        {
            var fullSlug = $"{year}/{month}/{day}/{slug}";

            try
            {
                var story = await _repo.GetStory(fullSlug);

                // Try with other slug if it doesn't work
                if (story == null) story = await _repo.GetStory($"{year:0000}/{month:00}/{day:00}/{slug}");

                if (story != null)
                {
                    FixSyntaxes(story);
                    return View(story);
                }
            }
            catch
            {
                _logger.LogWarning($"Couldn't find the ${fullSlug} story");
            }

            return Redirect("/");

        }

        private void FixSyntaxes(BlogStory story)
        {
            var html = story.Body;
            if (Regex.IsMatch(html, "<pre(.*)>(.*)<code>(.*)", RegexOptions.IgnoreCase))
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                var pres = doc.DocumentNode.SelectNodes("//pre");
                foreach (var pre in pres)
                {
                    var code = pre.FirstChild;
                    if (code != null && !code.Attributes.Contains("class")) code.Attributes.Add("class", "lang-none");
                }
                story.Body = doc.DocumentNode.OuterHtml;
            };
        }
        #endregion

        #region Redirect
        //[HttpGet("download")]
        //public IActionResult Download()
        //{
        //    return Redirect("")
        //}
        #endregion

        #region Errors & Exceptions
        [HttpGet("Error/{code:int}")]
        public IActionResult Error(int errorCode)
        {
            if (Response.StatusCode == (int)HttpStatusCode.NotFound ||
                errorCode == (int)HttpStatusCode.NotFound ||
                Request.Path.Value!.EndsWith("404"))
            {
                return View("NotFound");
            }

            return View();
        }

        [HttpGet("Exception")]
        public async Task<IActionResult> Exception()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var request = HttpContext.Features.Get<IHttpRequestFeature>();

            var botWhitelist = new string[]
            {
                "sellers.json",
                "ads.txt",
                "docs/youtube_dlhelper",
                "docs/blog/ExpandAll.png",
                ".env",
                "wp-includes/wlwmanifest.xml",
                "xmlrpc.php",
                "bookmarks",
                "wp-login.php",
                "wp-admin.php",
                "sitemap.xml",
                "wordpress",
                "lcv",
                "docs/publican_creators",
                "saschamanns@outlookde.crt",
                "hoe-reek",
                "DebugWarmUp"
            };

            bool matchWhitelist = false;
            if (botWhitelist.Contains(request.RawTarget))
            {
                matchWhitelist = true;
            }

            if (exception != null && request != null && matchWhitelist == false)
            {
                var message = $@"<p>RequestUrl: {request.RawTarget}</p>
<p>Exception: ${exception.Error}</p>";

                await _mailService.SendMailAsync("logmessage.txt", "Sascha Manns", "Sascha.Manns@outlook.de", "[MannsBlog Exception]", message);
            }

            return View();
        }
        #endregion

        #region Views
        [HttpGet("about")]
        public IActionResult About()
        {
            return View();
        }

        [HttpGet("rss")]
        public IActionResult Rss()
        {
            return Redirect("http://feeds.feedburner.com/saigkills-backtrace");
        }

        [HttpGet("testerror")]
        public IActionResult TestError()
        {
            throw new InvalidOperationException("Failure");
        }

        [HttpGet("imprint")]
        public IActionResult Imprint()
        {
            return View();
        }

        [HttpGet("privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet("curriculum_vitae")]
        public IActionResult CurriculumVitae()
        {
            return View();
        }

        [HttpGet("testimonials")]
        public IActionResult Testimonials()
        {
            return View();
        }
        #endregion        

        #region Set Language
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = System.DateTimeOffset.UtcNow.AddYears(1) }
            );
            return LocalRedirect(returnUrl);
        }
        #endregion

        #region Feeds
        [HttpGet("feed")]
        public async Task<IActionResult> Feed()
        {
            var feed = new Feed()
            {
                Title = "Sascha Manns's Twilight Zone - English Feed",
                Description = "Blog about Linux, Windows (WSL, Insider), Programming (Ruby, Python, Java, Android ASP and Mono/.NET) and other random stuff",
                Link = new Uri("https://saschamanns.de/feed"),
                Copyright = "©" + " " + MyYear + " " + " Sascha Manns"
            };

            var license = @"<div>
        <div style=""float: left;"">
          <a rel=""license"" href=""https://creativecommons.org/licenses/by-sa/3.0/de/deed.en"">
            <img alt=""Creative Commons License"" style=""border-width: 0"" src=""https://i.creativecommons.org/l/by-sa/3.0/de/88x31.png"" /></a></div>
        <div>
          This work by <a xmlns:cc=""http://creativecommons.org/ns#"" href=""https://saschamanns.de""
            property=""cc:attributionName"" rel=""cc:attributionURL"">Sascha Manns</a> is
          licensed under a <a rel=""license"" href=""https://creativecommons.org/licenses/by-sa/3.0/de/deed.en/"">
            Attribution-ShareAlike 3.0 Germany License (CC BY-SA 3.0 DE)</a>.<br />
          Based on a work at <a xmlns:dct=""https://purl.org/dc/terms/"" href=""https://saschamanns.de""
            rel=""dct:source"">saschamanns.de</a>.</div>
        </div>";

            var ad = "If you liked this article: " + _adService.InlineAdd();
            var entries = await _repo.GetStoriesByTag("en-US", 25, 1);

            foreach (var entry in entries.Stories)
            {
                var item = new Item()
                {
                    Title = entry.Title,
                    Body = string.Concat(entry.Body, license, ad), // Removed ad
                    Link = new Uri(new Uri(Request.GetEncodedUrl()), entry.Slug),
                    Permalink = new Uri(new Uri(Request.GetEncodedUrl()), entry.Slug).ToString(),
                    PublishDate = entry.DatePublished,
                    Author = new Author() { Name = "Sascha Manns", Email = "Sascha.Manns@outlook.de" }
                };

                foreach (var cat in entry.Categories.Split(','))
                {
                    item.Categories.Add(cat);
                }
                feed.Items.Add(item);
            }

            return File(Encoding.UTF8.GetBytes(feed.Serialize()), "text/xml");

        }

        [HttpGet("feed-de")]
        public async Task<IActionResult> FeedDe()
        {
            var feed = new Feed()
            {
                Title = "Sascha Manns's Twilight Zone - German Feed",
                Description = "Blog about Linux, Windows (WSL, Insider), Programming (Ruby, Python, Java, Android ASP and Mono/.NET) and other random stuff",
                Link = new Uri("https://saschamanns.de/feed-de"),
                Copyright = "©" + " " + MyYear + " " + " Sascha Manns"
            };

            var license = @"<div>
        <div style=""float: left;"">
          <a rel=""license"" href=""https://creativecommons.org/licenses/by-sa/3.0/de/deed.en"">
            <img alt=""Creative Commons License"" style=""border-width: 0"" src=""https://i.creativecommons.org/l/by-sa/3.0/de/88x31.png"" /></a></div>
        <div>
          This work by <a xmlns:cc=""http://creativecommons.org/ns#"" href=""https://saschamanns.de""
            property=""cc:attributionName"" rel=""cc:attributionURL"">Sascha Manns</a> is
          licensed under a <a rel=""license"" href=""https://creativecommons.org/licenses/by-sa/3.0/de/deed.en/"">
            Attribution-ShareAlike 3.0 Germany License (CC BY-SA 3.0 DE)</a>.<br />
          Based on a work at <a xmlns:dct=""https://purl.org/dc/terms/"" href=""https://saschamanns.de""
            rel=""dct:source"">saschamanns.de</a>.</div>
        </div>";

            var ad = "Wenn Ihnen dieser Artikel gefallen hat: " + _adService.InlineAdd();
            var entries = await _repo.GetStoriesByTag("de-DE", 25, 1);

            foreach (var entry in entries.Stories)
            {
                var item = new Item()
                {
                    Title = entry.Title,
                    Body = string.Concat(entry.Body, license, ad), // Removed ad
                    Link = new Uri(new Uri(Request.GetEncodedUrl()), entry.Slug),
                    Permalink = new Uri(new Uri(Request.GetEncodedUrl()), entry.Slug).ToString(),
                    PublishDate = entry.DatePublished,
                    Author = new Author() { Name = "Sascha Manns", Email = "Sascha.Manns@outlook.de" }
                };

                foreach (var cat in entry.Categories.Split(','))
                {
                    item.Categories.Add(cat);
                }
                feed.Items.Add(item);
            }

            return File(Encoding.UTF8.GetBytes(feed.Serialize()), "text/xml");
        }

        [HttpGet("opensource-de")]
        public async Task<IActionResult> OpensourceDe()
        {
            var feed = new Feed()
            {
                Title = "Sascha Manns's Twilight Zone - German Opensource Feed",
                Description = "Blog about Linux, Windows (WSL, Insider), Programming (Ruby, Python, Java, Android ASP and Mono/.NET) and other random stuff",
                Link = new Uri("https://saschamanns.de/opensource-de"),
                Copyright = "©" + " " + MyYear + " " + " Sascha Manns"
            };

            var license = @"<div>
        <div style=""float: left;"">
          <a rel=""license"" href=""https://creativecommons.org/licenses/by-sa/3.0/de/deed.en"">
            <img alt=""Creative Commons License"" style=""border-width: 0"" src=""https://i.creativecommons.org/l/by-sa/3.0/de/88x31.png"" /></a></div>
        <div>
          This work by <a xmlns:cc=""http://creativecommons.org/ns#"" href=""https://saschamanns.de""
            property=""cc:attributionName"" rel=""cc:attributionURL"">Sascha Manns</a> is
          licensed under a <a rel=""license"" href=""https://creativecommons.org/licenses/by-sa/3.0/de/deed.en/"">
            Attribution-ShareAlike 3.0 Germany License (CC BY-SA 3.0 DE)</a>.<br />
          Based on a work at <a xmlns:dct=""https://purl.org/dc/terms/"" href=""https://saschamanns.de""
            rel=""dct:source"">saschamanns.de</a>.</div>
        </div>";

            var ad = "If you liked this article: " + _adService.InlineAdd();
            var entries = await _repo.GetStoriesByTag("OpensourceDE", 25, 1);

            foreach (var entry in entries.Stories)
            {
                var item = new Item()
                {
                    Title = entry.Title,
                    Body = string.Concat(entry.Body, license, ad), // Removed ad
                    Link = new Uri(new Uri(Request.GetEncodedUrl()), entry.Slug),
                    Permalink = new Uri(new Uri(Request.GetEncodedUrl()), entry.Slug).ToString(),
                    PublishDate = entry.DatePublished,
                    Author = new Author() { Name = "Sascha Manns", Email = "Sascha.Manns@outlook.de" }
                };

                foreach (var cat in entry.Categories.Split(','))
                {
                    item.Categories.Add(cat);
                }
                feed.Items.Add(item);
            }

            return File(Encoding.UTF8.GetBytes(feed.Serialize()), "text/xml");
        }

        [HttpGet("opensource")]
        public async Task<IActionResult> Opensource()
        {
            var feed = new Feed()
            {
                Title = "Sascha Manns's Twilight Zone - English Opensource Feed",
                Description = "Blog about Linux, Windows (WSL, Insider), Programming (Ruby, Python, Java, Android ASP and Mono/.NET) and other random stuff",
                Link = new Uri("https://saschamanns.de/opensource"),
                Copyright = "©" + " " + MyYear + " " + " Sascha Manns"
            };

            var license = @"<div>
        <div style=""float: left;"">
          <a rel=""license"" href=""https://creativecommons.org/licenses/by-sa/3.0/de/deed.en"">
            <img alt=""Creative Commons License"" style=""border-width: 0"" src=""https://i.creativecommons.org/l/by-sa/3.0/de/88x31.png"" /></a></div>
        <div>
          This work by <a xmlns:cc=""http://creativecommons.org/ns#"" href=""https://saschamanns.de""
            property=""cc:attributionName"" rel=""cc:attributionURL"">Sascha Manns</a> is
          licensed under a <a rel=""license"" href=""https://creativecommons.org/licenses/by-sa/3.0/de/deed.en/"">
            Attribution-ShareAlike 3.0 Germany License (CC BY-SA 3.0 DE)</a>.<br />
          Based on a work at <a xmlns:dct=""https://purl.org/dc/terms/"" href=""https://saschamanns.de""
            rel=""dct:source"">saschamanns.de</a>.</div>
        </div>";
            //var ad = @"<hr/><div>If you liked this article, so <a target=""_blank"" href=""https://www.buymeacoffee.com/PE0y8DF""><img src=""~/img/misc/buymeacoffee.jpg"" alt=""Buy me a coffee"" width=""12%""></a>.</div>";

            var ad = "If you liked this article: " + _adService.InlineAdd();
            var entries = await _repo.GetStoriesByTag("Opensource", 25, 1);

            foreach (var entry in entries.Stories)
            {
                var item = new Item()
                {
                    Title = entry.Title,
                    Body = string.Concat(entry.Body, license, ad), // Removed ad
                    Link = new Uri(new Uri(Request.GetEncodedUrl()), entry.Slug),
                    Permalink = new Uri(new Uri(Request.GetEncodedUrl()), entry.Slug).ToString(),
                    PublishDate = entry.DatePublished,
                    Author = new Author() { Name = "Sascha Manns", Email = "Sascha.Manns@outlook.de" }
                };

                foreach (var cat in entry.Categories.Split(','))
                {
                    item.Categories.Add(cat);
                }
                feed.Items.Add(item);
            }

            return File(Encoding.UTF8.GetBytes(feed.Serialize()), "text/xml");
        }

        [HttpGet("linux")]
        public async Task<ActionResult> Linux()
        {
            var feed = new Feed()
            {
                Title = "Sascha Manns's Twilight Zone - Linux Feed (German and English)",
                Description = "Blog about Linux, Windows (WSL, Insider), Programming (Ruby, Python, Java, Android ASP and Mono/.NET) and other random stuff",
                Link = new Uri("https://saschamanns.de/linux"),
                Copyright = "©" + " " + MyYear + " " + " Sascha Manns"
            };

            var license = @"<div>
        <div style=""float: left;"">
          <a rel=""license"" href=""https://creativecommons.org/licenses/by-sa/3.0/de/deed.en"">
            <img alt=""Creative Commons License"" style=""border-width: 0"" src=""https://i.creativecommons.org/l/by-sa/3.0/de/88x31.png"" /></a></div>
        <div>
          This work by <a xmlns:cc=""http://creativecommons.org/ns#"" href=""https://saschamanns.de""
            property=""cc:attributionName"" rel=""cc:attributionURL"">Sascha Manns</a> is
          licensed under a <a rel=""license"" href=""https://creativecommons.org/licenses/by-sa/3.0/de/deed.en/"">
            Attribution-ShareAlike 3.0 Germany License (CC BY-SA 3.0 DE)</a>.<br />
          Based on a work at <a xmlns:dct=""https://purl.org/dc/terms/"" href=""https://saschamanns.de""
            rel=""dct:source"">saschamanns.de</a>.</div>
        </div>";

            var ad = "If you liked this article: " + _adService.InlineAdd();
            var entries = await _repo.GetStoriesByTag("Linux", 25, 1);

            foreach (var entry in entries.Stories)
            {
                var item = new Item()
                {
                    Title = entry.Title,
                    Body = string.Concat(entry.Body, license, ad), // Removed ad
                    Link = new Uri(new Uri(Request.GetEncodedUrl()), entry.Slug),
                    Permalink = new Uri(new Uri(Request.GetEncodedUrl()), entry.Slug).ToString(),
                    PublishDate = entry.DatePublished,
                    Author = new Author() { Name = "Sascha Manns", Email = "Sascha.Manns@outlook.de" }
                };

                foreach (var cat in entry.Categories.Split(','))
                {
                    item.Categories.Add(cat);
                }
                feed.Items.Add(item);
            }

            return File(Encoding.UTF8.GetBytes(feed.Serialize()), "text/xml");
        }

        [HttpGet("windows")]
        public async Task<IActionResult> Windows()
        {
            var feed = new Feed()
            {
                Title = "Sascha Manns's Twilight Zone - Windows Feed (German and English)",
                Description = "Blog about Linux, Windows (WSL, Insider), Programming (Ruby, Python, Java, Android ASP and Mono/.NET) and other random stuff",
                Link = new Uri("https://saschamanns.de/windows"),
                Copyright = "©" + " " + MyYear + " " + " Sascha Manns"
            };

            var license = @"<div>
        <div style=""float: left;"">
          <a rel=""license"" href=""https://creativecommons.org/licenses/by-sa/3.0/de/deed.en"">
            <img alt=""Creative Commons License"" style=""border-width: 0"" src=""https://i.creativecommons.org/l/by-sa/3.0/de/88x31.png"" /></a></div>
        <div>
          This work by <a xmlns:cc=""http://creativecommons.org/ns#"" href=""https://saschamanns.de""
            property=""cc:attributionName"" rel=""cc:attributionURL"">Sascha Manns</a> is
          licensed under a <a rel=""license"" href=""https://creativecommons.org/licenses/by-sa/3.0/de/deed.en/"">
            Attribution-ShareAlike 3.0 Germany License (CC BY-SA 3.0 DE)</a>.<br />
          Based on a work at <a xmlns:dct=""https://purl.org/dc/terms/"" href=""https://saschamanns.de""
            rel=""dct:source"">saschamanns.de</a>.</div>
        </div>";

            var ad = "If you liked this article: " + _adService.InlineAdd();
            var entries = await _repo.GetStoriesByTag("Windows", 25, 1);

            foreach (var entry in entries.Stories)
            {
                var item = new Item()
                {
                    Title = entry.Title,
                    Body = string.Concat(entry.Body, license, ad), // Removed ad
                    Link = new Uri(new Uri(Request.GetEncodedUrl()), entry.Slug),
                    Permalink = new Uri(new Uri(Request.GetEncodedUrl()), entry.Slug).ToString(),
                    PublishDate = entry.DatePublished,
                    Author = new Author() { Name = "Sascha Manns", Email = "Sascha.Manns@outlook.de" }
                };

                foreach (var cat in entry.Categories.Split(','))
                {
                    item.Categories.Add(cat);
                }
                feed.Items.Add(item);
            }

            return File(Encoding.UTF8.GetBytes(feed.Serialize()), "text/xml");
        }

        [HttpGet("dotnetcore")]
        public async Task<IActionResult> DotNetCore()
        {
            var feed = new Feed()
            {
                Title = "Sascha Manns's Twilight Zone - Dotnetcore Feed (German and English)",
                Description = "Blog about Linux, Windows (WSL, Insider), Programming (Ruby, Python, Java, Android ASP and Mono/.NET) and other random stuff",
                Link = new Uri("https://saschamanns.de/dotnetcore"),
                Copyright = "©" + " " + MyYear + " " + " Sascha Manns"
            };

            var license = @"<div>
        <div style=""float: left;"">
          <a rel=""license"" href=""https://creativecommons.org/licenses/by-sa/3.0/de/deed.en"">
            <img alt=""Creative Commons License"" style=""border-width: 0"" src=""https://i.creativecommons.org/l/by-sa/3.0/de/88x31.png"" /></a></div>
        <div>
          This work by <a xmlns:cc=""http://creativecommons.org/ns#"" href=""https://saschamanns.de""
            property=""cc:attributionName"" rel=""cc:attributionURL"">Sascha Manns</a> is
          licensed under a <a rel=""license"" href=""https://creativecommons.org/licenses/by-sa/3.0/de/deed.en/"">
            Attribution-ShareAlike 3.0 Germany License (CC BY-SA 3.0 DE)</a>.<br />
          Based on a work at <a xmlns:dct=""https://purl.org/dc/terms/"" href=""https://saschamanns.de""
            rel=""dct:source"">saschamanns.de</a>.</div>
        </div>";
            //var ad = @"<hr/><div>If you liked this article, so <a target=""_blank"" href=""https://www.buymeacoffee.com/PE0y8DF""><img src=""~/img/misc/buymeacoffee.jpg"" alt=""Buy me a coffee"" width=""12%""></a>.</div>";

            var ad = "If you liked this article: " + _adService.InlineAdd();
            var entries = await _repo.GetStoriesByTag("DotNetCore", 25, 1);

            foreach (var entry in entries.Stories)
            {
                var item = new Item()
                {
                    Title = entry.Title,
                    Body = string.Concat(entry.Body, license, ad), // Removed ad
                    Link = new Uri(new Uri(Request.GetEncodedUrl()), entry.Slug),
                    Permalink = new Uri(new Uri(Request.GetEncodedUrl()), entry.Slug).ToString(),
                    PublishDate = entry.DatePublished,
                    Author = new Author() { Name = "Sascha Manns", Email = "Sascha.Manns@outlook.de" }
                };

                foreach (var cat in entry.Categories.Split(','))
                {
                    item.Categories.Add(cat);
                }
                feed.Items.Add(item);
            }

            return File(Encoding.UTF8.GetBytes(feed.Serialize()), "text/xml");
        }
        #endregion
    }
}
