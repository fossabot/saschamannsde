using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MannsBlog.Data;

namespace MannsBlog.Services
{
  public class MannsContextFactory : IDesignTimeDbContextFactory<MannsContext>
  {
    public MannsContext CreateDbContext(string[] args)
    {
      // Create a configuration 
      var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("config.json")
        .AddEnvironmentVariables()
        .Build();

      return new MannsContext(new DbContextOptionsBuilder<MannsContext>().Options, config);
    }
  }
}
