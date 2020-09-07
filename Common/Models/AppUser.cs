using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models
{
	public class AppUser
	{
		public int AppUserId { get; set; }
		public string Username { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public int DefaultRecipeGroupId { get; set; }
		public DateTime? LastLoggedInDate { get; set; }
		public DateTime CreateDateTime { get; set; }
		public DateTime UpdateDateTime { get; set; }

		[NotMapped]
		public string FullName => $"{FirstName} {LastName}";
	}
}
