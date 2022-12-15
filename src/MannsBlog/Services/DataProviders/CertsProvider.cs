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
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Hosting;

namespace MannsBlog.Services.DataProviders
{
    /// <summary>
    /// Job Certification Provider.
    /// </summary>
    /// <seealso cref="MannsBlog.Services.DataProviders.DataProvider&lt;saschamannsde.Services.DataProviders.Cert&gt;" />
    public class CertsProvider : DataProvider<Cert>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CertsProvider"/> class.
        /// </summary>
        /// <param name="env">The env.</param>
        public CertsProvider(IHostEnvironment env)
            : base(env, "certificates.json")
        {
        }

        /// <summary>
        /// Gets the certificates.
        /// </summary>
        /// <returns>Certificates List.</returns>
        public override IEnumerable<Cert> Get()
        {
            string culture = CultureInfo.CurrentCulture.Name;
            return base.Get().Where(t => t.Language == culture).OrderByDescending(p => p.Years).ToList();
        }
    }

    /// <summary>
    /// Certification Model.
    /// </summary>
#pragma warning disable SA1402 // File may only contain a single type
    public class Cert
#pragma warning restore SA1402 // File may only contain a single type
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the certtitle.
        /// </summary>
        /// <value>
        /// The certtitle.
        /// </value>
        public string? Certtitle { get; set; }

        /// <summary>
        /// Gets or sets the provider.
        /// </summary>
        /// <value>
        /// The provider.
        /// </value>
        public string? Provider { get; set; }

        /// <summary>
        /// Gets or sets the years.
        /// </summary>
        /// <value>
        /// The years.
        /// </value>
        public string? Years { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public string? Content { get; set; }

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>
        /// The link.
        /// </value>
        public string? Link { get; set; }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        public string? Language { get; set; }
    }
}