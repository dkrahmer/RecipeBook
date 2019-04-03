using RecipeBookApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeBookApi.Logic.Contracts
{
    public interface IRecipeLogic
    {
        Task<string> Create(RecipePostModel recipePostModel);
        Task Delete(string id);
        Task<IEnumerable<RecipeViewModel>> GetAll();
        Task<RecipeViewModel> GetById(string id);
        Task Update(RecipePutModel recipePutModel);
    }
}