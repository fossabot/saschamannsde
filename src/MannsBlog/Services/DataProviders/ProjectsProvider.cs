using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MannsBlog.Services.DataProviders
{
    public class ProjectsProvider : DataProvider<Project>
    {
        public ProjectsProvider(IHostEnvironment env) : base(env, "projects.json")
        {

        }

        public override IEnumerable<Project> Get()
        {
            string culture = CultureInfo.CurrentCulture.Name;
            return base.Get().Where(t => t.Language == culture).ToList();
        }
    }

    public class Project
    {
        public int Id { get; set; }
        public string? ProjectHref { get; set; }
        public string? ProjectHrefTarget { get; set; }
        public string? Portfoliolink { get; set; }
        public string? ProjectLink { get; set; }
        public string? Projectname { get; set; }
        public string? ProjectBlurp { get; set; }
        public string? Clients { get; set; }
        public string? Completion { get; set; }
        public string? ProjectType { get; set; }
        public string? Authors { get; set; }
        public string? Goal { get; set; }
        public string? Language { get; set; }
        public string? Width { get; set; }
    }
}