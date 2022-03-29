using Common;
using Common.Models;
using Common.Structs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RecipeBookApi.Attributes;
using RecipeBookApi.Models;
using RecipeBookApi.Options;
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
		private readonly AppOptions _appOptions;

		public RecipesController(IAuthService authService, IRecipesService recipesService, IOptionsSnapshot<AppOptions> appOptions)
					: base(authService)
		{
			_recipesService = recipesService;
			_appOptions = appOptions.Value;
		}

		[RequirePermission("CanViewRecipe")]
		[HttpGet]
		[Route("")]
		[ProducesResponseType((int) HttpStatusCode.NotModified)]
		[ProducesResponseType(typeof(IEnumerable<Recipe>), (int) HttpStatusCode.OK)]
		public async Task<IActionResult> GetRecipeList([FromQuery] string nameSearch, [FromQuery] string tags)
		{
			var tagsArray = tags?.Split(',')
				.Where(t=>!string.IsNullOrWhiteSpace(t))
				.Select(t => t.Trim())
				.ToArray() ?? new string[0];

			var recipes = await _recipesService.Find(nameSearch, tagsArray);

			if (recipes.Any())
			{
				DateTime lastRecipeUpdate = recipes.Max(r => r.UpdateDateTime);
				if (TryGetNotModifiedResult(lastRecipeUpdate, out IActionResult notModifiedResult))
					return notModifiedResult;
			}

			return Ok(recipes);
		}

		[RequirePermission("CanViewRecipe")]
		[HttpGet]
		[Route("{recipeId}")]
		[ProducesResponseType((int) HttpStatusCode.NotModified)]
		[ProducesResponseType((int) HttpStatusCode.NotFound)]
		[ProducesResponseType(typeof(Recipe), (int) HttpStatusCode.OK)]
		public async Task<IActionResult> GetRecipe(int recipeId, [FromQuery] string scale, [FromQuery] string system, [FromQuery] string convertToMass, [FromQuery] string editing)
		{
			var recipe = await _recipesService.Get(recipeId);
			if (recipe == null)
				return NotFound();

			if (TryGetNotModifiedResult(recipe.UpdateDateTime, out IActionResult notModifiedResult))
				return notModifiedResult;

			bool isEditing = !string.IsNullOrWhiteSpace(editing) && (editing == "1" || editing.Equals("true", StringComparison.InvariantCultureIgnoreCase));
			Amount scaleAmount = new Amount(1M);
			bool needScaling = !string.IsNullOrEmpty(scale) && Amount.TryParse(scale, out scaleAmount) && scaleAmount.ToString() != "1";

			foreach (var ingredient in recipe.IngredientsList)
			{
				if (ingredient.Amount.IsEmpty)
					continue;

				if (needScaling)
					ingredient.Amount *= scaleAmount;

				if (!isEditing)
					ingredient.Name = Helpers.UpdateScalableNumbers(ingredient.Name, scaleAmount);

				bool allMetric = "metric".Equals(system, StringComparison.InvariantCultureIgnoreCase);
				bool isConvertToMass = convertToMass == "1";
				_appOptions.IngredientUnitStandardizer?.StandardizeUnit(ingredient, allMetric: allMetric, convertToMass: isConvertToMass);
			}

			if (!isEditing)
			{
				recipe.Instructions = Helpers.GetCleanLines(recipe.Instructions);
				recipe.Instructions = Helpers.UpdateScalableNumbers(recipe.Instructions, scaleAmount);
				recipe.Notes = Helpers.UpdateScalableNumbers(recipe.Notes, scaleAmount);
			}

			return Ok(recipe);
		}

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
