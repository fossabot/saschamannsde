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
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MannsBlog.EntityFramework.Context;
using MannsBlog.EntityFramework.Entities;
using MannsBlog.Models;
using MannsBlog.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MannsBlog
{
    /// <summary>
    /// Class for seeding some data.
    /// </summary>
    public class MannsInitializer
    {
        private readonly MannsContext? _ctx;
        private readonly UserManager<MannsUser>? _userMgr;
        private readonly ILogger<MannsInitializer>? _logger;
        private readonly IConfiguration? _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="MannsInitializer"/> class.
        /// </summary>
        /// <param name="ctx">The CTX.</param>
        /// <param name="userMgr">The user MGR.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="config">The configuration.</param>
        public MannsInitializer(MannsContext ctx, UserManager<MannsUser> userMgr, ILogger<MannsInitializer> logger, IConfiguration config)
        {
            this._ctx = ctx;
            this._userMgr = userMgr;
            this._logger = logger;
            this._config = config;
        }

        /// <summary>
        /// Users the asynchronous.
        /// </summary>
        /// <exception cref="System.InvalidProgramException">Failed to create seed user.</exception>
        /// <returns>Succeeded or Exception.</returns>
        public async Task SeedUserAsync()
        {
            // Seed User
            string email = this._config.GetValue<string>("Blog:Email");
            string username = this._config.GetValue<string>("Blog:Username");
            string password = this._config.GetValue<string>("Blog:Password");

            if (await this._userMgr.FindByNameAsync(username) == null)
            {
                var user = new MannsUser()
                {
                    Email = email,
                    UserName = username,
                    EmailConfirmed = true,
                };

                var result = await this._userMgr.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    throw new InvalidProgramException("Failed to create seed user");
                }
            }
        }

        /// <summary>
        /// Storieses the asynchronous.
        /// </summary>
        /// <returns>AFAIK Nothing.</returns>
        public async Task SeedStoriesAsync()
        {
            // Seed Stories
            if (!this._ctx.Stories.Any())
            {
                var stories = MemoryRepository.Stories;
                stories.ForEach(s => s.Id = 0);
                this._ctx.Stories.AddRange(stories);
                await this._ctx.SaveChangesAsync();
            }

            // Update Abstract and Image
            //if (!this._ctx.Stories.Any(s => !string.IsNullOrEmpty(s.Abstract)))
            //{
            //    var stories = await this._ctx.Stories.ToListAsync();
            //    foreach (var story in stories)
            //    {
            //        // Get Image
            //        var summary = GetSummary(story);
            //        var regex = new Regex("\\<img.+src\\=(?:\"|\')(.+?)(?:\"|\')(?:.+?)\\>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            //        var result = regex.Matches(summary);
            //        if (result.Any())
            //        {
            //            var firstResult = result.First();
            //            var rawImg = firstResult.Groups[0].Value;
            //            var url = firstResult.Groups[1].Value;
            //            story.FeatureImageUrl = url;
            //            story.Body = story.Body.Replace(rawImg, string.Empty);
            //            story.Abstract = summary.Replace(rawImg, string.Empty);
            //        }
            //    }

            //    if (!(await this._ctx.SaveChangesAsync() > 0))
            //    {
            //        this._logger.LogError("Failed to save changes to image and abstracts");
            //    }

            //    this._logger.LogDebug($"Done Updating");
            //}
        }

        /// <summary>
        /// Gets the summary.
        /// </summary>
        /// <param name="story">The story.</param>
        /// <returns>Summary.</returns>
        private static string GetSummary(BlogStory story)
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
                if (x == maxparagraphs)
                {
                    break;
                }
            }

            return bldr.ToString();
        }
    }
}
