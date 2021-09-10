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
using System;
using System.Collections.Generic;

namespace MannsBlog.Data
{
  public class BlogStory
  {
    public int Id { get; set; }
    public string Body { get; set; } = "";
    public string Categories { get; set; } = "";
    public DateTime DatePublished { get; set; }
    public bool IsPublished { get; set; }
    public string Slug { get; set; } = "";
    public string Title { get; set; } = "";
    public string? UniqueId { get; set; } = "";
    public string? FeatureImageUrl { get; set; } = "";
    public string? Abstract { get; set; } = "";
  }
}