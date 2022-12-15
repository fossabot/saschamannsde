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
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace MannsBlog.Services
{
    /// <summary>
    /// Url Rewriting Service.
    /// </summary>
    public class UrlRewriter
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlRewriter"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public UrlRewriter(RequestDelegate next)
        {
            _next = next;
        }

        readonly IEnumerable<UrlRule> _rules = new List<UrlRule>()
    {
      new UrlRule() { Search = "/images/", Replace = "/img/blog/" },
    };
        /// <summary>
        /// Invokes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public async Task Invoke(HttpContext context)
        {
            foreach (var rule in _rules)
            {
                if (context.Request.Path.Value!.Contains(rule.Search))
                {
                    context.Request.Path = context.Request.Path.Value.Replace(rule.Search, rule.Replace);
                }
            }

            // Specialized for hwpod
            var ex = new Regex(@"\/hwpod\/([0-9]*)_([a-zA-Z]*)_([a-zA-Z]*)", RegexOptions.IgnoreCase);
            var match = ex.Match(context.Request.Path);
            if (match.Success)
            {
                context.Response.Redirect($"/hwpod/{match.Groups[1]}/{match.Groups[2]}-{match.Groups[3]}", true);
            }
            else
            {
                await _next(context);
            }
        }

    }

    /// <summary>
    /// Url Rule.
    /// </summary>
    public class UrlRule
    {
        /// <summary>
        /// Gets the replace.
        /// </summary>
        /// <value>
        /// The replace.
        /// </value>
        public string Replace { get; internal set; } = string.Empty;

        /// <summary>
        /// Gets the search.
        /// </summary>
        /// <value>
        /// The search.
        /// </value>
        public string Search { get; internal set; } = string.Empty;
    }

    /// <summary>
    /// Extension for Url Rewriter.
    /// </summary>
    public static class UrlRewriterExtensions
    {
        /// <summary>
        /// Uses the URL rewriter.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseUrlRewriter(this IApplicationBuilder builder)
        {
            return builder.Use(next => new UrlRewriter(next).Invoke);
        }
    }
}
