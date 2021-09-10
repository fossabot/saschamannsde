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
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MannsBlog.Services.DataProviders
{
  public class CoursesProvider : DataProvider<Course>
  {
  
    public CoursesProvider(IHostEnvironment env)
      : base(env, "courses.json")
    {
    }
  }

  public enum CourseType
  {
    Invalid = 0,
    Pluralsight = 1,
    MannsMinds = 2
  }

  public class Course
  {
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Hint { get; set; } = "";
    public CourseType CourseType { get; set; } = CourseType.Pluralsight;
  }
}
