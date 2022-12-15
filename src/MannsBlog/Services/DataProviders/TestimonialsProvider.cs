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
