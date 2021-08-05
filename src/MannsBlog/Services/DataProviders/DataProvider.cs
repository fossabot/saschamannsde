using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace MannsBlog.Services.DataProviders
{
    public abstract class DataProvider<T>
    {
        private readonly string _path;

        protected DataProvider(IHostEnvironment env, string path)
        {
            _path = System.IO.Path.Combine(env.ContentRootPath, $@"Data{System.IO.Path.DirectorySeparatorChar}{path}");
        }

        public virtual IEnumerable<T> Get()
        {
            var json = File.ReadAllText(_path);
            return JsonConvert.DeserializeObject<List<T>>(json);
        }

    }
}