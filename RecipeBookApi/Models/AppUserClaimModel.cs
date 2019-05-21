namespace RecipeBookApi.Models
{
	public class AppUserClaimModel
	{
		public string AppUserId { get; set; }

		public string EmailAddress { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public bool IsAdmin { get; set; }
	}
}
