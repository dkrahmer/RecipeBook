using Common.Models;
using Common.MySql;
using RecipeBookApi.Services.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace RecipeBookApi.Services
{
	public class MySqlRecipeService : IRecipesService
	{
		public MySqlRecipeService()
		{
		}

		public IEnumerable<RecipeSummary> GetAll()
		{
			using (var db = new MySqlDbContext())
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
			using (var db = new MySqlDbContext())
			{
				return db.Recipes.Where(p => p.RecipeId == recipeId).FirstOrDefault();
			}
		}

		public int Create(Recipe recipe)
		{
			using (var db = new MySqlDbContext())
			{
				var addedRecipe = db.Add(recipe);
				db.SaveChanges();
				return addedRecipe.Entity.RecipeId;
			}
		}

		public void Update(Recipe receipe)
		{
			using (var db = new MySqlDbContext())
			{
				var updatededRecipe = db.Update(receipe);
				db.SaveChanges();
			}
		}

		public void Delete(int recipeId)
		{
			using (var db = new MySqlDbContext())
			{
				//var recipe = db.Recipes.Where(p => p.RecipeId == recipeId).FirstOrDefault();
				//if (recipe == null)
				//	throw new ApplicationException($"Recipe ID '{recipeId}' does not exist.");

				var addedRecipe = db.Remove(new Recipe() { RecipeId = recipeId });
				db.SaveChanges();
			}
		}
		/*
		private static RecipeViewModel CreateRecipeViewModel(Recipe recipe, AppUser owner)
		{
			return new RecipeViewModel
			{
				Id = recipe.Id,
				Name = recipe.Name,
				Ingredients = recipe.Ingredients,
				Instructions = recipe.Instructions,
				Description = recipe.Description,
				OwnerName = owner.FullName,
				UpdateDate = recipe.UpdateDate,
			};
		}
		*/
	}
}
