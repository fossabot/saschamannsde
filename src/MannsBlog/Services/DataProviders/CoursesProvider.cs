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
