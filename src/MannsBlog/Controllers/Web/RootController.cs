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
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WilderMinds.RssSyndication;

namespace MannsBlog.Controllers.Web
{
    /// <summary>
    /// Main-Controller.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Route("")]
    public class RootController : Controller
    {
        private readonly int myYear = DateTime.Now.Year;
        readonly int _pageSize = 15;

        private IMannsRepository _repo;
        private IMemoryCache _memoryCache;
        private ILogger<RootController> _logger;
        private IConfiguration _config;
        private AdService _adService;
        private readonly IMailService _mailService;
        private readonly IViewLocalizer _localizer;
        private readonly string _firstname;
        private readonly string _surname;
        private readonly string _name;
        private readonly string _email;
        private readonly string _url;
        private readonly GoogleCaptchaService _captcha;

        /// <summary>
        /// Initializes a new instance of the <see cref="RootController"/> class.
        /// </summary>
        /// <param name="repo">The repo.</param>
        /// <param name="mailService">The mail service.</param>
        /// <param name="memoryCache">The memory cache.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="adService">The ad service.</param>
        /// <param name="config">The configuration.</param>
        /// <param name="localizer">The localizer.</param>
        public RootController(IMannsRepository repo,
                              IMailService mailService,
                              IMemoryCache memoryCache,
                              ILogger<RootController> logger,
                              AdService adService,
                              IConfiguration config,
                              GoogleCaptchaService captcha)
        {
            _repo = repo;
            _memoryCache = memoryCache;
            _config = config;
            _mailService = mailService;
            _logger = logger;
            _adService = adService;
            _captcha = captcha;

#pragma warning disable CS8601 // Mögliche Nullverweiszuweisung.
            _firstname = _config["Blog:UserFirstname"];
            _surname = _config["Blog:UserSurname"];
            _name = _firstname + " " + _surname;
            _email = _config["Blog:Email"];

            bool useHttps = bool.Parse(_config["Blog:UseHttps"]);
            if (useHttps)
            {
                _url = _config["Blog:HTTPSUrl"];
            }
            else
            {
                _url = _config["Blog:HTTPUrl"];
            }
#pragma warning restore CS8601 // Mögliche Nullverweiszuweisung.
        }

        #region Main

        /// <summary>
        /// Returns the Index with pager.
        /// </summary>
        /// <returns>Index.</returns>
        [HttpGet("")]
        public Task<IActionResult> Index()
        {
            return Pager(1);
        }

        /// <summary>
        /// Returns the pagesd (calucalted) View.
        /// </summary>
        /// <param name="page">Pagenumber.</param>
        /// <returns>View.</returns>
        [HttpGet("blog/{page:int?}")]
        public async Task<IActionResult> Pager(int page = 1)
        {
            return View("~/Views/Root/Index.cshtml", await _repo.GetStories(_pageSize, page));
        }

        /// <summary>
        /// Get the view with the blog story.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <param name="slug">The slug.</param>
        /// <returns>Story View.</returns>
        [HttpGet("{year:int}/{month:int}/{day:int}/{slug}")]
        public async Task<IActionResult> Story(int year, int month, int day, string slug)
        {
            var fullSlug = $"{year}/{month}/{day}/{slug}";

            try
            {
                var story = await _repo.GetStory(fullSlug);

                // Try with other slug if it doesn't work
                if (story == null)
                {
                    story = await _repo.GetStory($"{year:0000}/{month:00}/{day:00}/{slug}");
                }

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

        /// <summary>
        /// Fixes the syntaxes.
        /// </summary>
        /// <param name="story">The story.</param>
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
                    if (code != null && !code.Attributes.Contains("class"))
                    {
                        code.Attributes.Add("class", "lang-none");
                    }
                }

                story.Body = doc.DocumentNode.OuterHtml;
            }
        }

        #endregion

        #region Redirect
        // [HttpGet("download")]
        // public IActionResult Download()
        // {
        //    return Redirect("")
        // }
        #endregion

        #region Errors & Exceptions

        /// <summary>
        /// Returns a Error View.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <returns>ErrorView.</returns>
        [HttpGet("Error/{code:int}")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:Elements should be ordered by access", Justification = "<Ausstehend>")]
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

        /// <summary>
        /// Returns Exception Site.
        /// </summary>
        /// <returns>Exception.</returns>
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
                "DebugWarmUp",
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

                await _mailService.SendMailAsync("logmessage.txt", _name, _email, "[MannsBlog Exception]", message);
            }

            return View();
        }
        #endregion

        #region Views

        /// <summary>
        /// The About-Me View.
        /// </summary>
        /// <returns>View.</returns>
        [HttpGet("about")]
        public IActionResult About()
        {
            return View();
        }

        /// <summary>
        /// Redirects to Feedburner.
        /// </summary>
        /// <returns>Redirect.</returns>
        [HttpGet("rss")]
        public IActionResult Rss()
        {
            return Redirect("http://saschamanns.de/feed");
        }

        /// <summary>
        /// Tests the error.
        /// </summary>
        /// <returns>InvalidOperationException.</returns>
        /// <exception cref="System.InvalidOperationException">Failure.</exception>
        [HttpGet("testerror")]
        public IActionResult TestError()
        {
            throw new InvalidOperationException("Failure");
        }

        /// <summary>
        /// Its the imprint.
        /// </summary>
        /// <returns>Imprint-View.</returns>
        [HttpGet("imprint")]
        public IActionResult Imprint()
        {
            return View();
        }

        /// <summary>
        /// The Privacy Policy.
        /// </summary>
        /// <returns>Privacy View.</returns>
        [HttpGet("privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// My Curriculum Vitae Site.
        /// </summary>
        /// <returns>CurriculumVitae View.</returns>
        [HttpGet("curriculum_vitae")]
        public IActionResult CurriculumVitae()
        {
            return View();
        }

        /// <summary>
        /// My testimonial site.
        /// </summary>
        /// <returns>Testimonials Site.</returns>
        [HttpGet("testimonials")]
        public IActionResult Testimonials()
        {
            return View();
        }

        /// <summary>
        /// Returns the Index.
        /// </summary>
        /// <returns>Contact View.</returns>
        [HttpGet("contact")]
        public IActionResult Contact()
        {
            return View();
        }
        #endregion

        #region Set Language

        /// <summary>
        /// Sets the language.
        /// </summary>
        /// <param name="culture">The culture.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>Localisation Url.</returns>
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = System.DateTimeOffset.UtcNow.AddYears(1) });
            return LocalRedirect(returnUrl);
        }
        #endregion

        #region Feeds

        // TODO: You have to modify this feeds to match your categories.

        /// <summary>
        /// This is the main feed including all categories in english.
        /// </summary>
        /// <returns>RSS-Feed.</returns>
        [HttpGet("feed")]
        public async Task<IActionResult> Feed()
        {
            var feed = new Feed()
            {
                Title = "Sascha Manns's Twilight Zone - English Feed",
                Description = "Blog about Linux, Windows (WSL, Insider), Programming (Ruby, Python, Java, Android ASP and Mono/.NET) and other random stuff",
                Link = new Uri(_url + "/feed"), // There is your
                Copyright = "©" + " " + myYear + " " + " " + _name,
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
            var entries = await _repo.GetStoriesByTag("en-US", 25, 1); // Just returns stories with "en-US".

            foreach (var entry in entries.Stories)
            {
                var item = new Item()
                {
                    Title = entry.Title,
                    Body = string.Concat(entry.Body, license, ad), // Removed ad
                    Link = new Uri(new Uri(Request.GetEncodedUrl()), entry.Slug),
                    Permalink = new Uri(new Uri(Request.GetEncodedUrl()), entry.Slug).ToString(),
                    PublishDate = entry.DatePublished,
                    Author = new Author() { Name = _name, Email = _email },
                };

                foreach (var cat in entry.Categories.Split(','))
                {
                    item.Categories.Add(cat);
                }

                feed.Items.Add(item);
            }

            return File(Encoding.UTF8.GetBytes(feed.Serialize()), "text/xml");
        }

        /// <summary>
        /// Feed with all german articles in all category.
        /// </summary>
        /// <returns>RSS-Feed.</returns>
        [HttpGet("feed-de")]
        public async Task<IActionResult> FeedDe()
        {
            var feed = new Feed()
            {
                Title = "Sascha Manns's Twilight Zone - German Feed",
                Description = "Blog about Linux, Windows (WSL, Insider), Programming (Ruby, Python, Java, Android ASP and Mono/.NET) and other random stuff",
                Link = new Uri(_url + "/feed-de"),
                Copyright = "©" + " " + myYear + " " + " " + _name,
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
                    Author = new Author() { Name = _name, Email = _email },
                };

                foreach (var cat in entry.Categories.Split(','))
                {
                    item.Categories.Add(cat);
                }

                feed.Items.Add(item);
            }

            return File(Encoding.UTF8.GetBytes(feed.Serialize()), "text/xml");
        }

        /// <summary>
        /// My german Opensource Feed.
        /// </summary>
        /// <returns>RSS-Feed.</returns>
        [HttpGet("opensource-de")]
        public async Task<IActionResult> OpensourceDe()
        {
            var feed = new Feed()
            {
                Title = "Sascha Manns's Twilight Zone - German Opensource Feed",
                Description = "Blog about Linux, Windows (WSL, Insider), Programming (Ruby, Python, Java, Android ASP and Mono/.NET) and other random stuff",
                Link = new Uri(_url + "/opensource-de"),
                Copyright = "©" + " " + myYear + " " + " " + _name,
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
                    Author = new Author() { Name = _name, Email = _email },
                };

                foreach (var cat in entry.Categories.Split(','))
                {
                    item.Categories.Add(cat);
                }

                feed.Items.Add(item);
            }

            return File(Encoding.UTF8.GetBytes(feed.Serialize()), "text/xml");
        }

        /// <summary>
        /// My english Opensource feed.
        /// </summary>
        /// <returns>RSS-Feed.</returns>
        [HttpGet("opensource")]
        public async Task<IActionResult> Opensource()
        {
            var feed = new Feed()
            {
                Title = "Sascha Manns's Twilight Zone - English Opensource Feed",
                Description = "Blog about Linux, Windows (WSL, Insider), Programming (Ruby, Python, Java, Android ASP and Mono/.NET) and other random stuff",
                Link = new Uri(_url + "/opensource"),
                Copyright = "©" + " " + myYear + " " + " " + _name,
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
                    Author = new Author() { Name = _name, Email = _email },
                };

                foreach (var cat in entry.Categories.Split(','))
                {
                    item.Categories.Add(cat);
                }

                feed.Items.Add(item);
            }

            return File(Encoding.UTF8.GetBytes(feed.Serialize()), "text/xml");
        }

        /// <summary>
        /// My engish RSS-Feed.
        /// </summary>
        /// <returns>RSS-Feee.</returns>
        [HttpGet("linux")]
        public async Task<ActionResult> Linux()
        {
            var feed = new Feed()
            {
                Title = "Sascha Manns's Twilight Zone - Linux Feed (German and English)",
                Description = "Blog about Linux, Windows (WSL, Insider), Programming (Ruby, Python, Java, Android ASP and Mono/.NET) and other random stuff",
                Link = new Uri(_url + "/linux"),
                Copyright = "©" + " " + myYear + " " + " " + _name,
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
                    Author = new Author() { Name = _name, Email = _email },
                };

                foreach (var cat in entry.Categories.Split(','))
                {
                    item.Categories.Add(cat);
                }

                feed.Items.Add(item);
            }

            return File(Encoding.UTF8.GetBytes(feed.Serialize()), "text/xml");
        }

        /// <summary>
        /// My english Windows feed.
        /// </summary>
        /// <returns>RSS-Feed.</returns>
        [HttpGet("windows")]
        public async Task<IActionResult> Windows()
        {
            var feed = new Feed()
            {
                Title = "Sascha Manns's Twilight Zone - Windows Feed (German and English)",
                Description = "Blog about Linux, Windows (WSL, Insider), Programming (Ruby, Python, Java, Android ASP and Mono/.NET) and other random stuff",
                Link = new Uri(_url + "/windows"),
                Copyright = "©" + " " + myYear + " " + " " + _name,
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
                    Author = new Author() { Name = _name, Email = _email },
                };

                foreach (var cat in entry.Categories.Split(','))
                {
                    item.Categories.Add(cat);
                }

                feed.Items.Add(item);
            }

            return File(Encoding.UTF8.GetBytes(feed.Serialize()), "text/xml");
        }

        /// <summary>
        /// My DotNetCore Feed.
        /// </summary>
        /// <returns>RSS-Feed.</returns>
        [HttpGet("dotnetcore")]
        public async Task<IActionResult> DotNetCore()
        {
            var feed = new Feed()
            {
                Title = "Sascha Manns's Twilight Zone - Dotnetcore Feed (German and English)",
                Description = "Blog about Linux, Windows (WSL, Insider), Programming (Ruby, Python, Java, Android ASP and Mono/.NET) and other random stuff",
                Link = new Uri(_url + "/dotnetcore"),
                Copyright = "©" + " " + myYear + " " + " " + _name,
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
                    Author = new Author() { Name = _name, Email = _email },
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
