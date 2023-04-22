using Rem.Core.ComponentModel;
using Rem.Core.Numerics.Digits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RemTest.Core.Numerics;

/// <summary>
/// Internal assertion extensions.
/// </summary>
internal static class AssertExtensions
{
    #region Equality
    #region General
    /// <summary>
    /// Determines if the two values are equal.
    /// </summary>
    /// <param name="_"></param>
    /// <param name="expected"></param>
    /// <param name="actual"></param>
    /// <param name="comparer"></param>
    /// <param name="message"></param>
    /// <typeparam name="T"></typeparam>
    public static void AreEqual<T>(this Assert _,
                                   T expected, T actual, IEqualityComparer<T>? comparer, string message = "")
    {
        Assert.IsTrue(comparer.DefaultIfNull().Equals(expected, actual),
                      CombineMessages(
                        $"Failed asserting that expected {expected} matches actual {actual}.",
                        string.IsNullOrWhiteSpace(message) ? "" : $" {message}"));
    }
    #endregion

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

    #region Try
    /// <summary>
    /// Asserts that the given operation succeeds and returns the expected value in an <see langword="out"/> parameter.
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="expectedSuccess"></param>
    /// <param name="message"></param>
    /// <typeparam name="TSuccess"></typeparam>
    public static void Succeeds<TSuccess>(this Assert _, TryFunc<TSuccess> operation, TSuccess expectedSuccess,
                                          string message = "")
    {
        Assert.IsTrue(operation.Invoke(out var actualSuccess), CombineMessages("Operation failed.", message));
        Assert.That.AreEqual(expectedSuccess, actualSuccess, EqualityComparer<TSuccess>.Default,
                             CombineMessages("Operation values were not equal.", message));
    }
    #endregion

    #region Helpers
    /// <summary>
    /// Combines two error messages.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string CombineMessages(string first, string? next)
    {
        if (string.IsNullOrWhiteSpace(next)) return first;
        else return $"{first} {next}";
    }
    #endregion
}
