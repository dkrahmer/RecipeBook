using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Security.Authentication;

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
			{
				throw new AuthenticationException("User is not logged in.");
			}

			// Make sure the user has the required permission
			if (!httpContext.User.Claims.Any(c => c.Type == RequiredPermission && c.Value == true.ToString()))
			{
				throw new AuthenticationException($"Uccess denied. Must have the '{RequiredPermission}' permission enabled.");
			}
		}
	}
}