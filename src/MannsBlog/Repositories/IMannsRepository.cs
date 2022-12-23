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
using System.Threading.Tasks;
using MannsBlog.EntityFramework.Entities;
using MannsBlog.Models;

namespace MannsBlog.Repositories
{
    /// <summary>
    /// Interface for the Repository.
    /// </summary>
    public interface IMannsRepository
    {
        /// <summary>
        /// Gets the stories.
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="page">The page.</param>
        /// <returns>Stories as BlogResult.</returns>
        Task<BlogResult> GetStories(int pageSize = 10, int page = 1);

        /// <summary>
        /// Gets the stories by term.
        /// </summary>
        /// <param name="term">The term.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="page">The page.</param>
        /// <returns>Stories by a given term as BlogResult.</returns>
        Task<BlogResult> GetStoriesByTerm(string term, int pageSize, int page);

        /// <summary>
        /// Gets the stories by tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="page">The page.</param>
        /// <returns>Stories by a given Tag.</returns>
        Task<BlogResult> GetStoriesByTag(string tag, int pageSize, int page);

        /// <summary>
        /// Gets the story by id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A Story by its id.</returns>
        Task<BlogStory> GetStory(int id);

        /// <summary>
        /// Gets the story by its slug.
        /// </summary>
        /// <param name="slug">The slug.</param>
        /// <returns>Stories by a given slug.</returns>
        Task<BlogStory> GetStory(string slug);

        /// <summary>
        /// Gets the categories.
        /// </summary>
        /// <returns>Categories.</returns>
        Task<IEnumerable<string>> GetCategories();

        /// <summary>
        /// Adds the story.
        /// </summary>
        /// <param name="story">The story.</param>
        void AddStory(BlogStory story);

        /// <summary>
        /// Deletes the story.
        /// </summary>
        /// <param name="postid">The postid.</param>
        /// <returns>Bool what reflects the success of deletion.</returns>
        Task<bool> DeleteStory(string postid);

        /// <summary>
        /// Saves all asynchronous.
        /// </summary>
        /// <returns>Result as bool.</returns>
        Task<bool> SaveAllAsync();
    }
}
