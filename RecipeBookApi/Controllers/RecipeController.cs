using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeBookApi.Models;
using RecipeBookApi.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Net;

namespace RecipeBookApi.Controllers
{
	[Route("api/[controller]")]
	public class RecipeController : ControllerBase
	{
		private readonly IRecipeService _recipeService;

		public RecipeController(IRecipeService recipeService)
		{
			_recipeService = recipeService;
		}

		[AllowAnonymous]
		[HttpGet]
		[Route("")]
		[ProducesResponseType(typeof(IEnumerable<RecipeViewModel>), (int)HttpStatusCode.OK)]
		public IActionResult GetAllRecipes()
		{
			var allRecipes = _recipeService.GetAll();

			return Ok(allRecipes);
		}

		[AllowAnonymous]
		[HttpGet]
		[Route("{recipeId}")]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		[ProducesResponseType(typeof(RecipeViewModel), (int)HttpStatusCode.OK)]
		public IActionResult GetRecipe(int recipeId)
		{
			var recipe = _recipeService.Get(recipeId);
			if (recipe == null)
			{
				return NotFound();
			}

			return Ok(recipe);
		}

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
				var createdId = _recipeService.Create(data);
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
				_recipeService.Update(data);

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
				_recipeService.Delete(recipeId);

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
