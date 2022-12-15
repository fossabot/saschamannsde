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

namespace MannsBlog.Controllers.Web
{
    /// <summary>
    /// Controller for the held talks.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Route("[controller]")]
    public class TalksController : Controller
    {
        private TalksProvider _talks;

        /// <summary>
        /// Initializes a new instance of the <see cref="TalksController"/> class.
        /// </summary>
        /// <param name="talks">The talks.</param>
        public TalksController(TalksProvider talks)
        {
            _talks = talks;
        }

        /// <summary>
        /// Talks view.
        /// </summary>
        /// <returns>Talks View.</returns>
        [HttpGet("")]
        public IActionResult Index()
        {
            return View(_talks.Get());
        }
    }
}