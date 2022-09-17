using Rem.Core.Numerics.Digits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RemTest.Core.Numerics;

/// <summary>
/// Internal assertion extensions.
/// </summary>
internal static class AssertExtensions
{
    #region Equality
    #region Ratio
    /// <inheritdoc cref="RatioEquals(Assert, BigInteger, BigInteger, BigRatio, string)"/>
    public static void RatioEquals(
        this Assert _,
        int expectedNumerator, int expectedDenominator,
        Ratio32 actualRatio,
        string message = "")
    {
        Assert.AreEqual(expectedNumerator, actualRatio.Numerator, message);
        Assert.AreEqual(expectedDenominator, actualRatio.Denominator, message);
    }

    /// <summary>
    /// Asserts that the ratio passed in matches the expected components.
    /// </summary>
    /// <remarks>
    /// This is useful when testing the creation methods that make ratio instances, and for ensuring that a ratio
    /// creation reduces its components as expected.
    /// </remarks>
    /// <param name="_"></param>
    /// <param name="expectedNumerator"></param>
    /// <param name="expectedDenominator"></param>
    /// <param name="actualRatio"></param>
    /// <param name="message"></param>
    public static void RatioEquals(
        this Assert _,
        BigInteger expectedNumerator, BigInteger expectedDenominator,
        BigRatio actualRatio,
        string message = "")
    {
        Assert.AreEqual(expectedNumerator, actualRatio.Numerator, message);
        Assert.AreEqual(expectedDenominator, actualRatio.Denominator, message);
    }
    #endregion

    #region RatioDigitRep
    /// <summary>
    /// Asserts that the properties of the <see cref="RatioDigitRep"/> passed in matches the expected values passed in.
    /// </summary>
    /// <param name="_"></param>
    /// <param name="expectedRep"></param>
    /// <param name="actualRep"></param>
    /// <param name="message"></param>
    public static void RatioDigitRepEquals(
        this Assert _,
        ExpectedRatioDigitRep expectedRep,
        RatioDigitRep actualRep,
        string message = "")
    {
        Assert.AreEqual(expectedRep.IsNegative, actualRep.IsNegative, message);
        Assert.AreEqual(expectedRep.Whole, actualRep.Whole, message);
        Assert.AreEqual(expectedRep.Terminating, actualRep.Terminating, message);
        Assert.AreEqual(expectedRep.Repeating, actualRep.Repeating, message);
    }
    #endregion
    #endregion
}
