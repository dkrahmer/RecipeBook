using System;

namespace RecipeBookApi.Attributes
{
	public class RequirePermissionAttribute : Attribute
	{
		public RequirePermissionAttribute(string permissionName)
		{

		}
	}
}