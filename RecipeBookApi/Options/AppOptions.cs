using Common.Contracts;

namespace RecipeBookApi.Options
{
	public class AppOptions  : ICommonOptions
	{
		public string AllowedOrigins { get; set; }
		public bool UseUtcForTokenExpire { get; set; }
		public string EasternTimeZoneId { get; set; }
		public string GoogleClientId { get; set; }
		public string GoogleClientSecret { get; set; }
		public string MySqlConnectionString { get; set; }
	}
}
