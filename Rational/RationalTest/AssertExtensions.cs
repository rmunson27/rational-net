using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemTest.Core.Numerics;

/// <summary>
/// Internal assertion extensions.
/// </summary>
internal static class AssertExtensions
{
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
        int expectedNumerator, int expectedDenominator,
        Ratio32 actualRatio,
        string message = "")
    {
        Assert.AreEqual(expectedNumerator, actualRatio.Numerator, message);
        Assert.AreEqual(expectedDenominator, actualRatio.Denominator, message);
    }
}
