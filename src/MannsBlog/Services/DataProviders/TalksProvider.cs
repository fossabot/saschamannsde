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
    public class TalksProvider : DataProvider<Talk>
    {
        public TalksProvider(IHostEnvironment env) : base(env, "talks.json")
        {

        }

        public override IEnumerable<Talk> Get()
        {
            string culture = CultureInfo.CurrentCulture.Name;
            return base.Get().Where(t => t.Language == culture).OrderByDescending(p => p.Date).ToList();
        }
    }

    public enum TalkType
    {
        Unknown = 0,
        Slideshare
    }

    public class Talk
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Where { get; set; }
        public string? Link { get; set; }
        public string? Blurp { get; set; }
        public DateTime Date { get; set; }
        public TalkType TalkType { get; set; }
        public string? Language { get; set; }
    }
}
