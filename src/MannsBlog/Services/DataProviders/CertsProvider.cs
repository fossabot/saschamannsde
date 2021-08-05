using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MannsBlog.Services.DataProviders
{
    public class CertsProvider : DataProvider<Cert>
    {
        public CertsProvider(IHostEnvironment env) : base(env, "certificates.json")
        {
        }

        public override IEnumerable<Cert> Get()
        {
            string culture = CultureInfo.CurrentCulture.Name;
            return base.Get().Where(t => t.Language == culture).OrderByDescending(p => p.Years).ToList();
        }
    }

    public class Cert
    {
        public int Id { get; set; }
        public string? Certtitle { get; set; }
        public string? Provider { get; set; }
        public string? Years { get; set; }
        public string? Content { get; set; }
        public string? Link { get; set; }
        public string? Language { get; set; }
    }
}