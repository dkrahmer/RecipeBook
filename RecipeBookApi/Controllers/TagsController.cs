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
	public class TagsController : BaseApiController
	{
		private readonly IRecipesService _recipesService;
		private readonly AppOptions _appOptions;

		public TagsController(IAuthService authService, IRecipesService recipesService, IOptionsSnapshot<AppOptions> appOptions)
					: base(authService)
		{
			_recipesService = recipesService;
			_appOptions = appOptions.Value;
		}

		[RequirePermission("CanViewRecipe")]
		[HttpGet]
		[Route("")]
		[ProducesResponseType((int) HttpStatusCode.NotModified)]
		[ProducesResponseType(typeof(IEnumerable<string>), (int) HttpStatusCode.OK)]
		public async Task<IActionResult> GetTags()
		{
			var tags = await _recipesService.GetTags();

			return Ok(tags);
		}
	}
}
