// MIT License
//
// Copyright (c) 2022 Sascha Manns
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

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
