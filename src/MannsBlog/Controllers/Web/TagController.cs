using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using MannsBlog.Data;

namespace MannsBlog.Controllers.Web
{
  [Route("[controller]")]
  public class TagController : Controller
  {
    private readonly IMannsRepository _repo;
    private readonly ILogger<TagController> _logger;
    readonly int _pageSize = 25;

    public TagController(IMannsRepository repo, ILogger<TagController> logger)
    {
      _repo = repo;
      _logger = logger;
    }

    [HttpGet("{tag}")]
    public Task<IActionResult> Index(string tag)
    {
      return Pager(tag, 1);
    }

    [HttpGet("{tag}/{page}")]
    public async Task<IActionResult> Pager(string tag, int page)
    {
      BlogResult result = new();

      try
      {
        result = await _repo.GetStoriesByTag(tag, _pageSize, page);
      }
      catch (Exception ex)
      {
        _logger.LogError($"Failed to load Tags: {tag} - {ex}");
        
      }

      return View("Index", result);
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