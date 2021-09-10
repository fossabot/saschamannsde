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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MannsBlog.Data;

namespace MannsBlog
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var host = WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(ConfigureConfiguration)
            .UseStartup<Startup>()
            .Build();

      if (args.Contains("/seed"))
      {
        Seed(host).Wait();
      }

      host.Run();
    }

    private static async Task Seed(IWebHost host)
    {
      var scopeFactory = host.Services.GetService<IServiceScopeFactory>();
      using var scope = scopeFactory?.CreateScope();
      var initializer = scope?.ServiceProvider.GetService<MannsInitializer>();
      await initializer!.SeedAsync();
    }


    private static void ConfigureConfiguration(WebHostBuilderContext ctx, IConfigurationBuilder builder)
    {
      // Reset to remove the old configuration sources to give us complete control
      builder.Sources.Clear();

      builder.SetBasePath(ctx.HostingEnvironment.ContentRootPath)
        .AddJsonFile("config.json", false, true)
        .AddUserSecrets(Assembly.GetEntryAssembly())
        .AddEnvironmentVariables();

    }
  }
}
