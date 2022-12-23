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
    /// Controller for Tags.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Route("[controller]")]
    public class TagController : Controller
    {
        private readonly IMannsRepository _repo;
        private readonly ILogger<TagController> _logger;
        private readonly int _pageSize = 25;

        /// <summary>
        /// Initializes a new instance of the <see cref="TagController"/> class.
        /// </summary>
        /// <param name="repo">The repo.</param>
        /// <param name="logger">The logger.</param>
        public TagController(IMannsRepository repo, ILogger<TagController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        /// <summary>
        /// Tag search results.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns>Pager.</returns>
        [HttpGet("{tag}")]
        public Task<IActionResult> Index(string tag)
        {
            return Pager(tag, 1);
        }

        /// <summary>
        /// Pagers the specified tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="page">The page.</param>
        /// <returns>Searched Tags.</returns>
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
    }
}