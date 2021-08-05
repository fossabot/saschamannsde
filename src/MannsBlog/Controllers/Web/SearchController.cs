using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MannsBlog.Data;

namespace MannsBlog.Controllers.Web
{
  [Route("[controller]")]
  public class SearchController : Controller
  {
    private readonly IMannsRepository _repo;
    private readonly ILogger<SearchController> _logger;

    public SearchController(IMannsRepository repo, ILogger<SearchController> logger)
    {
      _repo = repo;
      _logger = logger;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
      ViewBag.Term = "";
      return View(new BlogResult());
    }

    [HttpGet("{term}/{page:int?}")]
    public async Task<IActionResult> Pager(string term, int page = 1)
    {
      ViewBag.Term = term;
      var results = new BlogResult();
      try
      {
        results = await _repo.GetStoriesByTerm(term, 15, page);
      }
      catch (Exception ex)
      {
        _logger.LogError($"Failed to get search results: {term} - {ex}");
      }

      return View("Index", results);
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
