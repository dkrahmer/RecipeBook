using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeBookApi.Logic.Contracts;
using RecipeBookApi.Models;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace RecipeBookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeLogic _recipeLogic;

        public RecipeController(IRecipeLogic recipeLogic)
        {
            _recipeLogic = recipeLogic;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(IEnumerable<RecipeViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllRecipes()
        {
            var model = await _recipeLogic.GetAll();

            return Ok(model);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{recipeId}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(RecipeViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetRecipeById(string recipeId)
        {
            var model = await _recipeLogic.GetById(recipeId);
            if (model == null)
            {
                return NotFound();
            }

            return Ok(model);
        }
    }
}