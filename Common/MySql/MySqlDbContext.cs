using Common.Models;
using Microsoft.EntityFrameworkCore;
using MySql.Data.EntityFrameworkCore.Extensions;

namespace Common.MySql
{
	public class MySqlDbContext : DbContext
	{
		private readonly string _connectionString;

		public DbSet<Recipe> Recipes { get; set; }

		public DbSet<Tag> Tags { get; set; }

		public DbSet<RecipeTag> RecipeTags { get; set; }

		public DbSet<AppUser> AppUsers { get; set; }

		public MySqlDbContext(string connectionString) : base()
		{
			_connectionString = connectionString;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseMySQL(_connectionString);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Recipe>(entity =>
			{
				entity.ForMySQLHasCharset("utf8").ForMySQLHasCollation("utf8_unicode_ci");

				entity.HasKey(e => e.RecipeId);
				entity.Property(e => e.RecipeId).UseMySQLAutoIncrementColumn("INT");

				entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
				entity.Property(e => e.CreateDateTime).IsRequired();
				entity.Property(e => e.UpdateDateTime).IsRequired();
			});

			modelBuilder.Entity<Tag>(entity =>
			{
				entity.ForMySQLHasCharset("utf8").ForMySQLHasCollation("utf8_unicode_ci");

				entity.HasKey(e => e.TagId);
				entity.Property(e => e.TagId).UseMySQLAutoIncrementColumn("INT");

				entity.Property(e => e.TagName).IsRequired();
			});

			modelBuilder.Entity<RecipeTag>(entity =>
			{
				entity.ForMySQLHasCharset("utf8").ForMySQLHasCollation("utf8_unicode_ci");

				entity.HasKey(e => new { e.RecipeId, e.TagId });

				entity.Property(e => e.RecipeId).IsRequired();
				entity.Property(e => e.TagId).IsRequired();
			});

			modelBuilder.Entity<AppUser>(entity =>
			{
				entity.ForMySQLHasCharset("utf8").ForMySQLHasCollation("utf8_unicode_ci");

				entity.HasKey(e => e.AppUserId);
				entity.Property(e => e.AppUserId).UseMySQLAutoIncrementColumn("INT");

				entity.Property(e => e.Username).IsRequired();

				entity.Property(e => e.CanViewRecipe).HasConversion<int>();
				entity.Property(e => e.CanEditRecipe).HasConversion<int>();
				entity.Property(e => e.IsAdmin).HasConversion<int>();
			});
		}
	}
}
