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
using System;
using System.Threading.Tasks;
using MannsBlog.Models;
using MannsBlog.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MannsBlog.Controllers.Web
{
    /// <summary>
    /// Controller for searching in the blog.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Route("[controller]")]
    public class SearchController : Controller
    {
        private readonly IMannsRepository _repo;
        private readonly ILogger<SearchController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchController"/> class.
        /// </summary>
        /// <param name="repo">The repo.</param>
        /// <param name="logger">The logger.</param>
        public SearchController(IMannsRepository repo, ILogger<SearchController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>Search-View.</returns>
        [HttpGet("")]
        public IActionResult Index()
        {
            ViewBag.Term = "";
            return View(new BlogResult());
        }

        /// <summary>
        /// Pagers the specified term.
        /// </summary>
        /// <param name="term">The term.</param>
        /// <param name="page">The page.</param>
        /// <returns>Searchresults.</returns>
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
    }
}
