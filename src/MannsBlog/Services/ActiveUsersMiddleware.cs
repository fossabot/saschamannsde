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
