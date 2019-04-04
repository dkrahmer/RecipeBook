using RecipeBookApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeBookApi.Logic.Contracts
{
    public interface IRecipeLogic
    {
        Task<string> Create(RecipePostPutModel model);
        Task Delete(string id);
        Task<IEnumerable<RecipeViewModel>> GetAll();
        Task<RecipeViewModel> GetById(string id);
        Task Update(string id, RecipePostPutModel model);
    }
}