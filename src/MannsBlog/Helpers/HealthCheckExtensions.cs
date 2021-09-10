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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MannsBlog.Data;

namespace MannsBlog.Helpers
{
  public static class HealthCheckExtensions
  {
    public static IServiceCollection ConfigureHealthChecks(this IServiceCollection coll, IConfiguration config)
    {
      var connectionString = config["MannsDb:ConnectionString"];
      var instrumentationKey = config["ApplicationInsights:InstrumentationKey"];

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
