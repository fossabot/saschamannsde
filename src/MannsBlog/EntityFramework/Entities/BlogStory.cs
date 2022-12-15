// <copyright file="BlogStory.cs" company="Sascha Manns">
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

namespace MannsBlog.EntityFramework.Entities
{
    /// <summary>
    /// Model for the Story itself.
    /// </summary>
    public class BlogStory
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        public string Body { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        /// <value>
        /// The categories.
        /// </value>
        public string Categories { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date published.
        /// </summary>
        /// <value>
        /// The date published.
        /// </value>
        public DateTime DatePublished { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is published.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is published; otherwise, <c>false</c>.
        /// </value>
        public bool IsPublished { get; set; }

        /// <summary>
        /// Gets or sets the slug.
        /// </summary>
        /// <value>
        /// The slug.
        /// </value>
        public string Slug { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        /// <value>
        /// The unique identifier.
        /// </value>
        public string? UniqueId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the feature image URL.
        /// </summary>
        /// <value>
        /// The feature image URL.
        /// </value>
        public string? FeatureImageUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the abstract.
        /// </summary>
        /// <value>
        /// The abstract.
        /// </value>
        public string? Abstract { get; set; } = string.Empty;
    }
}
