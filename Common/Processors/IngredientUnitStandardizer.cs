using Common.Models;
using Common.Structs;
using System.Collections.Generic;

namespace Common.Processors
{
	public class IngredientUnitStandardizer
	{
		private readonly Dictionary<string, string> _unitMap;

		public IngredientUnitStandardizer(List<List<string>> unitEquivalents)
		{
			_unitMap = new Dictionary<string, string>();

			if (unitEquivalents != null)
			{
				foreach (var unitEquivalentList in unitEquivalents)
				{
					if (unitEquivalentList == null || unitEquivalentList.Count <= 1)
						continue;

					string preferred = unitEquivalentList[0];
					for (int i = 0; i < unitEquivalentList.Count; i++)
					{
						string nonPreferred = unitEquivalentList[i];

						_unitMap[nonPreferred] = preferred;
					}
				}
			}
		}

		private static Amount One = new Amount(1M);
		private static Amount Zero = new Amount(0M);

		public bool StandardizeUnit(Ingredient ingredient)
		{
			string cleanUnit = ingredient.Unit.Trim(new char[] { ' ', '.' });
			bool changed = false;
			if (_unitMap.TryGetValue(cleanUnit, out string mappedUnit))
			{
				if (mappedUnit == ingredient.Unit)
					return false; // Case standardization

				ingredient.Unit = mappedUnit;
				changed = true;
			}
			// Try again with lower case (for cases like "T" and "t" that are case sensitive)
			else if (_unitMap.TryGetValue(cleanUnit.ToLower(), out mappedUnit))
			{
				ingredient.Unit = mappedUnit;
				changed = true;
			}


			if (ingredient.Unit.EndsWith("s") &&
				(ingredient.Amount == One
					|| (ingredient.Amount < One && ingredient.Amount != Zero && ingredient.Amount.IsFraction)))
			{
				string singularUnit = ingredient.Unit.Substring(0, ingredient.Unit.Length - 1);
				if (_unitMap.TryGetValue(singularUnit, out _))
				{
					// The updated unit with the "s" removed from the end is a valid unit
					ingredient.Unit = singularUnit;
					changed = true;
				}
			}

			return changed;
		}
	}
}
