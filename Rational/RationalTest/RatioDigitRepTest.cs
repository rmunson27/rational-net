using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemTest.Core.Numerics;

/// <summary>
/// Tests of the <see cref="RatioDigitRep"/> class.
/// </summary>
[TestClass]
public class RatioDigitRepTest
{
    /// <summary>
    /// Tests the non-override <see cref="RatioDigitRep.ToString"/> overloads.
    /// </summary>
    [TestMethod]
    public void TestToString()
    {
        Assert.AreEqual("0", BigRatio.Zero.RepresentInBase(10).ToString());
        Assert.AreEqual("1", BigRatio.One.RepresentInBase(10).ToString());
        Assert.AreEqual("1 0 (Base 10)", BigRatio.Create(10).RepresentInBase(10).ToString());
        Assert.AreEqual("-4", BigRatio.Create(-4).RepresentInBase(10).ToString());
        Assert.AreEqual("-3 0 (Base 10)", BigRatio.Create(-30).RepresentInBase(10).ToString());
        Assert.AreEqual("2 0 . 5 (Base 10)", BigRatio.Create(41, 2).RepresentInBase(10).ToString());
        Assert.AreEqual("-0 . 5 (Base 10)", BigRatio.Create(1, -2).RepresentInBase(10).ToString());
        Assert.AreEqual("4 4 . [ 3 ] (Base 10)", BigRatio.Create(133, 3).RepresentInBase(10).ToString());
        Assert.AreEqual("1 0 . 3 [ 2 8 5 7 1 4 ] (Base 10)", BigRatio.Create(723, 70).RepresentInBase(10).ToString());
    }
}
