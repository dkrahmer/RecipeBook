using Common.Models;
using Microsoft.EntityFrameworkCore;
using MySql.Data.EntityFrameworkCore.Extensions;

namespace Common.MySql
{
	public class MySqlDbContext : DbContext
	{
		private readonly string _connectionString;

		public DbSet<Recipe> Recipes { get; set; }

		public DbSet<AppUser> AppUsers { get; set; }

		public DbSet<DensityMapData> VolumeToMassConversions { get; set; }

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

			modelBuilder.Entity<DensityMapData>(entity =>
			{
				entity.ForMySQLHasCharset("utf8").ForMySQLHasCollation("utf8_unicode_ci");

				entity.HasKey(e => e.Name);

				entity.Property(e => e.Density).IsRequired();
			});
		}
	}
}
