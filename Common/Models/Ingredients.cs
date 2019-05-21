using System;
using System.Collections.Generic;
using System.IO;

namespace Common.Models
{
	public class Ingredients : List<Ingredient>
	{
		public static Ingredients GetIngredientsStructure(string ingredients)
		{
			var ingredientsStructure = new Ingredients();

			if (string.IsNullOrWhiteSpace(ingredients))
				return ingredientsStructure;

			using (StringReader reader = new StringReader(ingredients))
			{
				string line;
				do
				{
					line = reader.ReadLine();
					if (line == null)
						break;

					if (string.IsNullOrWhiteSpace(line))
						continue;

					line = line.Trim();

					var ingredient = Ingredient.Parse(line);

					ingredientsStructure.Add(ingredient);
				} while (line != null);
			}

			return ingredientsStructure;
		}
	}
}