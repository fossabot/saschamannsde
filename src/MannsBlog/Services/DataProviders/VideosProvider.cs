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
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MannsBlog.Services.DataProviders
{
    public class VideosProvider : DataProvider<Video>
    {
        public VideosProvider(IHostEnvironment env) : base(env, "videos.json")
        {

        }

        public override IEnumerable<Video> Get()
        {
            string culture = CultureInfo.CurrentCulture.Name;
            return base.Get().Where(t => t.Language == culture).OrderByDescending(p => p.DatePublished).ToList();
        }
    }

    public enum VideoType
    {
        Unknown = 0,
        YouTube,
        Channel9,
        Vimeo
    }

    public class Video
    {
        public string? Title { get; set; }
        public int Id { get; set; }
        public string? Description { get; set; }
        public string? VideoCode { get; set; }
        public VideoType VideoType { get; set; }
        public DateTime DatePublished { get; set; }
        public string? Language { get; set; }
    }
}
