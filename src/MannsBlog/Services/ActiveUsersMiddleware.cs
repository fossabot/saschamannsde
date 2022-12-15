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
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace MannsBlog.Services
{
    /// <summary>
    /// Middleware for Active Users.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Http.IMiddleware" />
    public class ActiveUsersMiddleware : IMiddleware
    {
        private const string Cookiename = ".Vanity.MannsBlog";
        private const string Prefix = "ActiveUser_";
        private const int Timeoutminutes = 5;
        private readonly IMemoryCache _cache;
        private readonly ILogger<ActiveUsersMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveUsersMiddleware"/> class.
        /// </summary>
        /// <param name="cache">The IMemory cache.</param>
        /// <param name="logger">The logger.</param>
        public ActiveUsersMiddleware(IMemoryCache cache, ILogger<ActiveUsersMiddleware> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        /// <summary>
        /// Gets the active user count.
        /// </summary>
        /// <param name="cache">The cache.</param>
        /// <returns>Counted active Users.</returns>
        public static long GetActiveUserCount(IMemoryCache cache)
        {
            var cacheType = cache.GetType();
            var fieldInfo = cacheType.GetField("_entries", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo is null)
            {
                return 0;
            }

            var dict = fieldInfo.GetValue(cache);

            if (dict is null)
            {
                return 0;
            }

            var keys = ((IDictionary)dict).Keys
                .Cast<object>()
                .Where(k => k is string && ((string)k).StartsWith(Prefix))
                .Cast<string>()
                .ToList();

            return keys.Count(k =>
            {
                DateTime expiration;
                if (!cache.TryGetValue<DateTime>(k, out expiration))
                {
                    return false;
                }

                if (expiration > DateTime.UtcNow)
                {
                    return true;
                }

                return false;
            });
        }

        /// <summary>
        /// Request handling method.
        /// </summary>
        /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Http.HttpContext" /> for the current request.</param>
        /// <param name="next">The delegate representing the remaining middleware in the request pipeline.</param>
        /// <returns>The Cookie who should appended.</returns>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                if (!context.Request.Path.StartsWithSegments("/api") && !context.Request.Path.StartsWithSegments("/livewriter"))
                {
                    string cookie;
                    cookie = context.Request.Cookies.ContainsKey(Cookiename) ? context.Request.Cookies[Cookiename]! : Guid.NewGuid().ToString();

                    var key = $"{Prefix}{cookie}";
                    var expiration = DateTime.UtcNow.AddMinutes(Timeoutminutes);
                    _cache.Remove(key);
                    _cache.Set<object>(key, expiration, expiration);
                    context.Response.Cookies.Append(Cookiename, cookie, new CookieOptions() { Expires = DateTimeOffset.Now.AddMinutes(Timeoutminutes) });
                }
            }
            catch
            {
                _logger.LogError("Failed to store active user");
            }

            await next.Invoke(context);
        }
    }
}
