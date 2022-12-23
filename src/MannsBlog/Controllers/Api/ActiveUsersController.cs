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
using MannsBlog.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace MannsBlog.Controllers.Api
{
    /// <summary>
    /// Controller for all active users.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    public class ActiveUsersController : Controller
    {
        private IMemoryCache _cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveUsersController"/> class.
        /// </summary>
        /// <param name="cache">The cache.</param>
        public ActiveUsersController(IMemoryCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns>OK Result.</returns>
        [HttpGet("/api/active/users")]
        public IActionResult Get()
        {
            try
            {
                var users = ActiveUsersMiddleware.GetActiveUserCount(_cache);
                return Ok(new { ActiveUsers = users, Message = $"{users} active on the site" });
            }
            catch (Exception ex)
            {
                return Ok(new { ActiveUsers = 0, Message = $"Exception Thrown during process: {ex}" });
            }
        }
    }
}
