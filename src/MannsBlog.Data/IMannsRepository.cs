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
using System.Threading.Tasks;

namespace MannsBlog.Data
{
  public interface IMannsRepository
  {
    // Story
    Task<BlogResult> GetStories(int pageSize = 10, int page = 1);
    Task<BlogResult> GetStoriesByTerm(string term, int pageSize, int page);
    Task<BlogResult> GetStoriesByTag(string tag, int pageSize, int page);

    Task<BlogStory> GetStory(int id);
    Task<BlogStory> GetStory(string slug);

    Task<IEnumerable<string>> GetCategories();

    void AddStory(BlogStory story);
    Task<bool> DeleteStory(string postid);
    Task<bool> SaveAllAsync();

  }
}