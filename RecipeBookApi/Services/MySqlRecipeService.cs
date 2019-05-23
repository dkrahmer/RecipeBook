using Common.Models;
using Common.MySql;
using Microsoft.Extensions.Options;
using RecipeBookApi.Options;
using RecipeBookApi.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipeBookApi.Services
{
	public class MySqlRecipeService : IRecipesService
	{
		private AppOptions _options;
		public MySqlRecipeService(IOptions<AppOptions> options)
		{
			_options = options.Value;
		}

		public IEnumerable<RecipeSummary> GetAll()
		{
			using (var db = new MySqlDbContext(_options.MySqlConnectionString))
			{
				return db.Recipes.Select(r => new RecipeSummary()
				{
					RecipeId = r.RecipeId,
					Name = r.Name,
					CreateDateTime = DateTime.SpecifyKind(r.CreateDateTime, DateTimeKind.Utc),
					UpdateDateTime = DateTime.SpecifyKind(r.UpdateDateTime, DateTimeKind.Utc)
				}).ToArray();
			}
		}

		public Recipe Get(int recipeId)
		{
			using (var db = new MySqlDbContext(_options.MySqlConnectionString))
			{
				var recipe = db.Recipes.Where(p => p.RecipeId == recipeId).FirstOrDefault();

				if (recipe != null)
				{
					recipe.CreateDateTime = DateTime.SpecifyKind(recipe.CreateDateTime, DateTimeKind.Utc);
					recipe.UpdateDateTime = DateTime.SpecifyKind(recipe.UpdateDateTime, DateTimeKind.Utc);
				}

				return recipe;
			}
		}

		public int Create(Recipe recipe)
		{
			recipe.CreateDateTime = recipe.UpdateDateTime = DateTime.UtcNow;
			using (var db = new MySqlDbContext(_options.MySqlConnectionString))
			{
				var addedRecipe = db.Add(recipe);
				db.SaveChanges();
				return addedRecipe.Entity.RecipeId;
			}
		}

		public void Update(Recipe recipe)
		{
			recipe.UpdateDateTime = DateTime.UtcNow;
			using (var db = new MySqlDbContext(_options.MySqlConnectionString))
			{
				var updatededRecipe = db.Update(recipe);
				db.SaveChanges();
			}
		}

		public void Delete(int recipeId)
		{
			using (var db = new MySqlDbContext(_options.MySqlConnectionString))
			{
				var deletedRecipe = db.Remove(new Recipe() { RecipeId = recipeId });
				db.SaveChanges();
			}
		}
	}
}
