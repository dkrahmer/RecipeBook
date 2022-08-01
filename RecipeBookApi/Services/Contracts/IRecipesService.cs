using Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeBookApi.Services.Contracts
{
	public interface IRecipesService
	{
		Task<int> Create(Recipe model);
		Task Delete(int recipeId);
		Task<IEnumerable<RecipeSummary>> GetAll();
		Task<IEnumerable<RecipeSummary>> Find(string nameSearch, IEnumerable<string> tags);
		Task<Recipe> Get(int recipeId);
		Task<Recipe> Import(string recipeUrl);
		Task Update(Recipe model);
		Task<List<string>> GetTags();
	}
}
