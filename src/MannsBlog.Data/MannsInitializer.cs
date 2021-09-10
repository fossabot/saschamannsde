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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MannsBlog.Data
{
  public class MannsInitializer
  {
    private readonly MannsContext _ctx;
    private readonly UserManager<MannsUser> _userMgr;
    private readonly ILogger<MannsInitializer> _logger;

    public MannsInitializer(MannsContext ctx, UserManager<MannsUser> userMgr, ILogger<MannsInitializer> logger)
    {
      _ctx = ctx;
      _userMgr = userMgr;
      _logger = logger;
    }

    public async Task SeedAsync()
    {
      // Seed User
      if (await _userMgr.FindByNameAsync("saschamanns") == null)
      {
        var user = new MannsUser()
        {
          Email = "Sascha.Manns@outlook.de",
          UserName = "saschamanns",
          EmailConfirmed = true
        };

        var result = await _userMgr.CreateAsync(user, "@Password"); // Temp Password
        if (!result.Succeeded) throw new InvalidProgramException("Failed to create seed user");
      }

      // Seed Stories
      if (!_ctx.Stories.Any())
      {
        var stories = MemoryRepository.Stories;
        stories.ForEach(s => s.Id = 0);
        _ctx.Stories.AddRange(stories);
        await _ctx.SaveChangesAsync();
      }

      // Update Abstract and Image
      if (!_ctx.Stories.Any(s => !string.IsNullOrEmpty(s.Abstract)))
      {
        var stories = await _ctx.Stories.ToListAsync();
        foreach (var story in stories)
        {
          // Get Image
          var summary = GetSummary(story);
          var regex = new Regex("\\<img.+src\\=(?:\"|\')(.+?)(?:\"|\')(?:.+?)\\>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
          var result = regex.Matches(summary);
          if (result.Any())
          {
            var firstResult = result.First();
            var rawImg = firstResult.Groups[0].Value;
            var url = firstResult.Groups[1].Value;
            story.FeatureImageUrl = url;
            story.Body = story.Body.Replace(rawImg, "");
            story.Abstract = summary.Replace(rawImg, "");
            _logger.LogDebug($"Updating: {story.Title}");
          }
        }
        if (!(await _ctx.SaveChangesAsync() > 0))
        {
          _logger.LogError("Failed to save changes to image and abstracts");
        }
        _logger.LogDebug($"Done Updating");
      }
    }

    private string GetSummary(BlogStory story)
    {
      var maxparagraphs = 3;
      var regex = new Regex("(<p[^>]*>.*?</p>)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
      var result = regex.Matches(story.Body);
      StringBuilder bldr = new StringBuilder();
      var x = 0;
      foreach (Match m in result)
      {
        x++;
        bldr.Append(m.Value);
        if (x == maxparagraphs) break;
      }
      return bldr.ToString();
    }
  }
}
