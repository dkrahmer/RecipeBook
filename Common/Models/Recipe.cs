using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models
{
	public class Recipe
	{
		public int RecipeId { get; set; }
		public string Name { get; set; }

		private string _ingredients;
		public string Ingredients
		{
			get => _ingredients;
			set
			{
				_ingredients = value;
				_ingredientsList = null;
			}
		}

		private Ingredients _ingredientsList;
		[NotMapped]
		public Ingredients IngredientsList
		{
			get
			{
				if (_ingredientsList == null)
				{
					_ingredientsList = Models.Ingredients.GetIngredientsStructure(_ingredients);
				}

				return _ingredientsList;
			}
		}

		public string Instructions { get; set; }

		public string Notes { get; set; }

		public int RecipeGroupId { get; set; }

		public DateTime CreateDateTime { get; set; }
		public DateTime UpdateDateTime { get; set; }
	}
}
