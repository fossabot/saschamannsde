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