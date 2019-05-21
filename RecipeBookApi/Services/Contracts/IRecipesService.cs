using Common.Models;
using RecipeBookApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeBookApi.Services.Contracts
{
	public interface IRecipeService
	{
		int Create(Recipe model);
		void Delete(int recipeId);
		IEnumerable<Recipe> GetAll();
		Recipe Get(int recipeId);
		void Update(Recipe model);
	}
}
