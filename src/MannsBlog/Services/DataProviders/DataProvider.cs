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
using System.IO;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace MannsBlog.Services.DataProviders
{
    /// <summary>
    /// Data Provider Base class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DataProvider<T>
    {
        private readonly string _path;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataProvider{T}"/> class.
        /// </summary>
        /// <param name="env">The env.</param>
        /// <param name="path">The path.</param>
        protected DataProvider(IHostEnvironment env, string path)
        {
            _path = System.IO.Path.Combine(env.ContentRootPath, $@"Data{System.IO.Path.DirectorySeparatorChar}{path}");
        }

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<T> Get()
        {
            var json = File.ReadAllText(_path);
            return JsonConvert.DeserializeObject<List<T>>(json);
        }

    }
}