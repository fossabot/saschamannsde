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
using System.Linq;

namespace MannsBlog.Services.DataProviders
{
    public class PublicationsProvider : DataProvider<Publication>
    {
        public PublicationsProvider(IHostEnvironment env)
          : base(env, "publications.json")
        {
        }

        public override IEnumerable<Publication> Get()
        {
            return base.Get().OrderByDescending(p => p.DatePublished).ToList();
        }
    }

    public class Publication
    {
        public int Id { get; set; }
        public string? PublicationName { get; set; }
        public string? Publisher { get; set; }
        public DateTime DatePublished { get; set; }
        public string? Comments { get; set; }
        public bool IsBook { get; set; }
        public string? Title { get; set; }
        public string? Link { get; set; }
        public string? Identifier { get; set; }
    }
}
