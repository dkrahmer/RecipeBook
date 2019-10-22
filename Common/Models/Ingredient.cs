using Common.Structs;
using Newtonsoft.Json;
using System.Linq;
using System.Text.RegularExpressions;

namespace Common.Models
{
	public class Ingredient
	{
		private static readonly string REGEX_FRACTION_STRINGS = string.Join("", Amount.FractionMap.Select(m => Regex.Escape(m.Key)));
		private static readonly string FRACTION_REGEX_PARTIAL = @"(?<Fraction>(([0-9]+\s+)?(([0-9]+\/[1-9]+[0-9]*)|([0-9" + REGEX_FRACTION_STRINGS + @"]+))))";
		private static readonly string DECIMAL_REGEX_PARTIAL = @"(?<Decimal>([0-9]+\.[0-9]*)|([0-9]*\.[0-9]+)|[0-9]+)";
		private static readonly string AMOUNT_REGEX_PARTIAL = "(?<Amount>" + FRACTION_REGEX_PARTIAL + "|" + DECIMAL_REGEX_PARTIAL + ")";
		private const string RECIPE_UNIT_NAME_REGEX_PARTIAL = @"(?<Name>(?<Unit>[^\s]*).*)";

		protected static readonly Regex IngredientLineRegEx =
			new Regex($"^({AMOUNT_REGEX_PARTIAL}\\s*)?{RECIPE_UNIT_NAME_REGEX_PARTIAL}$", RegexOptions.Compiled);

		public static Ingredient Parse(string ingredientLine)
		{
			var ingredient = new Ingredient();

			ingredientLine = ingredientLine.Trim();
			if (ingredientLine.StartsWith('[') && ingredientLine.EndsWith(']'))
			{
				ingredient.IsHeading = true;
				ingredient.Name = ingredientLine.Trim('[', ']');
				return ingredient;
			}

			var matches = IngredientLineRegEx.Match(ingredientLine);
			if (!matches.Success)
			{
				// This should never happen. The RegEx should always match but save the raw line
				ingredient.Name = ingredientLine;
				return ingredient;
			}

			ingredient.Unit = matches.Groups["Unit"].Value;
			ingredient.Name = matches.Groups["Name"].Value;
			string amountStr = matches.Groups["Amount"].Value;
			if (!string.IsNullOrWhiteSpace(amountStr))
			{
				if (Amount.TryParse(amountStr, out Amount amount))
					ingredient.Amount = amount;
			}

			return ingredient;
		}

		public Amount Amount { get; set; }

		[JsonIgnore]
		public string Unit { get; private set; }
		public string Name { get; set; }
		public bool IsHeading { get; set; }
	}
}