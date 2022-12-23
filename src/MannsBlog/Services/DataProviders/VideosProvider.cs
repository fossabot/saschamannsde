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
