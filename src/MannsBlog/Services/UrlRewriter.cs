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
