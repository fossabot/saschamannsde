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