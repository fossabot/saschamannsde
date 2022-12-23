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
