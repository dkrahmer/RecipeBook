using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RecipeBookApi.Attributes;
using RecipeBookApi.Options;
using RecipeBookApi.Services.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace RecipeBookApi.Controllers
{
	[Route("Recipes/{recipeId}/Images")]
	public class RecipeImagesController : BaseApiController
	{
		private readonly IRecipeImagesService _recipeImagesService;
		private readonly AppOptions _appOptions;

		public RecipeImagesController(IAuthService authService, IRecipeImagesService recipeImagesService, IOptionsSnapshot<AppOptions> appOptions)
					: base(authService)
		{
			_recipeImagesService = recipeImagesService;
			_appOptions = appOptions.Value;
		}

		[RequirePermission("CanViewRecipe")]
		[HttpGet]
		[Route("")]
		[ProducesResponseType(typeof(IEnumerable<string>), (int) HttpStatusCode.OK)]
		public IActionResult GetImageIds([FromRoute] int recipeId)
		{
			return Ok(new Dictionary<string, object>() { { "ImageIds", _recipeImagesService.GetImageIds(recipeId) } });
		}

		[RequirePermission("CanViewRecipe")]
		[HttpGet]
		[Route("{imageId}")]
		[Route("{imageId}/{size}")]
		[ProducesResponseType(typeof(IEnumerable<string>), (int) HttpStatusCode.OK)]
		public IActionResult GetImage([FromRoute] int recipeId, [FromRoute] string size, [FromRoute] int imageId)
		{
			FileInfo imageFileInfo = null;
			try
			{
				imageFileInfo = _recipeImagesService.GetImageFileInfo(recipeId, imageId, size);
			}
			catch (FileNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
			}

			if (!imageFileInfo.Exists)
				return NotFound();

			if (TryGetNotModifiedResult(imageFileInfo.LastWriteTimeUtc, out IActionResult notModifiedResult))
				return notModifiedResult;

			var imageFileStream = new FileStream(imageFileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);

			return File(imageFileStream, $"image/{imageFileInfo.Extension.Trim('.').ToLower()}");
		}

		[RequirePermission("CanEditRecipe")]
		[HttpPost]
		[Route("")]
		[ProducesResponseType(typeof(IEnumerable<string>), (int) HttpStatusCode.OK)]
		public async Task<IActionResult> AddImageAsync([FromRoute] int recipeId, List<IFormFile> files)
		{
			if (files.Count == 0)
				return BadRequest("No files uploaded.");

			List<int> newImageIds = null;
			try
			{
				newImageIds = await _recipeImagesService.AddImageAsync(recipeId, files);
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
			}

			return Ok(new Dictionary<string, object>() { { "ImageIds", newImageIds } });
		}

		[RequirePermission("CanEditRecipe")]
		[HttpDelete]
		[Route("{imageId}")]
		[ProducesResponseType(typeof(IEnumerable<string>), (int) HttpStatusCode.OK)]
		public IActionResult DeleteImage([FromRoute] int recipeId, [FromRoute] int imageId)
		{
			try
			{
				_recipeImagesService.DeleteImage(recipeId, imageId);
			}
			catch (ApplicationException ex)
			{
				return StatusCode(500, ex.Message);
			}

			return Ok();
		}
	}
}
