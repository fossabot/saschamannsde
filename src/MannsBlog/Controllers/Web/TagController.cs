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