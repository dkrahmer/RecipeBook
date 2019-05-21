using Common.Models;
using Fractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rationals;
using Fractional;

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
			//Assert.AreEqual("4 2/3", ingredient.Amount.ToString());

			ingredient = Ingredient.Parse("2/3 cups Water");

			ingredient = Ingredient.Parse("2 cups Water");

			ingredient = Ingredient.Parse("2.3333333333 cups Water");

			ingredient = Ingredient.Parse("1.5 cups Water");

			ingredient = Ingredient.Parse("1. cups Water");

			ingredient = Ingredient.Parse(".5 cups Water");

			ingredient = Ingredient.Parse(".6666666 cups Water");
		}

		[TestMethod]
		public void RationalsTest()
		{
			var fraction = Rational.Parse("4 + 2/3");
			fraction = fraction * Rational.Parse("1.55545454");
			string str;
			str = fraction.ToString("F");
			str = fraction.ToString("C");
			str = fraction.ToString("W");
		}

		[TestMethod]
		public void FractionsTest()
		{
			//var fraction = new Fraction("4 2/3");
			bool success = Fraction.TryParse("4 1/3", out Fraction fraction);
			string str;
			str = fraction.ToString();
			str = fraction.ToString();
			str = fraction.ToString();
		}

		[TestMethod]
		public void FractionalTest()
		{
			var fraction = new Fractional.Fractional("4 2/3");
			fraction = fraction * 1.6;
			fraction = new Fractional.Fractional("4.88888");
			//bool success = Fractional.TryParse("4 1/3", out Fraction fraction);
			string str;
			str = fraction.ToString();
			str = fraction.ToString();
			str = fraction.ToString();
		}
	}
}
