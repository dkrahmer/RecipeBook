using Fractions;
using System.Text.RegularExpressions;

namespace Common.Models
{
	public class Ingredient
	{
		protected static readonly Regex IngredientLineRegEx =
			new Regex(@"(?i)^((?<Amount>(([0-9]+\s+)?[0-9]+\/[1-9]+[0-9]*)|([0-9]*\.?[0-9]*))\s?)?(?<IngredientName>.+)$", RegexOptions.Compiled);
		// 4 1/2 ingredient
		// 4.5 ingredient
		// 45 ingredient

		public static Ingredient Parse(string ingredientLine)
		{
			var ingredient = new Ingredient();

			ingredientLine = ingredientLine.Trim();
			if (ingredientLine.StartsWith('[') && ingredientLine.EndsWith(']'))
			{
				ingredient.IsHeading = true;
				ingredient.IngredientName = ingredientLine.Trim('[', ']');
				return ingredient;
			}

			var matches = IngredientLineRegEx.Match(ingredientLine);
			if (!matches.Success)
			{
				// This should never happen. The RegEx should always match but save the raw line
				ingredient.IngredientName = ingredientLine;
				return ingredient;
			}

			ingredient.IngredientName = matches.Groups["IngredientName"].Value;
			string amount = matches.Groups["Amount"].Value;
			if (string.IsNullOrWhiteSpace(amount))
				return ingredient;

			//if (Fraction.TryParse(amount, out Fraction amountFraction))
			//	ingredient.Amount = amountFraction;

			return ingredient;
		}

		public Amount Amount { get; set; }
		public string IngredientName { get; set; }
		public bool IsHeading { get; set; }
	}
}