using Common.Models;
using Common.Structs;
using Microsoft.AspNetCore.Mvc;
using RecipeBookApi.Attributes;
using RecipeBookApi.Models;
using RecipeBookApi.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RecipeBookApi.Controllers
{
	[Route("[controller]")]
	public class RecipesController : BaseApiController
	{
		private readonly IRecipesService _recipesService;

		public RecipesController(IAuthService authService, IRecipesService recipesService)
					: base(authService)
		{
			_recipesService = recipesService;
		}

		[RequirePermission("CanViewRecipe")]
		[HttpGet]
		[Route("")]
		[ProducesResponseType((int) HttpStatusCode.NotModified)]
		[ProducesResponseType(typeof(IEnumerable<RecipeViewModel>), (int) HttpStatusCode.OK)]
		public async Task<IActionResult> GetRecipeList()
		{
			var allRecipes = await _recipesService.GetAll();

			DateTime lastRecipeUpdate = allRecipes.Max(r => r.UpdateDateTime);
			if (TryGetNotModifiedResult(lastRecipeUpdate, out IActionResult notModifiedResult))
				return notModifiedResult;

			return Ok(allRecipes);
		}

		[RequirePermission("CanViewRecipe")]
		[HttpGet]
		[Route("{recipeId}")]
		[ProducesResponseType((int) HttpStatusCode.NotModified)]
		[ProducesResponseType((int) HttpStatusCode.NotFound)]
		[ProducesResponseType(typeof(RecipeViewModel), (int) HttpStatusCode.OK)]
		public async Task<IActionResult> GetRecipe(int recipeId, [FromQuery] string scale)
		{
			var recipe = await _recipesService.Get(recipeId);
			if (recipe == null)
				return NotFound();

			if (TryGetNotModifiedResult(recipe.UpdateDateTime, out IActionResult notModifiedResult))
				return notModifiedResult;

			if (!string.IsNullOrEmpty(scale) && Amount.TryParse(scale, out Amount scaleAmount) && scaleAmount.ToString() != "1")
			{
				foreach (var ingredient in recipe.IngredientsList)
				{
					if (ingredient.Amount.IsEmpty)
						continue;

					ingredient.Amount *= scaleAmount;

					if (_alwaysDecimalUnits.Contains(ingredient.Unit))
						ingredient.Amount = ingredient.Amount.ToDecimalAmount();
				}
			}

			return Ok(recipe);
		}

		private HashSet<string> _alwaysDecimalUnits = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase) { "g", "mg", "kg", "l", "ml" };

		[RequirePermission("CanEditRecipe")]
		[HttpPost]
		[Route("")]
		[ProducesResponseType(typeof(string), (int) HttpStatusCode.BadRequest)]
		[ProducesResponseType(typeof(Dictionary<string, string[]>), (int) HttpStatusCode.BadRequest)]
		[ProducesResponseType(typeof(string), (int) HttpStatusCode.OK)]
		public async Task<IActionResult> CreateRecipe([FromBody]Recipe data)
		{
			if (data == null)
			{
				ModelState.AddModelError("Body", "No body provided.");
			}

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				var createdId = await _recipesService.Create(data);
				return Ok(createdId);
			}
			catch (Exception ex)
			{
				return BadRequest($"Issue creating a new recipe: {ex.ToString()}");
			}
		}

		[RequirePermission("CanEditRecipe")]
		[HttpPut]
		[Route("{recipeId}")]
		[ProducesResponseType(typeof(string), (int) HttpStatusCode.BadRequest)]
		[ProducesResponseType(typeof(Dictionary<string, string[]>), (int) HttpStatusCode.BadRequest)]
		[ProducesResponseType((int) HttpStatusCode.NotFound)]
		[ProducesResponseType((int) HttpStatusCode.OK)]
		public async Task<IActionResult> UpdateRecipe(string recipeId, [FromBody]Recipe data)
		{
			if (string.IsNullOrWhiteSpace(recipeId))
			{
				ModelState.AddModelError(nameof(recipeId), "No ID provided to update.");
			}

			if (data == null)
			{
				ModelState.AddModelError("Body", "No body provided.");
			}

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				await _recipesService.Update(data);

				return Ok();
			}
			catch (KeyNotFoundException)
			{
				return NotFound();
			}
			catch (Exception ex)
			{
				return BadRequest($"Issue updating a recipe: {ex.ToString()}");
			}
		}

		[HttpDelete]
		[Route("{recipeId}")]
		[ProducesResponseType(typeof(string), (int) HttpStatusCode.BadRequest)]
		[ProducesResponseType((int) HttpStatusCode.NotFound)]
		[ProducesResponseType((int) HttpStatusCode.OK)]
		public async Task<IActionResult> DeleteRecipe(int recipeId)
		{
			try
			{
				await _recipesService.Delete(recipeId);

				return Ok();
			}
			catch (KeyNotFoundException)
			{
				return NotFound();
			}
			catch (Exception ex)
			{
				return BadRequest($"Issue deleting a recipe: {ex.ToString()}");
			}
		}
	}
}
