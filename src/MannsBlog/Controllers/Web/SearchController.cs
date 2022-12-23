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
