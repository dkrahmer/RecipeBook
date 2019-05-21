using System;

namespace Common.Models
{
	public class Recipe
	{
		public int RecipeId { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public string Ingredients { get; set; }

		public string Instructions { get; set; }

		public string Notes { get; set; }

		public DateTime CreateDateTime { get; set; }

		public DateTime UpdateDateTime { get; set; }
	}
}
