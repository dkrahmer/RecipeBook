using Common.Models;
using Common.MySql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using RecipeBookApi.Options;
using RecipeBookApi.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RecipeBookApi.Services
{
	public class MySqlRecipesService : BaseRecipesService, IRecipesService
	{
		private readonly Regex _extractSearchTermsRegex = new Regex("(?<=\")[^\"]*(?=\")|[^\" ]+"); // extract quoted terms and standalone words

		public MySqlRecipesService(IOptionsSnapshot<AppOptions> options) : base(options)
		{
		}

		public override async Task<IEnumerable<RecipeSummary>> Find(string nameSearch, IEnumerable<string> tags)
		{
			bool filterByName = !(string.IsNullOrWhiteSpace(nameSearch) || nameSearch == "*" || nameSearch == ".");
			bool filterByTags = !(tags == null || !tags.Any());
			var searchTerms = filterByName ? _extractSearchTermsRegex.Matches(nameSearch).Cast<Match>().Select(m => m.Value).ToArray() : new string[0];

			using (var db = new MySqlDbContext(_options.MySqlConnectionString))
			{
				// start with all recipes
				var foundRecipes = db.Recipes.Select(r => r);

				if (filterByTags)
				{
					// Match all tags
					foreach (string tag in tags)
					{
						foundRecipes = foundRecipes
							.Where(r => db.RecipeTags
								.Join(db.Tags,
									rt => rt.TagId,
									t => t.TagId,
									(rt, t) => new { rt.RecipeId, t.TagName })
								.Where(t => t.TagName == tag)
								.Select(rt => rt.RecipeId)
								.Contains(r.RecipeId));
					}
				}

				if (filterByName)
				{
					foundRecipes = foundRecipes.Where(r => searchTerms.All(st => r.Name.Contains(st, StringComparison.InvariantCultureIgnoreCase)));
				}

				return await foundRecipes
					.Select(r => new RecipeSummary()
					{
						RecipeId = r.RecipeId,
						Name = r.Name,
						CreateDateTime = DateTime.SpecifyKind(r.CreateDateTime, DateTimeKind.Utc),
						UpdateDateTime = DateTime.SpecifyKind(r.UpdateDateTime, DateTimeKind.Utc)
					})
					.ToArrayAsync();
			}
		}

		public override async Task<IEnumerable<RecipeSummary>> GetAll()
		{
			return await Find(null, null);
		}

		public override async Task<Recipe> Get(int recipeId)
		{
			using (var db = new MySqlDbContext(_options.MySqlConnectionString))
			{
				var recipe = await db.Recipes.Where(r => r.RecipeId == recipeId).FirstOrDefaultAsync();

				if (recipe != null)
				{
					recipe.CreateDateTime = DateTime.SpecifyKind(recipe.CreateDateTime, DateTimeKind.Utc);
					recipe.UpdateDateTime = DateTime.SpecifyKind(recipe.UpdateDateTime, DateTimeKind.Utc);

					recipe.Tags = db.RecipeTags
						.Join(db.Tags,
							rt => rt.TagId,
							t => t.TagId,
							(rt, t) => new { rt.RecipeId, t.TagName })
						.Where(rt => rt.RecipeId == recipeId)
						.Select(rt => rt.TagName)
						.OrderBy(t => t)
						.ToList();
				}

				return recipe;
			}
		}

		public override async Task<int> Create(Recipe recipe)
		{
			recipe.CreateDateTime = recipe.UpdateDateTime = DateTime.UtcNow;
			Verify(recipe);
			using (var db = new MySqlDbContext(_options.MySqlConnectionString))
			{
				var addedRecipe = await db.AddAsync(recipe);
				await db.SaveChangesAsync();

				await SetRecipeTags(db, addedRecipe.Entity.RecipeId, recipe.Tags);

				return addedRecipe.Entity.RecipeId;
			}
		}

		private async Task SetRecipeTags(MySqlDbContext db, int recipeId, List<string> tagNames)
		{
			tagNames = tagNames ?? new List<string>();

			// Delete existing tags
			var oldRecipeTags = db.RecipeTags.Where(rt => rt.RecipeId == recipeId);
			db.RemoveRange(oldRecipeTags);

			// Add missing tags
			foreach (string tagName in tagNames)
			{
				var tag = new Tag() { TagName = tagName };
				db.Tags.AddIfNotExists(tag, t => t.TagName);
			}
			await db.SaveChangesAsync();

			// Get tag IDs
			var tags = db.Tags.Where(rt => tagNames.Contains(rt.TagName));
			var recipeTags = tags.Select(t => new RecipeTag() { RecipeId = recipeId, TagId = t.TagId });

			// Add recipe tag associations
			await db.AddRangeAsync(recipeTags);

			// Save 
			await db.SaveChangesAsync();
		}

		public override async Task Update(Recipe recipe)
		{
			recipe.UpdateDateTime = DateTime.UtcNow;
			Verify(recipe);
			using (var db = new MySqlDbContext(_options.MySqlConnectionString))
			{
				var updatededRecipe = db.Update(recipe);
				await db.SaveChangesAsync();

				await SetRecipeTags(db, recipe.RecipeId, recipe.Tags);
			}
		}

		private void Verify(Recipe recipe)
		{
			if (recipe.Name.Length > 50)
				recipe.Name = recipe.Name.Substring(0, 50);
		}

		public override async Task Delete(int recipeId)
		{
			using (var db = new MySqlDbContext(_options.MySqlConnectionString))
			{
				await SetRecipeTags(db, recipeId, null);

				var deletedRecipe = db.Remove(new Recipe() { RecipeId = recipeId });
				await db.SaveChangesAsync();
			}
		}

		public override async Task<List<string>> GetTags()
		{
			using (var db = new MySqlDbContext(_options.MySqlConnectionString))
			{
				return await db.Tags
					.Select(t => t.TagName)
					.ToListAsync();
			}
		}
	}

	// Class copied from https://stackoverflow.com/questions/31162576/entity-framework-add-if-not-exist-without-update
	public static class DbSetExtensions
	{
		public static EntityEntry<TEnt> AddIfNotExists<TEnt, TKey>(this DbSet<TEnt> dbSet, TEnt entity, Func<TEnt, TKey> predicate) where TEnt : class
		{
			var exists = dbSet.Any(c => predicate(entity).Equals(predicate(c)));
			return exists
				? null
				: dbSet.Add(entity);
		}

		public static void AddRangeIfNotExists<TEnt, TKey>(this DbSet<TEnt> dbSet, IEnumerable<TEnt> entities, Func<TEnt, TKey> predicate) where TEnt : class
		{
			var entitiesExist = from ent in dbSet
								where entities.Any(add => predicate(ent).Equals(predicate(add)))
								select ent;

			dbSet.AddRange(entities.Except(entitiesExist));
		}
	}
}
