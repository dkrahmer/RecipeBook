namespace Common.Models
{
	public class AppUserRecipeGroup
	{
		public int AppUserId { get; set; }
		public int RecipeGroupId { get; set; }
		public bool CanViewRecipe { get; set; }
		public bool CanEditRecipe { get; set; }
		public bool IsAdmin { get; set; }
	}
}
