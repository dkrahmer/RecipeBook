using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeBookApi.Models;
using RecipeBookApi.Services.Contracts;
using System;

namespace RecipeBookApi.Controllers
{
	[Authorize]
	[ApiController]
	public abstract class BaseApiController : ControllerBase
	{
		protected IAuthService AuthService { get; }

		protected AppUserClaimModel CurrentUser => AuthService.GetUserFromClaims(User);

		protected BaseApiController(IAuthService authService)
		{
			AuthService = authService;
		}

		protected bool TryGetNotModifiedResult(DateTime lastUpdate, out IActionResult notModifiedResult)
		{
			notModifiedResult = null;
			lastUpdate = lastUpdate.ToUniversalTime();
			string ifModifiedSinceStr = HttpContext.Request.Headers["If-Modified-Since"];   // Example header: If-Modified-Since: Thu, 30 May 2019 05:28:46 GMT
			bool isModified = true;
			try
			{
				if (string.IsNullOrEmpty(ifModifiedSinceStr))
					return false;

				if (!DateTime.TryParse(ifModifiedSinceStr, out DateTime ifModifiedSince))
					return false;

				ifModifiedSince = ifModifiedSince.ToUniversalTime();
				if (lastUpdate < ifModifiedSince)
					return false;

				isModified = false;
			}
			finally // Use finally to make sure to add the Last-Modified header if returning false
			{
				// Add the Last-Modified header to allow the client to later use it in If-Modified-Since
				if (isModified)
					HttpContext.Response.Headers.Add("Last-Modified", lastUpdate.ToString("r"));
			}

			string ifNoneMatch = HttpContext.Request.Headers["If-None-Match"];              // Example header: If-None-Match: "7f0-589c3b4a69f80"
			if (!string.IsNullOrEmpty(ifNoneMatch))
			{
				// Not changed since the ifModifiedSince date/time
				HttpContext.Response.Headers.Add("ETag", ifNoneMatch);
			}

			notModifiedResult = new NotModifiedResult();
			return true;
		}
	}
}
