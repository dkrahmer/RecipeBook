using Common.Models;
using Common.MySql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RecipeBookApi.Options;
using RecipeBookApi.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeBookApi.Services
{
	public class MySqlRecipesService : IRecipesService
	{
		private AppOptions _options;
		public MySqlRecipesService(IOptions<AppOptions> options)
		{
			_options = options.Value;
		}

		public async Task<IEnumerable<RecipeSummary>> GetAll()
		{
			using (var db = new MySqlDbContext(_options.MySqlConnectionString))
			{
				return await db.Recipes.Select(r => new RecipeSummary()
				{
					RecipeId = r.RecipeId,
					Name = r.Name,
					CreateDateTime = DateTime.SpecifyKind(r.CreateDateTime, DateTimeKind.Utc),
					UpdateDateTime = DateTime.SpecifyKind(r.UpdateDateTime, DateTimeKind.Utc)
				}).ToArrayAsync();
			}
		}

		public async Task<Recipe> Get(int recipeId)
		{
			using (var db = new MySqlDbContext(_options.MySqlConnectionString))
			{
				var recipe = await db.Recipes.Where(r => r.RecipeId == recipeId).FirstOrDefaultAsync();

				if (recipe != null)
				{
					recipe.CreateDateTime = DateTime.SpecifyKind(recipe.CreateDateTime, DateTimeKind.Utc);
					recipe.UpdateDateTime = DateTime.SpecifyKind(recipe.UpdateDateTime, DateTimeKind.Utc);
				}

				return recipe;
			}
		}

		public async Task<int> Create(Recipe recipe)
		{
			recipe.CreateDateTime = recipe.UpdateDateTime = DateTime.UtcNow;
			using (var db = new MySqlDbContext(_options.MySqlConnectionString))
			{
				var addedRecipe = await db.AddAsync(recipe);
				await db.SaveChangesAsync();
				return addedRecipe.Entity.RecipeId;
			}
		}

		public async Task Update(Recipe recipe)
		{
			recipe.UpdateDateTime = DateTime.UtcNow;
			using (var db = new MySqlDbContext(_options.MySqlConnectionString))
			{
				var updatededRecipe = db.Update(recipe);
				await db.SaveChangesAsync();
			}
		}

		public async Task Delete(int recipeId)
		{
			using (var db = new MySqlDbContext(_options.MySqlConnectionString))
			{
				var deletedRecipe = db.Remove(new Recipe() { RecipeId = recipeId });
				await db.SaveChangesAsync();
			}
		}
	}
}
