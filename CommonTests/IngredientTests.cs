using Common.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
	}
}
