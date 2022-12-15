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
using Microsoft.Extensions.Hosting;

namespace MannsBlog.Services.DataProviders
{
    /// <summary>
    /// Provider for Publications.
    /// </summary>
    /// <seealso cref="MannsBlog.Services.DataProviders.DataProvider&lt;saschamannsde.Services.DataProviders.Publication&gt;" />
    public class PublicationProvider : DataProvider<Publication>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PublicationProvider"/> class.
        /// </summary>
        /// <param name="env">The env.</param>
        public PublicationProvider(IHostEnvironment env)
            : base(env, "publications.json")
        {
        }

        /// <summary>
        /// Gets the publications.
        /// </summary>
        /// <returns>Publication List</returns>
        public override IEnumerable<Publication> Get()
        {
            return base.Get().OrderByDescending(p => p.DatePublished).ToList();
        }
    }

    /// <summary>
    /// Publication Model.
    /// </summary>
    public class Publication
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the publication.
        /// </summary>
        /// <value>
        /// The name of the publication.
        /// </value>
        public string? PublicationName { get; set; }

        /// <summary>
        /// Gets or sets the publisher.
        /// </summary>
        /// <value>
        /// The publisher.
        /// </value>
        public string? Publisher { get; set; }

        /// <summary>
        /// Gets or sets the date published.
        /// </summary>
        /// <value>
        /// The date published.
        /// </value>
        public DateTime DatePublished { get; set; }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>
        /// The comments.
        /// </value>
        public string? Comments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is book.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is book; otherwise, <c>false</c>.
        /// </value>
        public bool IsBook { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>
        /// The link.
        /// </value>
        public string? Link { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string? Identifier { get; set; }
    }
}
