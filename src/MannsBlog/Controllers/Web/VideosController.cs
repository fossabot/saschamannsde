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
using MannsBlog.Services.DataProviders;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
    }
}
