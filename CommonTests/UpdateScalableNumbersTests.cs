using Common;
using Common.Structs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommonTests
{
	[TestClass]
	public class UpdateScalableNumbersTests
	{
		[TestMethod]
		public void UpdateScalableNumbersTest()
		{
			// Basic functionality with automatic decimal/fraction
			Assert.AreEqual("Makes 1 crust.", Helpers.UpdateScalableNumbers("Makes <1> crust.", new Amount(1m)));
			Assert.AreEqual("Makes 2 crusts.", Helpers.UpdateScalableNumbers("Makes <2> crusts.", new Amount(1m)));
			Assert.AreEqual("Makes 4 crusts.", Helpers.UpdateScalableNumbers("Makes <2> crusts.", new Amount(2m)));
			Assert.AreEqual("Makes 1 crusts.", Helpers.UpdateScalableNumbers("Makes <2> crusts.", new Amount("1/2")));
			Assert.AreEqual("Makes 1 crusts.", Helpers.UpdateScalableNumbers("Makes <2> crusts.", new Amount(0.5m)));
			Assert.AreEqual("Makes 1/2 crusts.", Helpers.UpdateScalableNumbers("Makes <2> crusts.", new Amount("1/4")));
			Assert.AreEqual("Makes 0.5 crusts.", Helpers.UpdateScalableNumbers("Makes <2> crusts.", new Amount(0.25m)));
			Assert.AreEqual("Makes 0.25 crusts.", Helpers.UpdateScalableNumbers("Makes <2> crusts.", new Amount(0.125m)));

			// Force decimal
			Assert.AreEqual("Makes 0.5 crusts.", Helpers.UpdateScalableNumbers("Makes <2:.> crusts.", new Amount("1/4")));
			Assert.AreEqual("Makes 1 crusts.", Helpers.UpdateScalableNumbers("Makes <2:.0> crusts.", new Amount("1/4")));
			Assert.AreEqual("Makes 0.3 crusts.", Helpers.UpdateScalableNumbers("Makes <2:.1> crusts.", new Amount(0.125m)));
			Assert.AreEqual("Makes 0.13 crust.", Helpers.UpdateScalableNumbers("Makes <1:.2> crust.", new Amount(0.125m)));
			Assert.AreEqual("Makes 0.1 crust.", Helpers.UpdateScalableNumbers("Makes <1:.1> crust.", new Amount(0.125m)));
			Assert.AreEqual("Makes 0.125 crusts.", Helpers.UpdateScalableNumbers("Makes <1:.4> crusts.", new Amount(0.125m)));

			// Force fraction
			Assert.AreEqual("Makes 1/2 crusts.", Helpers.UpdateScalableNumbers("Makes <2:/> crusts.", new Amount("1/4")));
			Assert.AreEqual("Makes 1/2 crusts.", Helpers.UpdateScalableNumbers("Makes <2:/> crusts.", new Amount(0.25m)));
			Assert.AreEqual("Makes 1/4 crusts.", Helpers.UpdateScalableNumbers("Makes <2:/> crusts.", new Amount(0.125m)));

			// With fraction value input
			Assert.AreEqual("Makes 1/4 crusts.", Helpers.UpdateScalableNumbers("Makes <1/2> crusts.", new Amount(0.5m)));
			Assert.AreEqual("Makes 1/4 crusts.", Helpers.UpdateScalableNumbers("Makes <1/2> crusts.", new Amount("1/2")));
			Assert.AreEqual("Makes 1/4 crusts.", Helpers.UpdateScalableNumbers("Makes <1/2:/> crusts.", new Amount("1/2")));
			Assert.AreEqual("Makes 0.25 crusts.", Helpers.UpdateScalableNumbers("Makes <1/2:.> crusts.", new Amount("1/2")));
			Assert.AreEqual("Makes 1/4 crusts.", Helpers.UpdateScalableNumbers("Makes <1/2:/> crusts.", new Amount(0.5m)));
			Assert.AreEqual("Makes 0.25 crusts.", Helpers.UpdateScalableNumbers("Makes <1/2:.> crusts.", new Amount(0.5m)));
			Assert.AreEqual("Makes 0.3 crusts.", Helpers.UpdateScalableNumbers("Makes <1/2:.1> crusts.", new Amount(0.5m)));
			Assert.AreEqual("Makes 0 crusts.", Helpers.UpdateScalableNumbers("Makes <1/2:.0> crusts.", new Amount(0.5m)));

			// Multiple in a single string
			Assert.AreEqual("Makes 2 crust, enough for 4 people.", Helpers.UpdateScalableNumbers("Makes <1> crust, enough for <2:.0> people.", new Amount(2m)));
		}
	}
}
