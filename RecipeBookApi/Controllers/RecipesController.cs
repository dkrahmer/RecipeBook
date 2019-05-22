using Common.Models;
using Common.Structs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeBookApi.Models;
using RecipeBookApi.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Net;

namespace RecipeBookApi.Controllers
{
	[Route("[controller]")]
	public class RecipesController : ControllerBase
	{
		private readonly IRecipesService _recipesService;

		public RecipesController(IRecipesService recipesService)
		{
			_recipesService = recipesService;
		}

		[AllowAnonymous]
		[HttpGet]
		[Route("")]
		[ProducesResponseType(typeof(IEnumerable<RecipeViewModel>), (int)HttpStatusCode.OK)]
		public IActionResult GetRecipeList()
		{
			var allRecipes = _recipesService.GetAll();

			return Ok(allRecipes);
		}

		[AllowAnonymous]
		[HttpGet]
		[Route("{recipeId}")]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		[ProducesResponseType(typeof(RecipeViewModel), (int)HttpStatusCode.OK)]
		public IActionResult GetRecipe(int recipeId, [FromQuery] string scale)
		{
			var recipe = _recipesService.Get(recipeId);
			if (recipe == null)
			{
				return NotFound();
			}

			if (scale != null && Amount.TryParse(scale, out Amount scaleAmount) && scaleAmount.ToString() != "1")
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

		HashSet<string> _alwaysDecimalUnits = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase) { "g", "mg", "kg", "l", "ml" };

		[HttpPost]
		[Route("")]
		[ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
		[ProducesResponseType(typeof(Dictionary<string, string[]>), (int)HttpStatusCode.BadRequest)]
		[ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
		public IActionResult CreateRecipe([FromBody]Recipe data)
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
				var createdId = _recipesService.Create(data);
				return Ok(createdId);
			}
			catch (Exception ex)
			{
				return BadRequest($"Issue creating a new recipe: {ex.Message}");
			}
		}

		[HttpPut]
		[Route("{recipeId}")]
		[ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
		[ProducesResponseType(typeof(Dictionary<string, string[]>), (int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		public IActionResult UpdateRecipe(string recipeId, [FromBody]Recipe data)
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
				_recipesService.Update(data);

				return Ok();
			}
			catch (KeyNotFoundException)
			{
				return NotFound();
			}
			catch (Exception ex)
			{
				return BadRequest($"Issue updating a recipe: {ex.Message}");
			}
		}

		[HttpDelete]
		[Route("{recipeId}")]
		[ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		public IActionResult DeleteRecipe(int recipeId)
		{
			try
			{
				_recipesService.Delete(recipeId);

				return Ok();
			}
			catch (KeyNotFoundException)
			{
				return NotFound();
			}
			catch (Exception ex)
			{
				return BadRequest($"Issue deleting a recipe: {ex.Message}");
			}
		}
	}
}
