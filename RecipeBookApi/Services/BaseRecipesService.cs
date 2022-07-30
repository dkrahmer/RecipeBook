using Common.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RecipeBookApi.Options;
using RecipeBookApi.Services.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace RecipeBookApi.Services
{
	public abstract class BaseRecipesService : IRecipesService
	{
		protected AppOptions _options;
		public BaseRecipesService(IOptionsSnapshot<AppOptions> options)
		{
			_options = options.Value;
		}

		public virtual async Task<Recipe> Import(string recipeUrl)
		{
			string apiUrl = _options.BaseRecipeScraperApiUrl.Replace(@"{RecipeUrl}", HttpUtility.UrlEncode(recipeUrl));
			HttpWebRequest request = (HttpWebRequest) WebRequest.Create(apiUrl);
			request.ContentType = "application/json";
			try
			{
				using (var response = await request.GetResponseAsync())
				using (var stream = response.GetResponseStream())
				using (var reader = new StreamReader(stream, Encoding.UTF8))
				{
					string json = reader.ReadToEnd();
					var recipe = JsonConvert.DeserializeObject<Recipe>(json);

					string ingredients = recipe.Ingredients;
					bool ingredientsUpdated = false;
					foreach (var ingredient in recipe.IngredientsList)
					{
						string name = Regex.Replace(ingredient.Name, $"\\-?{Ingredient.AMOUNT_REGEX_PARTIAL}", "<$0>");
						if (name != ingredient.Name)
						{
							ingredients = ingredients.Replace(ingredient.Name, name);
							ingredientsUpdated = true;
						}
					}

					if (ingredientsUpdated)
						recipe.Ingredients = ingredients; // Apply to the source ingredients string. This will cause the array to recalculate as needed.

					return recipe;
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public abstract Task<int> Create(Recipe model);
		public abstract Task Delete(int recipeId);
		public abstract Task<IEnumerable<RecipeSummary>> Find(string nameSearch, IEnumerable<string> tags);
		public abstract Task<Recipe> Get(int recipeId);
		public abstract Task<IEnumerable<RecipeSummary>> GetAll();
		public abstract Task<List<string>> GetTags();

		public abstract Task Update(Recipe model);
	}
}
