using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models
{
	public class RecipeSummary
	{
		public int RecipeId { get; set; }
		public string Name { get; set; }
		//public string Description { get; set; }
		public DateTime CreateDateTime { get; set; }
		public DateTime UpdateDateTime { get; set; }
	}
}
