using Common.Structs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommonTests
{
	[TestClass]
	public class AmountTests
	{
		[TestMethod]
		public void AmountParseTest()
		{
			Amount amount;

			amount = Amount.Parse("4 2/3");
			Assert.AreEqual("4 2/3", amount.ToString());

			amount = Amount.Parse("2/3");
			Assert.AreEqual("2/3", amount.ToString());

			amount = Amount.Parse("4/6");
			Assert.AreEqual("2/3", amount.ToString());

			amount = Amount.Parse("1.");
			Assert.AreEqual("1", amount.ToString());

			amount = Amount.Parse("2.0000");
			Assert.AreEqual("2", amount.ToString());

			amount = Amount.Parse("2.3333333333");
			Assert.AreEqual("2 1/3", amount.ToString());

			amount = Amount.Parse("1.5");
			Assert.AreEqual("1.5", amount.ToString());
			Assert.AreEqual("1 1/2", amount.ToFractionAmount().ToString());

			amount = Amount.Parse(".5");
			Assert.AreEqual("0.5", amount.ToString());
			Assert.AreEqual("1/2", amount.ToFractionAmount().ToString());

			amount = Amount.Parse("000.6666666000");
			Assert.AreEqual("2/3", amount.ToString());

			amount = Amount.Parse("3/4").ToDecimalAmount();
			Assert.AreEqual("0.75", amount.ToString());

			amount = Amount.Parse("1/9") * Amount.Parse(".6666666");
			Assert.AreEqual("2/27", amount.ToString());

			amount = Amount.Parse("1/9") + Amount.Parse(".6666666");
			Assert.AreEqual("7/9", amount.ToString());

			amount = Amount.Parse("1/9") / Amount.Parse(".6666666");
			Assert.AreEqual("1/6", amount.ToString());

			amount = Amount.Parse(".6666666") - Amount.Parse("1/9");
			Assert.AreEqual("5/9", amount.ToString());
		}
	}
}
