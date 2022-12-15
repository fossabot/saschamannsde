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
    /// Provider for jobs.
    /// </summary>
    /// <seealso cref="MannsBlog.Services.DataProviders.DataProvider&lt;Job&gt;" />
    public class JobsProvider : DataProvider<Job>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JobsProvider"/> class.
        /// </summary>
        /// <param name="env">The env.</param>
        public JobsProvider(IHostEnvironment env)
            : base(env, "jobs.json")
        {

        }

        /// <summary>
        /// Gets the jobs.
        /// </summary>
        /// <returns>Jobas for the current culture.</returns>
        public override IEnumerable<Job> Get()
        {
            string culture = CultureInfo.CurrentCulture.Name;
            return base.Get().Where(t => t.Language == culture).OrderByDescending(p => p.Id).ToList();
        }
    }

    /// <summary>
    /// Class for jobs.
    /// </summary>
    public class Job
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the jobtitle.
        /// </summary>
        /// <value>
        /// The jobtitle.
        /// </value>
        public string? Jobtitle { get; set; }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>
        /// The company.
        /// </value>
        public string? Company { get; set; }

        /// <summary>
        /// Gets or sets the years.
        /// </summary>
        /// <value>
        /// The years.
        /// </value>
        public string? Years { get; set; }

        /// <summary>
        /// Gets or sets the tasks.
        /// </summary>
        /// <value>
        /// The tasks.
        /// </value>
        public string? Tasks { get; set; }

        /// <summary>
        /// Gets or sets the stack.
        /// </summary>
        /// <value>
        /// The stack.
        /// </value>
        public string? Stack { get; set; }

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>
        /// The link.
        /// </value>
        public string? Link { get; set; }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        public string? Language { get; set; }
    }
}
