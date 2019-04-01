using Common.Dynamo.Contracts;
using Common.Dynamo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeBookApi.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeBookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IDynamoStorageRepository<Recipe> _recipeStorage;

        public RecipeController(IDynamoStorageRepository<Recipe> recipeStorage)
        {
            _recipeStorage = recipeStorage;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllRecipes()
        {
            var recipes = await _recipeStorage.ReadAll();
            var model = recipes.Select(r => new RecipeModel(r)).ToList();

            return Ok(model);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{recipeId}")]
        public async Task<IActionResult> GetRecipeById(string recipeId)
        {
            var recipe = await _recipeStorage.Read(recipeId);
            if (recipe == null)
            {
                return NotFound();
            }

            var model = new RecipeModel(recipe);
            return Ok(model);
        }
    }
}