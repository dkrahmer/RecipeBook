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
		Task<IEnumerable<RecipeSummary>> Find(string nameSearch);
		Task<Recipe> Get(int recipeId);
		Task Update(Recipe model);
	}
}
