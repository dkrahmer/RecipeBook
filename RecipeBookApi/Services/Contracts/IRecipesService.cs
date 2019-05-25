using Common.Models;
using RecipeBookApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeBookApi.Services.Contracts
{
	public interface IRecipesService
	{
		int Create(Recipe model);
		void Delete(int recipeId);
		IEnumerable<RecipeSummary> GetAll();
		Recipe Get(int recipeId);
		void Update(Recipe model);
	}
}
