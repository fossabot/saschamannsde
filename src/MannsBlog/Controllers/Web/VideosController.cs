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
using System.Linq;
using MannsBlog.Services.DataProviders;
using Microsoft.AspNetCore.Mvc;

namespace MannsBlog.Controllers.Web
{
    /// <summary>
    /// Controller for videos.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Route("[controller]")]
    public class VideosController : Controller
    {
        private VideosProvider _videos;

        /// <summary>
        /// Initializes a new instance of the <see cref="VideosController"/> class.
        /// </summary>
        /// <param name="videos">The videos.</param>
        public VideosController(VideosProvider videos)
        {
            _videos = videos;
        }

        /// <summary>
        /// Videos View.
        /// </summary>
        /// <returns>View.</returns>
        [HttpGet("")]
        public IActionResult Index()
        {
            return View(_videos.Get());
        }

        /// <summary>
        /// Prepares the Video view.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>View.</returns>
        [HttpGet("{id:int}")]
        public IActionResult Video(int id)
        {
            var result = _videos.Get().Where(v => v.Id == id).FirstOrDefault();
            if (result == null)
            {
                return RedirectToAction("Index");
            }

            return View(result);
        }
    }
}
