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
