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
    /// Provider for Projects.
    /// </summary>
    /// <seealso cref="MannsBlog.Services.DataProviders.DataProvider&lt;saschamannsde.Services.DataProviders.Project&gt;" />
    public class ProjectsProvider : DataProvider<Project>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectsProvider"/> class.
        /// </summary>
        /// <param name="env">The env.</param>
        public ProjectsProvider(IHostEnvironment env)
            : base(env, "projects.json")
        {

        }

        /// <summary>
        /// Gets the projects for that target..
        /// </summary>
        /// <returns>Projectlist.</returns>
        public override IEnumerable<Project> Get()
        {
            string culture = CultureInfo.CurrentCulture.Name;
            return base.Get().Where(t => t.Language == culture).ToList();
        }
    }

    /// <summary>
    /// Project Model.
    /// </summary>
    public class Project
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the project href.
        /// </summary>
        /// <value>
        /// The project href.
        /// </value>
        public string? ProjectHref { get; set; }

        /// <summary>
        /// Gets or sets the project href target.
        /// </summary>
        /// <value>
        /// The project href target.
        /// </value>
        public string? ProjectHrefTarget { get; set; }

        /// <summary>
        /// Gets or sets the portfolio link.
        /// </summary>
        /// <value>
        /// The portfolio link.
        /// </value>
        public string? PortfolioLink { get; set; }

        /// <summary>
        /// Gets or sets the project link.
        /// </summary>
        /// <value>
        /// The project link.
        /// </value>
        public string? ProjectLink { get; set; }

        /// <summary>
        /// Gets or sets the projectname.
        /// </summary>
        /// <value>
        /// The projectname.
        /// </value>
        public string? Projectname { get; set; }

        /// <summary>
        /// Gets or sets the project blurp.
        /// </summary>
        /// <value>
        /// The project blurp.
        /// </value>
        public string? ProjectBlurp { get; set; }

        /// <summary>
        /// Gets or sets the clients.
        /// </summary>
        /// <value>
        /// The clients.
        /// </value>
        public string? Clients { get; set; }

        /// <summary>
        /// Gets or sets the completion.
        /// </summary>
        /// <value>
        /// The completion.
        /// </value>
        public string? Completion { get; set; }

        /// <summary>
        /// Gets or sets the type of the project.
        /// </summary>
        /// <value>
        /// The type of the project.
        /// </value>
        public string? ProjectType { get; set; }

        /// <summary>
        /// Gets or sets the authors.
        /// </summary>
        /// <value>
        /// The authors.
        /// </value>
        public string? Authors { get; set; }

        /// <summary>
        /// Gets or sets the goal.
        /// </summary>
        /// <value>
        /// The goal.
        /// </value>
        public string? Goal { get; set; }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        public string? Language { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public string? Width { get; set; }
    }
}