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
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Hosting;

namespace MannsBlog.Services.DataProviders
{
    /// <summary>
    /// Provider for Testimonials.
    /// </summary>
    /// <seealso cref="MannsBlog.Services.DataProviders.DataProvider&lt;saschamannsde.Services.DataProviders.Testimonial&gt;" />
    public class TestimonialsProvider : DataProvider<Testimonial>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestimonialsProvider"/> class.
        /// </summary>
        /// <param name="env">The env.</param>
        public TestimonialsProvider(IHostEnvironment env)
            : base(env, "testimonials.json")
        {
        }

        /// <summary>
        /// Gets the talks.
        /// </summary>
        /// <returns>Talks List.</returns>
        public override IEnumerable<Testimonial> Get()
        {
            string culture = CultureInfo.CurrentCulture.Name;
            return base.Get().Where(t => t.Language == culture).OrderByDescending(p => p.Date).ToList();
        }
    }

    /// <summary>
    /// Testimonial Model.
    /// </summary>
#pragma warning disable SA1402 // File may only contain a single type
    public class Testimonial
#pragma warning restore SA1402 // File may only contain a single type
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>
        /// The link.
        /// </value>
        public string? Link { get; set; }

        /// <summary>
        /// Gets or sets the blurp.
        /// </summary>
        /// <value>
        /// The blurp.
        /// </value>
        public string? Blurp { get; set; }

        /// <summary>
        /// Gets or sets the recommender.
        /// </summary>
        /// <value>
        /// The recommender.
        /// </value>
        public string? Recommender { get; set; }

        /// <summary>
        /// Gets or sets the recommender job.
        /// </summary>
        /// <value>
        /// The recommender job.
        /// </value>
        public string? RecommenderJob { get; set; }

        /// <summary>
        /// Gets or sets the recommender location.
        /// </summary>
        /// <value>
        /// The recommender location.
        /// </value>
        public string? RecommenderLocation { get; set; }

        /// <summary>
        /// Gets or sets the relationship.
        /// </summary>
        /// <value>
        /// The relationship.
        /// </value>
        public string? Relationship { get; set; }

        /// <summary>
        /// Gets or sets the image path.
        /// </summary>
        /// <value>
        /// The image path.
        /// </value>
        public string? ImagePath { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public string? Date { get; set; }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        public string? Language { get; set; }
    }
}
