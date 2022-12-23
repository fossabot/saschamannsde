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
