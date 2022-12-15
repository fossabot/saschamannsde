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