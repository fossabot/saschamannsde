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
