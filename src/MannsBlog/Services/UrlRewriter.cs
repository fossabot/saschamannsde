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
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace MannsBlog.Services
{
  public class UrlRewriter
  {
    private readonly RequestDelegate _next;

    public UrlRewriter(RequestDelegate next)
    {
      _next = next;
    }

    readonly IEnumerable<UrlRule> _rules = new List<UrlRule>()
    {
      new UrlRule() { Search = "/images/", Replace = "/img/blog/" }
    };

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

  public class UrlRule
  {
    public string Replace { get; internal set; } = "";
    public string Search { get; internal set; } = "";
  }

  public static class UrlRewriterExtensions
  {
    public static IApplicationBuilder UseUrlRewriter(this IApplicationBuilder builder)
    {
      return builder.Use(next => new UrlRewriter(next).Invoke);
    }
  }
}
