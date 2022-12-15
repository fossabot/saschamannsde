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

namespace MannsBlog.Services.DataProviders
{
    /// <summary>
    /// Enumeration of Courses.
    /// </summary>
    public enum CourseType
    {
        /// <summary>
        /// The invalid
        /// </summary>
        Invalid = 0,

        /// <summary>
        /// The pluralsight
        /// </summary>
        Pluralsight = 1,

        /// <summary>
        /// The linkedin
        /// </summary>
        Linkedin = 2,
    }

    /// <summary>
    /// Provider for Courses.
    /// </summary>
    /// <seealso cref="MannsBlog.Services.DataProviders.DataProvider&lt;saschamannsde.Services.DataProviders.Course&gt;" />
    public class CoursesProvider : DataProvider<Course>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CoursesProvider"/> class.
        /// </summary>
        /// <param name="env">The env.</param>
        public CoursesProvider(IHostEnvironment env)
          : base(env, "courses.json")
        {
        }
    }

    /// <summary>
    /// Course Model.
    /// </summary>
    public class Course
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the hint.
        /// </summary>
        /// <value>
        /// The hint.
        /// </value>
        public string Hint { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of the course.
        /// </summary>
        /// <value>
        /// The type of the course.
        /// </value>
        public CourseType CourseType { get; set; } = CourseType.Linkedin;
    }
}
