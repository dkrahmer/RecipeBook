using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace RecipeBookApi.Controllers
{
	[Route("")]
	public class RootController : BaseApiController
	{
		public RootController()
			: base(null)
		{
		}

		[AllowAnonymous]
		[HttpGet]
		[Route("Ping")]
		public IActionResult Ping()
		{
			return Ok($"Pong - Current UTC time: {DateTime.UtcNow.ToString("u")}");
		}
	}
}
