using RecipeBookApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeBookApi.Logic.Contracts
{
    public interface IAppUserLogic
    {
        Task<IEnumerable<AppUserViewModel>> GetAll();
    }
}