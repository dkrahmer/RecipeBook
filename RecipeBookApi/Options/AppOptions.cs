using Common.Models;
using Common.Processors;
using System.Collections.Generic;
using System.Linq;

namespace RecipeBookApi.Options
{
	public class AppOptions
	{
		public string AllowedOrigins { get; set; }
		public string GoogleClientId { get; set; }
		public string GoogleClientSecret { get; set; }
		public string MySqlConnectionString { get; set; }
		public bool DebugMode { get; set; }
		public List<List<string>> UnitEquivalents { get; set; }
		public Dictionary<string, int> AlwaysDecimalUnits { get; set; }
		public List<UnitConversionRule> UnitAppropriations { get; set; }
		public List<UnitConversionRule> MetricConversions { get; set; }
		public decimal VolumeToMassConversionMinGrams { get; set; }
		public IngredientUnitStandardizer IngredientUnitStandardizer { get; private set; }

		public void SetIngredientUnitStandardizer(IEnumerable<DensityMapData> volumeToMassConversionData)
		{
			if (volumeToMassConversionData == null)
				volumeToMassConversionData = new List<DensityMapData>();

			var volumeToMassConversions = volumeToMassConversionData.Select(d =>
			{
				var names = (d.Name + '|' + (d.AlternateNames ?? ""))
					.Split('|')
					.Select(a => a.Trim())
					.Where(a => !string.IsNullOrEmpty(a));

				return new DensityMap()
				{
					Names = new List<string>(names),
					Density = d.Density
				};
			}).ToList();

			SetIngredientUnitStandardizer(volumeToMassConversions);
		}

		public void SetIngredientUnitStandardizer(List<DensityMap> volumeToMassConversions)
		{
			if (volumeToMassConversions == null)
				volumeToMassConversions = new List<DensityMap>();

			IngredientUnitStandardizer = new IngredientUnitStandardizer(UnitEquivalents, UnitAppropriations, MetricConversions, AlwaysDecimalUnits, volumeToMassConversions, VolumeToMassConversionMinGrams);
		}
	}
}
