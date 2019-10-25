using Common.Processors;
using System.Collections.Generic;

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
		public List<string> AlwaysDecimalUnits { get; set; }

		private IngredientUnitStandardizer _ingredientUnitStandardizer;
		public IngredientUnitStandardizer IngredientUnitStandardizer
		{
			get
			{
				if (_ingredientUnitStandardizer == null)
				{
					_ingredientUnitStandardizer = new IngredientUnitStandardizer(UnitEquivalents, AlwaysDecimalUnits);
				}

				return _ingredientUnitStandardizer;
			}
		}
	}
}
