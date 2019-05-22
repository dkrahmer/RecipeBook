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
			Assert.AreEqual("cups", ingredient.UnitName);
			Assert.AreEqual("cups Water", ingredient.IngredientName);

			ingredient = Ingredient.Parse("Water");
			Assert.AreEqual("", ingredient.Amount.ToString());
			Assert.AreEqual("Water", ingredient.UnitName);
			Assert.AreEqual("Water", ingredient.IngredientName);

			ingredient = Ingredient.Parse("cups of Water");
			Assert.AreEqual("", ingredient.Amount.ToString());
			Assert.AreEqual("cups", ingredient.UnitName);
			Assert.AreEqual("cups of Water", ingredient.IngredientName);

			ingredient = Ingredient.Parse("1");
			Assert.AreEqual("1", ingredient.Amount.ToString());
			Assert.AreEqual("", ingredient.UnitName);
			Assert.AreEqual("", ingredient.IngredientName);

			ingredient = Ingredient.Parse("2/3 cups Water");
			Assert.AreEqual("2/3", ingredient.Amount.ToString());
			Assert.AreEqual("cups", ingredient.UnitName);
			Assert.AreEqual("cups Water", ingredient.IngredientName);

			ingredient = Ingredient.Parse("0002 cups Water");
			Assert.AreEqual("2", ingredient.Amount.ToString());
			Assert.AreEqual("cups", ingredient.UnitName);
			Assert.AreEqual("cups Water", ingredient.IngredientName);

			ingredient = Ingredient.Parse("2.3333333333 cups Water");
			Assert.AreEqual("2 1/3", ingredient.Amount.ToString());
			Assert.AreEqual("cups", ingredient.UnitName);
			Assert.AreEqual("cups Water", ingredient.IngredientName);

			ingredient = Ingredient.Parse("0001.500 cups Water");
			Assert.AreEqual("1.5", ingredient.Amount.ToString());
			Assert.AreEqual("cups", ingredient.UnitName);
			Assert.AreEqual("cups Water", ingredient.IngredientName);

			ingredient = Ingredient.Parse("1. cup Water");
			Assert.AreEqual("1", ingredient.Amount.ToString());
			Assert.AreEqual("cup", ingredient.UnitName);
			Assert.AreEqual("cup Water", ingredient.IngredientName);

			ingredient = Ingredient.Parse(".5 cups Water");
			Assert.AreEqual("0.5", ingredient.Amount.ToString());
			Assert.AreEqual("cups", ingredient.UnitName);
			Assert.AreEqual("cups Water", ingredient.IngredientName);

			ingredient = Ingredient.Parse("  .6666666 cups Water  ");
			Assert.AreEqual("2/3", ingredient.Amount.ToString());
			Assert.AreEqual("cups", ingredient.UnitName);
			Assert.AreEqual("cups Water", ingredient.IngredientName);
		}
	}
}
