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
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;

namespace MannsBlog.Data
{
  public class MannsContext : IdentityDbContext<MannsUser>
  {
    public MannsContext(DbContextOptions<MannsContext> options, IConfiguration config) : base(options)
    {
      _config = config;
    }

    private readonly IConfiguration _config;

    public DbSet<BlogStory> Stories => Set<BlogStory>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
      // Create Defaults
      base.OnModelCreating(builder);

      MapEntity(builder.Entity<BlogStory>());
    }

    private void MapEntity(EntityTypeBuilder<BlogStory> bldr)
    {
      // Override the name of the table because of a RC2 change
      bldr.ToTable("BlogStory");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlServer(_config["MannsDb:ConnectionString"]);
      base.OnConfiguring(optionsBuilder);
    }

  }
}