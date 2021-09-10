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
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

namespace MannsBlog.Services.DataProviders
{
    public class TestimonialsProvider : DataProvider<Testimonial>
    {
        public TestimonialsProvider(IHostEnvironment env) : base(env, "testimonials.json")
        {

        }

        public override IEnumerable<Testimonial> Get()
        {
            string culture = CultureInfo.CurrentCulture.Name;
            return  base.Get().Where(t => t.Language == culture).OrderByDescending(p => p.Date).ToList();
        }
    }

    public class Testimonial
    {
        public int Id { get; set; }
        public string? Link { get; set; }
        public string? Blurp { get; set; }
        public string? Recommender { get; set; }
        public string? RecommenderJob { get; set; }
        public string? RecommenderLocation { get; set; }
        public string? Relationship { get; set; }
        public string? ImagePath { get; set; }
        public string? Date { get; set; }
        public string? Language { get; set; }
    }
}
