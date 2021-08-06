using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using MannsBlog.Services.DataProviders;

namespace MannsBlog.Controllers.Web
{
  [Route("[controller]")]
  public class VideosController : Controller
  {
    private VideosProvider _videos;

    public VideosController(VideosProvider videos)
    {
      _videos = videos;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
      return View(_videos.Get());
    }

    [HttpGet("{id:int}")]
    public IActionResult Video(int id)
    {
      var result = _videos.Get().Where(v => v.Id == id).FirstOrDefault();
      if (result == null) return RedirectToAction("Index");
      return View(result);
    }

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
    }
}
