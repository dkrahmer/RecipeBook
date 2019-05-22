using Common.Models;
using Common.MySql;
using Microsoft.Extensions.Options;
using RecipeBookApi.Options;
using RecipeBookApi.Services.Contracts;
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
					CreateDateTime = r.CreateDateTime,
					UpdateDateTime = r.UpdateDateTime
				}).ToArray();
			}
		}

		public Recipe Get(int recipeId)
		{
			using (var db = new MySqlDbContext(_options.MySqlConnectionString))
			{
				return db.Recipes.Where(p => p.RecipeId == recipeId).FirstOrDefault();
			}
		}

		public int Create(Recipe recipe)
		{
			using (var db = new MySqlDbContext(_options.MySqlConnectionString))
			{
				var addedRecipe = db.Add(recipe);
				db.SaveChanges();
				return addedRecipe.Entity.RecipeId;
			}
		}

		public void Update(Recipe receipe)
		{
			using (var db = new MySqlDbContext(_options.MySqlConnectionString))
			{
				var updatededRecipe = db.Update(receipe);
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
