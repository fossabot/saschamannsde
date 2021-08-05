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
