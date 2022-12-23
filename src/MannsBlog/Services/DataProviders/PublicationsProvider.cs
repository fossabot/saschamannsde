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
