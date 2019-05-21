using Common.Models;
using Microsoft.EntityFrameworkCore;
using MySql.Data.EntityFrameworkCore.Extensions;

namespace Common.MySql
{
	public class MySqlDbContext : DbContext
	{
		public DbSet<Recipe> Recipes { get; set; }

		//public DbSet<AppUser> AppUsers { get; set; }

		public MySqlDbContext() : base()
		{
			// Database.EnsureCreated();
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseMySQL("");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Recipe>(entity =>
			{
				entity.HasKey(e => e.RecipeId);
				entity.Property(e => e.RecipeId).UseMySQLAutoIncrementColumn("INT");

				entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
				entity.Property(e => e.CreateDateTime).IsRequired();
				entity.Property(e => e.UpdateDateTime).IsRequired();
			});

			//modelBuilder.Entity<AppUser>(entity =>
			//{
			//	entity.HasKey(e => e.AppUserId);
			//	entity.Property(e => e.Username).IsRequired();
			//	//entity.HasOne(d => d.Publisher)
			//	//  .WithMany(p => p.Books);
			//});
		}
	}
}
