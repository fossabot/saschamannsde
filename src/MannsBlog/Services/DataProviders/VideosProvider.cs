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
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Hosting;

namespace MannsBlog.Services.DataProviders
{
    /// <summary>
    /// Enumeration for Videos.
    /// </summary>
    public enum VideoType
    {
        /// <summary>
        /// The unknown
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// You tube
        /// </summary>
        YouTube,

        /// <summary>
        /// The channel9
        /// </summary>
        Channel9,

        /// <summary>
        /// The vimeo
        /// </summary>
        Vimeo,
    }

    /// <summary>
    /// Provider for Videos.
    /// </summary>
    /// <seealso cref="MannsBlog.Services.DataProviders.DataProvider&lt;saschamannsde.Services.DataProviders.Video&gt;" />
    public class VideosProvider : DataProvider<Video>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideosProvider"/> class.
        /// </summary>
        /// <param name="env">The env.</param>
        public VideosProvider(IHostEnvironment env)
            : base(env, "videos.json")
        {
        }

        /// <summary>
        /// Gets the videos..
        /// </summary>
        /// <returns>Video List.</returns>
        public override IEnumerable<Video> Get()
        {
            string culture = CultureInfo.CurrentCulture.Name;
            return base.Get().Where(t => t.Language == culture).OrderByDescending(p => p.DatePublished).ToList();
        }
    }

#pragma warning disable SA1402 // File may only contain a single type
    /// <summary>
    /// Video Model.
    /// </summary>
    public class Video
#pragma warning restore SA1402 // File may only contain a single type
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the video code.
        /// </summary>
        /// <value>
        /// The video code.
        /// </value>
        public string? VideoCode { get; set; }

        /// <summary>
        /// Gets or sets the type of the video.
        /// </summary>
        /// <value>
        /// The type of the video.
        /// </value>
        public VideoType VideoType { get; set; }

        /// <summary>
        /// Gets or sets the date published.
        /// </summary>
        /// <value>
        /// The date published.
        /// </value>
        public DateTime DatePublished { get; set; }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        public string? Language { get; set; }
    }
}
