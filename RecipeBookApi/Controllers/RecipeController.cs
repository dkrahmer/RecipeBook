using Common.Dynamo.Contracts;
using Common.Dynamo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetAllRecipesAsync()
        {
            var recipes = await _recipeStorage.ReadAll();

            return Ok(recipes);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{recipeId}")]
        public async Task<IActionResult> GetRecipeByIdAsync(string recipeId)
        {
            var recipe = await _recipeStorage.Read(recipeId);
            if (recipe == null)
            {
                return NotFound();
            }

            return Ok(recipe);
        }
    }
}