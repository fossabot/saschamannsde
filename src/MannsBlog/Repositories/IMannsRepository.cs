// <copyright file="IMannsRepository.cs" company="Sascha Manns">
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
