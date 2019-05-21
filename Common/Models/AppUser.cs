/*
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

		public DateTime? LastLoggedInDate { get; set; }

		public bool IsAdmin { get; set; }

		public DateTime CreateDate { get; set; }

		public DateTime UpdateDate { get; set; }

		[NotMapped]
		public string FullName => $"{FirstName} {LastName}";
	}
}
*/