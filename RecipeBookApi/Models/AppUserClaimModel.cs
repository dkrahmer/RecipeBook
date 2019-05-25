namespace RecipeBookApi.Models
{
	public class AppUserClaimModel
	{
		public int AppUserId { get; set; }

		public string Username { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public bool CanViewRecipe { get; set; }

		public bool CanEditRecipe { get; set; }

		public bool IsAdmin { get; set; }
	}
}
