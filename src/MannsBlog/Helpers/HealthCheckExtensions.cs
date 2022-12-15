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
using MannsBlog.EntityFramework.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MannsBlog.Helpers
{
    /// <summary>
    /// Extension for using HealthChecks.
    /// </summary>
    public static class HealthCheckExtensions
    {
        /// <summary>
        /// Configures the health checks.
        /// </summary>
        /// <param name="coll">The Servicecollection.</param>
        /// <param name="config">The configuration.</param>
        /// <returns>Service Collection.</returns>
        public static IServiceCollection ConfigureHealthChecks(this IServiceCollection coll, IConfiguration config)
        {
            var connectionString = config["MannsDb:ConnectionString"];
            var instrumentationKey = config["ApplicationInsights:ConnectionString"];

            coll.AddHealthChecks()
              .AddSqlServer(connectionString, name: "DbConnection")
              .AddSqlServer(connectionString,
                 "SELECT COUNT(*) FROM BlogStory",
                 "BlogDb")
              .AddDbContextCheck<MannsContext>()
              .AddApplicationInsightsPublisher(instrumentationKey: instrumentationKey); ;

            return coll;
        }
    }
}
