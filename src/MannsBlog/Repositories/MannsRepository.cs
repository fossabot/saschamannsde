// <copyright file="MannsRepository.cs" company="Sascha Manns">
// @author: Sascha Manns, Sascha.Manns@outlook.de
// @copyright: 2022, Sascha Manns, https://saschamanns.de
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
// </copyright>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MannsBlog.EntityFramework.Context;
using MannsBlog.EntityFramework.Entities;
using MannsBlog.Models;
using Microsoft.EntityFrameworkCore;

namespace MannsBlog.Repositories
{
    /// <summary>
    /// Main Repository.
    /// </summary>
    /// <seealso cref="MannsBlog.Data.BaseRepository" />
    /// <seealso cref="MannsBlog.Data.IMannsRepository" />
    public class MannsRepository : BaseRepository, IMannsRepository
    {
        private readonly MannsContext _ctx;

        /// <summary>
        /// Initializes a new instance of the <see cref="MannsRepository"/> class.
        /// </summary>
        /// <param name="ctx">The Context..</param>
        public MannsRepository(MannsContext ctx)
        {
            _ctx = ctx;
        }

        /// <summary>
        /// Adds the story.
        /// </summary>
        /// <param name="story">The story.</param>
        public void AddStory(BlogStory story)
        {
            _ctx.Stories.Add(story);
        }

        /// <summary>
        /// Gets the stories.
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="page">The page.</param>
        /// <returns>
        /// Stories as BlogResult.
        /// </returns>
        public async Task<BlogResult> GetStories(int pageSize = 25, int page = 1)
        {
            var count = _ctx.Stories.Count();

            // fix random SQL Errors due to bad offset
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize > 100)
            {
                pageSize = 100;
            }

            var result = new BlogResult()
            {
                CurrentPage = page,
                TotalResults = count,
                TotalPages = this.CalculatePages(count, pageSize),
                Stories = await _ctx.Stories.Where(s => s.IsPublished)
                .OrderByDescending(s => s.DatePublished)
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .ToListAsync(),
            };

            return result;
        }

        /// <summary>
        /// Gets the stories by term.
        /// </summary>
        /// <param name="term">The term.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="page">The page.</param>
        /// <returns>
        /// Stories by a given term as BlogResult.
        /// </returns>
        public async Task<BlogResult> GetStoriesByTerm(string term, int pageSize, int page)
        {
            var lowerTerm = term.ToLower();
            var totalCount = await _ctx.Stories.Where(s =>
                s.IsPublished &&
                (s.Body.ToLower().Contains(lowerTerm) ||
                s.Categories.ToLower().Contains(lowerTerm) ||
                s.Title.ToLower().Contains(lowerTerm)))
                .CountAsync();

            var result = new BlogResult()
            {
                CurrentPage = page,
                TotalResults = totalCount,
                TotalPages = this.CalculatePages(totalCount, pageSize),
                Stories = await _ctx.Stories
                    .Where(s => s.IsPublished && (s.Body.ToLower().Contains(lowerTerm) ||
                    s.Categories.ToLower().Contains(lowerTerm) ||
                    s.Title.ToLower().Contains(lowerTerm)))
                    .OrderByDescending(o => o.DatePublished)
                    .Skip((page - 1) * pageSize).Take(pageSize).ToArrayAsync(),
            };

            return result;
        }

        /// <summary>
        /// Gets the story by id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// A Story by its id.
        /// </returns>
        public async Task<BlogStory> GetStory(int id)
        {
            var result = await _ctx.Stories.Where(b => b.Id == id).FirstOrDefaultAsync();

            return result;
        }

        /// <summary>
        /// Gets the story by its slug.
        /// </summary>
        /// <param name="slug">The slug.</param>
        /// <returns>
        /// Stories by a given slug.
        /// </returns>
        public async Task<BlogStory> GetStory(string slug)
        {
            var result = await _ctx.Stories
                .Where(s => s.Slug == slug || s.Slug == slug.Replace('_', '-'))
                .FirstOrDefaultAsync();

            return result;
        }

        /// <summary>
        /// Deletes the story.
        /// </summary>
        /// <param name="postid">The postid.</param>
        /// <returns>Result if removing was successful.</returns>
        public async Task<bool> DeleteStory(string postid)
        {
            var id = int.Parse(postid);
            var story = await _ctx.Stories.Where(w => w.Id == id).FirstOrDefaultAsync();
            if (story != null)
            {
                _ctx.Stories.Remove(story);
            }

            return false;
        }

        /// <summary>
        /// Gets the categories.
        /// </summary>
        /// <returns>
        /// Categories.
        /// </returns>
        public async Task<IEnumerable<string>> GetCategories()
        {
            var cats = await _ctx.Stories
                      .Select(c => c.Categories.Split(new[] { ',' }, StringSplitOptions.None))
                      .ToListAsync();

            var result = new List<string>();
            foreach (var s in cats)
            {
                result.AddRange(s);
            }

            return result.Where(s => !string.IsNullOrWhiteSpace(s)).OrderBy(s => s).Distinct();
        }


        /// <summary>
        /// Gets the stories by tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="page">The page.</param>
        /// <returns>
        /// Stories by a given Tag.
        /// </returns>
        public async Task<BlogResult> GetStoriesByTag(string tag, int pageSize, int page)
        {
            var lowerTag = tag.ToLowerInvariant();
            var subResult = await _ctx.Stories
              .Where(s => s.IsPublished && s.Categories.ToLower().Contains(lowerTag)) // Limiting the search for perf
              .ToArrayAsync();
            var totalCount = subResult.Count(s => s.Categories.ToLower().Split(',').Contains(lowerTag));

            var stories = await _ctx.Stories
                .Where(s => s.IsPublished && s.Categories.ToLower().Contains(lowerTag))
                .ToArrayAsync();

            var result = new BlogResult()
            {
                CurrentPage = page,
                TotalResults = totalCount,
                TotalPages = this.CalculatePages(totalCount, pageSize),
                Stories = stories.Where(s => s.Categories.ToLower().Split(',').Contains(lowerTag))
                .OrderByDescending(s => s.DatePublished)
                .Skip((page - 1) * pageSize).Take(pageSize),
            };

            return result;
        }

        /// <summary>
        /// Saves all asynchronous.
        /// </summary>
        /// <returns>
        /// Result as bool.
        /// </returns>
        public async Task<bool> SaveAllAsync()
        {
            return await _ctx.SaveChangesAsync() > 0;
        }
    }
}
