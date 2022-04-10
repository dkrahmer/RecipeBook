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
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace RecipeBookApi.Controllers
{
	[Route("[controller]")]
	public class SendToController : BaseApiController
	{
		private readonly AppOptions _appOptions;

		public SendToController(IAuthService authService, IOptionsSnapshot<AppOptions> appOptions)
					: base(authService)
		{
			_appOptions = appOptions.Value;
		}

		[RequirePermission("CanEditRecipe")]
		[HttpPost]
		[Route("{deviceName}")]
		[ProducesResponseType((int) HttpStatusCode.NotModified)]
		[ProducesResponseType(typeof(IEnumerable<string>), (int) HttpStatusCode.OK)]
		public async Task<IActionResult> SendToDevice([FromRoute] string deviceName, [FromBody] Dictionary<string, string> data)
		{
			if (_appOptions.SendToUrls == null || !_appOptions.SendToUrls.TryGetValue(deviceName, out string urlTemplate))
				return BadRequest($"Invalid device name '{deviceName}'.");

			if (data == null || !data.TryGetValue("url", out string url))
				return BadRequest("No URL specified.");

			string targetUrl = urlTemplate
				.Replace("{Url}", url)
				.Replace("{EncodedUrl}", HttpUtility.UrlEncode(url));

			HttpWebRequest request = (HttpWebRequest) WebRequest.Create(targetUrl);
			using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
			{
				if (response.StatusCode != HttpStatusCode.OK)
					return BadRequest("Failed to send URL to device.");
			}

			return Ok(deviceName);
		}
	}
}
