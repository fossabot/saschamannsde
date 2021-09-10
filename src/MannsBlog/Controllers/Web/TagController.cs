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