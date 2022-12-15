// <copyright file="MannsContext.cs" company="Sascha Manns">
// @author: Sascha Manns, Sascha.Manns@outlook.de
// @copyright: 2022, Sascha Manns, https://saschamanns.de
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
// </copyright>

using MannsBlog.EntityFramework.Entities;
using MannsBlog.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;

namespace MannsBlog.EntityFramework.Context
{
    /// <summary>
    /// Main Database Context for the Stories.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext&lt;saschamannsde.Data.MannsUser&gt;" />
    public class MannsContext : IdentityDbContext<MannsUser>
    {
        private readonly IConfiguration config;

        /// <summary>
        /// Initializes a new instance of the <see cref="MannsContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="config">The configuration.</param>
        public MannsContext(DbContextOptions<MannsContext> options, IConfiguration config)
            : base(options)
        {
            this.config = config;
        }

        /// <summary>
        /// Gets the stories.
        /// </summary>
        /// <value>
        /// The stories.
        /// </value>
        public DbSet<BlogStory> Stories => this.Set<BlogStory>();

        /// <summary>
        /// Configures the schema needed for the identity framework.
        /// </summary>
        /// <param name="builder">The builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Create Defaults
            base.OnModelCreating(builder);

            this.MapEntity(builder.Entity<BlogStory>());
        }

        /// <summary>
        /// Override this method to configure the database (and other options) to be used for this context.
        /// This method is called for each instance of the context that is created.
        /// The base implementation does nothing.
        /// </summary>
        /// <param name="optionsBuilder">A builder used to create or modify options for this context. Databases (and other extensions)
        /// typically define extension methods on this object that allow you to configure the context.</param>
        /// <remarks>
        /// <para>
        /// In situations where an instance of <see cref="T:Microsoft.EntityFrameworkCore.DbContextOptions" /> may or may not have been passed
        /// to the constructor, you can use <see cref="P:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.IsConfigured" /> to determine if
        /// the options have already been set, and skip some or all of the logic in
        /// <see cref="M:Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)" />.
        /// </para>
        /// <para>
        /// See <see href="https://aka.ms/efcore-docs-dbcontext">DbContext lifetime, configuration, and initialization</see>
        /// for more information and examples.
        /// </para>
        /// </remarks>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this.config["MannsDb:ConnectionString"]);
            base.OnConfiguring(optionsBuilder);
        }

        /// <summary>
        /// Maps the entity.
        /// </summary>
        /// <param name="bldr">BlogStory Builder..</param>
        private void MapEntity(EntityTypeBuilder<BlogStory> bldr)
        {
            // Override the name of the table because of a RC2 change
            bldr.ToTable("BlogStory");
        }
    }
}
