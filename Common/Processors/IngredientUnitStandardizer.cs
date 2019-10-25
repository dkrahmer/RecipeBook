using Common.Models;
using Common.Structs;
using System;
using System.Collections.Generic;
using System.Data;

namespace Common.Processors
{
	public class IngredientUnitStandardizer
	{
		private readonly Dictionary<string, string> _unitMap;
		private readonly List<UnitConversionRule> _unitAppropriations;
		private readonly List<UnitConversionRule> _metricConversions;
		private readonly HashSet<string> _alwaysDecimalUnits;

		public IngredientUnitStandardizer(List<List<string>> unitEquivalents, List<UnitConversionRule> unitAppropriations, List<UnitConversionRule> metricConversions, List<string> alwaysDecimalUnits)
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

			_unitAppropriations = unitAppropriations ?? new List<UnitConversionRule>();
			_metricConversions = metricConversions ?? new List<UnitConversionRule>();

			_alwaysDecimalUnits = new HashSet<string>(alwaysDecimalUnits ?? new List<string>(), StringComparer.InvariantCultureIgnoreCase);
		}

		private static Amount Zero = new Amount(0M);
		private static Amount One = new Amount(1M);
		private static Amount Two = new Amount(2M);

		public bool StandardizeUnit(Ingredient ingredient, bool allMetric = false)
		{
			string cleanUnit = ingredient.Unit.Trim(new char[] { ' ', '.' });
			bool changed = false;
			if (_unitMap.TryGetValue(cleanUnit, out string mappedUnit))
			{
				if (mappedUnit != ingredient.Unit)
				{
					ingredient.Unit = mappedUnit;
					changed = true;
				}
			}
			// Try again with lower case (for cases like "T" and "t" that are case sensitive)
			else if (_unitMap.TryGetValue(cleanUnit.ToLower(), out mappedUnit))
			{
				ingredient.Unit = mappedUnit;
				changed = true;
			}

			ConvertUnit(ingredient, _unitAppropriations);

			if (allMetric)
			{
				ConvertUnit(ingredient, _metricConversions);
			}

			if (_alwaysDecimalUnits.Contains(ingredient.Unit) && !ingredient.Amount.IsDecimal)
			{
				ingredient.Amount = ingredient.Amount.ToDecimalAmount();
				changed = true;
			}

			if (ingredient.Unit.EndsWith("s") &&
				(ingredient.Amount == One
					|| (ingredient.Amount < Two && ingredient.Amount != Zero && ingredient.Amount.IsFraction)))
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

		private static void ConvertUnit(Ingredient ingredient, List<UnitConversionRule> unitConversionRules)
		{
			try
			{
				bool unitAppropriated;
				var dt = new DataTable();
				string amount = ingredient.Amount.ToDecimalAmount().ToString();
				int iterations = 0;
				do
				{
					unitAppropriated = false;
					foreach (var unitConversionRule in unitConversionRules)
					{
						if (ingredient.Unit != unitConversionRule.InputUnit)
							continue;

						bool rulesMatch = true;
						if (unitConversionRule.Rules != null)
						{
							foreach (var rule in unitConversionRule.Rules)
							{
								string ruleEquation = rule.Replace("{{value}}", amount);
								if (!Convert.ToBoolean(dt.Compute(ruleEquation, "")))
								{
									rulesMatch = false;
									break;
								}
							}
						}

						if (!rulesMatch)
							continue;

						// The rules match! Let's convert it
						string unitEquation = unitConversionRule.ConversionEquation.Replace("{{value}}", amount);
						var newAmountDecimal = Convert.ToDecimal(dt.Compute(unitEquation, ""));
						amount = newAmountDecimal.ToString();
						var newAmount = new Amount(newAmountDecimal);
						newAmount = newAmount.ToFractionAmount();

						ingredient.Amount = newAmount;
						ingredient.Unit = unitConversionRule.OutputUnit;
						unitAppropriated = true;
					}

					if (++iterations > 20)
						throw new ApplicationException("Too many iterations while calculating the appropriate unit. Check unit conversion rules for cyclical conversion loops.");
				}
				while (unitAppropriated);
			}
			catch (Exception ex)
			{
				throw;
			}
		}
	}
}
