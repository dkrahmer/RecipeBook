using Common.Models;
using Common.Processors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;

namespace CommonTests
{
	[TestClass]
	public class IngredientTests
	{
		[TestMethod]
		public void IngredientParseTest()
		{
			Ingredient ingredient;

			ingredient = Ingredient.Parse("4 2/3 cups Water");
			Assert.AreEqual("4 2/3", ingredient.Amount.ToString());
			Assert.AreEqual("cups", ingredient.Unit);
			Assert.AreEqual("cups Water", ingredient.Name);

			ingredient = Ingredient.Parse("Water");
			Assert.AreEqual("", ingredient.Amount.ToString());
			Assert.AreEqual("Water", ingredient.Unit);
			Assert.AreEqual("Water", ingredient.Name);

			ingredient = Ingredient.Parse("cups of Water");
			Assert.AreEqual("", ingredient.Amount.ToString());
			Assert.AreEqual("cups", ingredient.Unit);
			Assert.AreEqual("cups of Water", ingredient.Name);

			ingredient = Ingredient.Parse("1");
			Assert.AreEqual("1", ingredient.Amount.ToString());
			Assert.AreEqual("", ingredient.Unit);
			Assert.AreEqual("", ingredient.Name);

			ingredient = Ingredient.Parse("2/3 cups Water");
			Assert.AreEqual("2/3", ingredient.Amount.ToString());
			Assert.AreEqual("cups", ingredient.Unit);
			Assert.AreEqual("cups Water", ingredient.Name);

			ingredient = Ingredient.Parse("0002 cups Water");
			Assert.AreEqual("2", ingredient.Amount.ToString());
			Assert.AreEqual("cups", ingredient.Unit);
			Assert.AreEqual("cups Water", ingredient.Name);

			ingredient = Ingredient.Parse("2.3333333333 cups Water");
			Assert.AreEqual("2 1/3", ingredient.Amount.ToString());
			Assert.AreEqual("cups", ingredient.Unit);
			Assert.AreEqual("cups Water", ingredient.Name);

			ingredient = Ingredient.Parse("0001.500 cups Water");
			Assert.AreEqual("1.5", ingredient.Amount.ToString());
			Assert.AreEqual("cups", ingredient.Unit);
			Assert.AreEqual("cups Water", ingredient.Name);

			ingredient = Ingredient.Parse("1. cup Water");
			Assert.AreEqual("1", ingredient.Amount.ToString());
			Assert.AreEqual("cup", ingredient.Unit);
			Assert.AreEqual("cup Water", ingredient.Name);

			ingredient = Ingredient.Parse(".5 cups Water");
			Assert.AreEqual("0.5", ingredient.Amount.ToString());
			Assert.AreEqual("cups", ingredient.Unit);
			Assert.AreEqual("cups Water", ingredient.Name);

			ingredient = Ingredient.Parse("  .6666666 cups Water  ");
			Assert.AreEqual("2/3", ingredient.Amount.ToString());
			Assert.AreEqual("cups", ingredient.Unit);
			Assert.AreEqual("cups Water", ingredient.Name);

			ingredient = Ingredient.Parse("  [Part 1]  ");
			Assert.AreEqual("", ingredient.Amount.ToString());
			Assert.AreEqual(null, ingredient.Unit);
			Assert.AreEqual("Part 1", ingredient.Name);
			Assert.AreEqual(true, ingredient.IsHeading);
		}

		[TestMethod]
		public void IngredientUnitStandardizeTests()
		{
			var unitEquivalents = new List<List<string>>()
			{
				new List<string>(){ "tblsp",  "tbls", "tbls", "tbl", "tb", "tbs", "tbsp", "tablespoon", "tablespoons", "T" },
				new List<string>(){ "tsp", "teaspoons", "teaspoon", "ts", "t" },
				new List<string>(){ "cups", "cup", "c", "cp" }
			};

			var ingredientUnitStandardizer = new IngredientUnitStandardizer(unitEquivalents, null, null, null);

			var ingredient_T = Ingredient.Parse("8 T. Water");
			bool wasChanged_T = ingredientUnitStandardizer.StandardizeUnit(ingredient_T);
			Assert.IsTrue(wasChanged_T);
			Assert.AreEqual("tblsp", ingredient_T.Unit);
			Assert.AreEqual("tblsp Water", ingredient_T.Name);

			wasChanged_T = ingredientUnitStandardizer.StandardizeUnit(ingredient_T);
			Assert.IsFalse(wasChanged_T);
			Assert.AreEqual("tblsp", ingredient_T.Unit);
			Assert.AreEqual("tblsp Water", ingredient_T.Name);

			var ingredient_T2 = Ingredient.Parse("8 Tblsp Water");
			bool wasChanged_T2 = ingredientUnitStandardizer.StandardizeUnit(ingredient_T2);
			Assert.IsTrue(wasChanged_T2);
			Assert.AreEqual("tblsp", ingredient_T2.Unit);
			Assert.AreEqual("tblsp Water", ingredient_T2.Name);

			var ingredient_t = Ingredient.Parse("8 t. Water");
			bool wasChanged_t = ingredientUnitStandardizer.StandardizeUnit(ingredient_t);
			Assert.IsTrue(wasChanged_t);
			Assert.AreEqual("tsp", ingredient_t.Unit);
			Assert.AreEqual("tsp Water", ingredient_t.Name);

			var ingredient_cups = Ingredient.Parse("1 c. Water");
			bool wasChanged_cups = ingredientUnitStandardizer.StandardizeUnit(ingredient_cups);
			Assert.IsTrue(wasChanged_cups);
			Assert.AreEqual("cup", ingredient_cups.Unit);
			Assert.AreEqual("cup Water", ingredient_cups.Name);

			var ingredient_cups2 = Ingredient.Parse("1/2 c. Water");
			bool wasChanged_cups2 = ingredientUnitStandardizer.StandardizeUnit(ingredient_cups2);
			Assert.IsTrue(wasChanged_cups2);
			Assert.AreEqual("cup", ingredient_cups2.Unit);
			Assert.AreEqual("cup Water", ingredient_cups2.Name);

			var ingredient_cups3 = Ingredient.Parse(".5 c. Water");
			bool wasChanged_cups3 = ingredientUnitStandardizer.StandardizeUnit(ingredient_cups3);
			Assert.IsTrue(wasChanged_cups3);
			Assert.AreEqual("cups", ingredient_cups3.Unit);
			Assert.AreEqual("cups Water", ingredient_cups3.Name);

			var ingredient_cups4 = Ingredient.Parse("2 c. Water");
			bool wasChanged_cups4 = ingredientUnitStandardizer.StandardizeUnit(ingredient_cups4);
			Assert.IsTrue(wasChanged_cups4);
			Assert.AreEqual("cups", ingredient_cups4.Unit);
			Assert.AreEqual("cups Water", ingredient_cups4.Name);

			var ingredient_cups5 = Ingredient.Parse("1 1/2 c. Water");
			bool wasChanged_cups5 = ingredientUnitStandardizer.StandardizeUnit(ingredient_cups5);
			Assert.IsTrue(wasChanged_cups5);
			Assert.AreEqual("cup", ingredient_cups5.Unit);
			Assert.AreEqual("cup Water", ingredient_cups5.Name);
		}

		[TestMethod]
		public void IngredientUnitOptimizationTests()
		{
			var ingredient = Ingredient.Parse("8 tblsp Water");
		}
	}
}
