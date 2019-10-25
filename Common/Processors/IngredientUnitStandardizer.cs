using Common.Models;
using Common.Structs;
using DuoVia.FuzzyStrings;
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
		private readonly List<DensityMap> _volumeToMassConversions;

		public IngredientUnitStandardizer(List<List<string>> unitEquivalents, List<UnitConversionRule> unitAppropriations, List<UnitConversionRule> metricConversions, List<string> alwaysDecimalUnits, List<DensityMap> volumeToMassConversions)
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
			_volumeToMassConversions = volumeToMassConversions ?? new List<DensityMap>();
		}

		private static Amount Zero = new Amount(0M);
		private static Amount One = new Amount(1M);
		private static Amount Two = new Amount(2M);

		public bool StandardizeUnit(Ingredient ingredient, bool allMetric = false, bool convertToMass = false)
		{
			if (ingredient.Amount.IsEmpty)
				return false;

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

			if (convertToMass)
			{
				ConvertUnitToMass(ingredient);
			}

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

		private void ConvertUnitToMass(Ingredient ingredient)
		{
			if (ingredient.Unit == "g" || ingredient.Unit == "kg" || ingredient.Unit == "oz" || ingredient.Unit == "lbs") // TODO: add a config value for these
				return; // unit is alread in mass

			Amount amountMl;

			if (ingredient.Unit != "ml" && ingredient.Unit != "L")
			{
				var ingredientCopy = new Ingredient()
				{
					Unit = ingredient.Unit,
					Amount = new Amount(ingredient.Amount)
				};
				ConvertUnit(ingredientCopy, _metricConversions); // Convert to metric volume, if possible
			}

			if (ingredient.Unit == "ml")
			{
				amountMl = ingredient.Amount;
			}
			else if (ingredient.Unit == "L")
			{
				amountMl = ingredient.Amount * new Amount(1000M);
			}
			else
			{
				return; // cannot work with this unit type
			}

			string cleanIngredientName = ingredient.GetCleanName();

			string bestLevenshteinDistanceName = null;
			double bestLevenshteinDistanceScore = 1000;
			decimal bestLevenshteinDistanceDensity = 0;

			string bestLongestCommonSubsequenceName = null;
			double bestLongestCommonSubsequenceScore = 0;
			decimal bestLongestCommonSubsequenceDensity = 0;

			string bestDiceCoefficientName = null;
			double bestDiceCoefficientScore = 0;
			decimal bestDiceCoefficientDensity = 0;

			foreach (var volumeToMassConversion in _volumeToMassConversions)
			{
				foreach (string name in volumeToMassConversion.Names)
				{
					//bool fuzzyEquals = name.FuzzyEquals(cleanIngredientName);
					//double fuzzyMatch = name.FuzzyMatch(cleanIngredientName);
					int levenshteinDistance = cleanIngredientName.LevenshteinDistance(name);
					Tuple<string, double> longestCommonSubsequence = cleanIngredientName.LongestCommonSubsequence(name);
					double diceCoefficient = cleanIngredientName.DiceCoefficient(name);

					if (levenshteinDistance < bestLevenshteinDistanceScore)
					{
						bestLevenshteinDistanceScore = levenshteinDistance;
						bestLevenshteinDistanceName = name;
						bestLevenshteinDistanceDensity = volumeToMassConversion.Density;
					}

					if (longestCommonSubsequence.Item2 > bestLongestCommonSubsequenceScore)
					{
						bestLongestCommonSubsequenceScore = longestCommonSubsequence.Item2;
						bestLongestCommonSubsequenceName = name;
						bestLongestCommonSubsequenceDensity = volumeToMassConversion.Density;
					}

					if (diceCoefficient > bestDiceCoefficientScore)
					{
						bestDiceCoefficientScore = diceCoefficient;
						bestDiceCoefficientName = name;
						bestDiceCoefficientDensity = volumeToMassConversion.Density;
					}
				}
			}

			//if (string.IsNullOrEmpty(bestLongestCommonSubsequenceName))
			//	return; // No matches found

			if (bestLongestCommonSubsequenceScore > 0.60)
			{
				ingredient.Unit = "g";
				ingredient.Amount *= new Amount(bestLongestCommonSubsequenceDensity);
				ConvertUnit(ingredient, _unitAppropriations);
			}
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
