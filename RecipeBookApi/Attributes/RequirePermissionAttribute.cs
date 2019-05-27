using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace RecipeBookApi.Attributes
{
	public class RequirePermissionAttribute : ActionFilterAttribute
	{
		public string RequiredPermission { get; }

		public RequirePermissionAttribute(string permissionName)
		{
			RequiredPermission = permissionName;
		}

		public override void OnActionExecuting(ActionExecutingContext actionExecutingContext)
		{
			var httpContext = actionExecutingContext.HttpContext;
			if (!(httpContext.User?.Identity?.IsAuthenticated ?? false))
				return; // We must be in debug mode, let the user have access

			// if (!(httpContext.User?.Identity?.IsAuthenticated ?? false))
			// {
			// 	// User is not logged in.
			// 	actionExecutingContext.Result = new UnauthorizedResult();
			// 	return;
			// }

			// Make sure the user has the required permission
			if (!httpContext.User.Claims.Any(c => c.Type == RequiredPermission && c.Value == true.ToString()))
			{
				// Access denied. Must have the '{RequiredPermission}' permission enabled.
				actionExecutingContext.Result = new ForbidResult();
				return;
			}
		}

		private IActionResult NotLoggedIn()
		{
			return new NotFoundResult();
		}
	}
}