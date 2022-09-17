using Rem.Core.Attributes;
using Rem.Core.Numerics.Digits;
using Rem.Core.Numerics.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Numerics;

using TInt = BigInteger;

/// <summary>
/// Represents a ratio of two <see cref="TInt"/> values.
/// </summary>
/// <remarks>
/// All instances of this type are fully simplified.
/// </remarks>
public readonly record struct BigRatio : IComparable<BigRatio>, IComparable<TInt>
{
    #region Constants
    #region Fundamental
    /// <summary>
    /// The <see cref="BigRatio"/> representing negative one.
    /// </summary>
    public static readonly BigRatio NegativeOne = Create(TInt.MinusOne);

    /// <summary>
    /// The <see cref="BigRatio"/> representing zero.
    /// </summary>
    public static readonly BigRatio Zero = default;

    /// <summary>
    /// The <see cref="BigRatio"/> representing one.
    /// </summary>
    public static readonly BigRatio One = Create(TInt.One);
    #endregion

    #region IEEE-754
    /// <summary>
    /// The (finite) <see cref="BigRatio"/> representing the numerical interpretation of the IEEE-754
    /// <see cref="double.PositiveInfinity"/> value.
    /// </summary>
    /// <remarks>
    /// This value is the result of calling the <see cref="FromExactDouble(double)"/> method on
    /// <see cref="double.PositiveInfinity"/>.
    /// </remarks>
    public static readonly BigRatio DoublePositiveInfinity = FromExactDouble(double.PositiveInfinity);

    /// <summary>
    /// The (finite) <see cref="BigRatio"/> representing the numerical interpretation of the IEEE-754
    /// <see cref="double.NegativeInfinity"/> value.
    /// </summary>
    /// <remarks>
    /// This value is the result of calling the <see cref="FromExactDouble(double)"/> method on
    /// <see cref="double.NegativeInfinity"/>.
    /// </remarks>
    public static readonly BigRatio DoubleNegativeInfinity = FromExactDouble(double.NegativeInfinity);

    /// <summary>
    /// The <see cref="BigRatio"/> exactly representing <see cref="double.MaxValue"/>.
    /// </summary>
    public static readonly BigRatio DoubleMaxValue = FromExactDouble(double.MaxValue);

    /// <summary>
    /// The <see cref="BigRatio"/> exactly representing <see cref="double.MinValue"/>.
    /// </summary>
    public static readonly BigRatio DoubleMinValue = FromExactDouble(double.MinValue);
    #endregion
    #endregion

    #region Properties And Fields
    /// <summary>
    /// Gets the sign of this instance.
    /// </summary>
    public TInt Sign => Numerator.Sign;

    /// <summary>
    /// Gets whether or not this instance is a whole number.
    /// </summary>
    public bool IsWhole => Denominator.IsOne;

    /// <summary>
    /// Gets the numerator of this instance.
    /// </summary>
    public TInt Numerator { get; }

    /// <summary>
    /// Gets the denominator of this instance.
    /// </summary>
    [Positive] public TInt Denominator => _denominator.IsZero ? TInt.One : _denominator;
    [NonNegative] private readonly TInt _denominator;

    /// <summary>
    /// Gets the reciprocal (multiplicative inverse) of this instance.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="DivideByZeroException">This instance is zero.</exception>
    public BigRatio Reciprocal
    {
        get
        {
            if (Numerator.IsZero) throw new DivideByZeroException("Attempted to find the reciprocal of zero.");

            var newDenominator = Numerator;
            var newNumerator = _denominator;
            MakeDenominatorNonNegative(ref newNumerator, ref newDenominator);
            return new(newNumerator, newDenominator);
        }
    }
    #endregion

    #region Constructors
    private BigRatio(TInt Numerator, [NonNegative] TInt Denominator)
    {
        this.Numerator = Numerator;
        _denominator = Denominator;
    }
    #endregion

    #region Methods
    #region Factory
    #region Create
    /// <summary>
    /// Creates a new <see cref="BigRatio"/> with the given numerator and denominator.
    /// </summary>
    /// <param name="Numerator"></param>
    /// <param name="Denominator"></param>
    /// <returns></returns>
    /// <exception cref="ZeroDenominatorException"><paramref name="Denominator"/> was zero.</exception>
    public static BigRatio Create(TInt Numerator, [NonZero] TInt Denominator)
    {
        if (Denominator.IsZero) throw new ZeroDenominatorException();
        else if (Numerator.IsZero) return default; // 0

        Reduce(ref Numerator, ref Denominator);
        MakeDenominatorNonNegative(ref Numerator, ref Denominator);
        return new(Numerator, Denominator);
    }

    /// <summary>
    /// Creates a new <see cref="BigRatio"/> with the given numerator and a denominator of 1.
    /// </summary>
    /// <param name="Numerator"></param>
    /// <returns></returns>
    public static BigRatio Create(TInt Numerator) => CreateInternalFormat(Numerator, TInt.One);

    /// <summary>
    /// Creates a new <see cref="BigRatio"/> equivalent to 1 over the given denominator.
    /// </summary>
    /// <param name="Denominator"></param>
    /// <returns></returns>
    /// <exception cref="ZeroDenominatorException"><paramref name="Denominator"/> was zero.</exception>
    public static BigRatio CreateOneOver([NonZero] TInt Denominator)
    {
        if (Denominator.IsZero) throw new ZeroDenominatorException();
        var Numerator = TInt.One;
        MakeDenominatorNonNegative(ref Numerator, ref Denominator);
        return new(Numerator, Denominator);
    }

    /// <summary>
    /// Creates a new <see cref="BigRatio"/> in the internal format used by the library (with a numerator and
    /// denominator of 0 used for 0).
    /// </summary>
    /// <param name="Numerator"></param>
    /// <param name="Denominator"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static BigRatio CreateInternalFormat(TInt Numerator, [NonNegative] TInt Denominator)
        => new(Numerator, Numerator.IsZero ? TInt.Zero : Denominator);
    #endregion

    #region Float Conversions
    /// <summary>
    /// Creates a <see cref="BigRatio"/> from the exact <see cref="double"/> value passed in.
    /// </summary>
    /// <remarks>
    /// This method will return a <see cref="BigRatio"/> that is <i>exactly</i> equivalent to the value passed in,
    /// even if the value has a shorter round-trip representation.  For example, while the method will return 1/4
    /// when passed the value 0.25 since the value 0.25 has an exact representation in the <see cref="double"/> type,
    /// passing in the value 0.1 will cause the method to return a ratio equal to the decimal
    /// value 0.1000000000000000055511151231257827021181583404541015625.
    /// 
    /// <para/>
    /// 
    /// When passed an infinite value, this method will return the value numerically represented by the IEEE-754
    /// double-precision representation of the value.
    /// </remarks>
    /// <param name="d"></param>
    /// <returns></returns>
    /// <exception cref="ArithmeticException"><paramref name="d"/> was a not-a-number (NaN) value.</exception>
    public static BigRatio FromExactDouble(double d)
    {
        if (double.IsNaN(d))
        {
            throw new ArithmeticException("Floating point not-a-number (NaN) values are not permitted.");
        }

        // Split the components of the double value into normalized sign, exponent and mantissa
        // Infinite values will be treated like finite values - converting back will yield the same infinity that was
        // passed in
        new DoubleComponents(d).TryGetNormalizedLogical(out var sign, out var exp, out var mantissa);

        // Combine the mantissa and sign by treating the mantissa as a long (cannot overflow since is at most 53 bits)
        var longMantissa = unchecked((long)mantissa);
        if (sign < 0) longMantissa = -longMantissa;

        return exp switch
        {
            < 0 => Create(longMantissa, TInt.One << (-exp)),
            0 => Create(longMantissa),
            > 0 => Create(longMantissa * TInt.One << exp),
        };
    }
    #endregion
    #endregion

    #region Equality
    /// <summary>
    /// Determines if this instance is equal to another object of the same type.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(BigRatio other) => Numerator == other.Numerator && _denominator == other._denominator;

    /// <summary>
    /// Gets a hash code for this instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => HashCode.Combine(Numerator, _denominator);
    #endregion

    #region Computation
    #region Arithmetic
    /// <summary>
    /// Computes the additive inverse of the ratio passed in.
    /// </summary>
    /// <param name="r"></param>
    /// <returns></returns>
    public static BigRatio operator -(in BigRatio r) => new(-r.Numerator, r._denominator);
    #endregion

    #region Representation
    /// <summary>
    /// Gets a representation for the ratio in the given base.
    /// </summary>
    /// <param name="Base">The base to compute the representation in.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="Base"/> was less than 2.</exception>
    public RatioDigitRep RepresentInBase([GreaterThanOrEqualToInteger(2)] TInt Base)
        => RepresentInBaseInternal(Numerator, _denominator, Base);

    internal static RatioDigitRep RepresentInBaseInternal(
        TInt Numerator,
        TInt Denominator,
        [GreaterThanOrEqualToInteger(2)] TInt Base)
    {
        Throw.IfArgLessThan(2, Base, nameof(Base));

        // Avoid having to do long division if we have a whole number
        if (Denominator.IsZero || Denominator.IsOne)
        {
            var numeratorRep = DigitReps.InBase(Numerator, Base);
            return new(
                IsNegative: Numerator.Sign < 0,
                Base,
                numeratorRep.Digits, DigitList.EmptyFromBaseSize(Base), null);
        }

        // Try to avoid storing big integers as digits and remainders if possible, since there could be as many
        // entries as the denominator is large in the worst case
        var digitListBuilder = DigitList.Builder.NewFromBaseSize(Base);
        var remainderMap = RemainderMap.NewFromDenominatorSize(Denominator);

        bool isNegative = Numerator.Sign < 0;
        var posNumerator = isNegative ? -Numerator : Numerator;

        var whole = TInt.DivRem(posNumerator, Denominator, out var remainder);
        var wholeRep = DigitReps.InBase(whole, Base).Digits;

        int fractionalIndex = 0;
        remainderMap.AddIfNotExists(remainder, fractionalIndex, out _); // Cannot fail

        while (true) // Will run until return condition is encountered
        {
            fractionalIndex++;
            remainder *= Base;
            var digit = TInt.DivRem(remainder, Denominator, out remainder);
            digitListBuilder.Add(digit);

            if (remainder.IsZero) // Reached the end of a terminating expansion
            {
                return new(isNegative, Base, wholeRep, digitListBuilder.ToList(), Repeating: null);
            }

            else if (!remainderMap.AddIfNotExists(remainder, fractionalIndex, out var repeatStartIndex))
            {
                // Reached the end of a repeat
                // Split the digits into terminating and repeating portion
                var digitsSplit = digitListBuilder.ToList().SplitAtIndices(repeatStartIndex);
                DigitList terminating = digitsSplit[0], repeating = digitsSplit[1];

                return new(isNegative, Base, wholeRep, terminating, repeating);
            }
        }
    }
    #endregion
    #endregion

    #region Comparison
    #region Self
    /// <summary>
    /// Determines if <paramref name="lhs"/> is greater than <paramref name="rhs"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator >(in BigRatio lhs, in BigRatio rhs) => lhs.CompareTo(rhs) > 0;

    /// <summary>
    /// Determines if <paramref name="lhs"/> is less than <paramref name="rhs"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator <(in BigRatio lhs, in BigRatio rhs) => lhs.CompareTo(rhs) < 0;

    /// <summary>
    /// Determines if <paramref name="lhs"/> is greater than or equal to <paramref name="rhs"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator >=(in BigRatio lhs, in BigRatio rhs) => lhs.CompareTo(rhs) >= 0;

    /// <summary>
    /// Determines if <paramref name="lhs"/> is less than or equal to <paramref name="rhs"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator <=(in BigRatio lhs, in BigRatio rhs) => lhs.CompareTo(rhs) <= 0;

    /// <inheritdoc/>
    public int CompareTo(BigRatio other) => (Numerator * other._denominator).CompareTo(other.Numerator * _denominator);
    #endregion

    #region TInt
    /// <summary>
    /// Determines if <paramref name="lhs"/> is greater than <paramref name="rhs"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator >(in BigRatio lhs, TInt rhs) => lhs.CompareTo(rhs) > 0;

    /// <summary>
    /// Determines if <paramref name="lhs"/> is less than <paramref name="rhs"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator <(in BigRatio lhs, TInt rhs) => lhs.CompareTo(rhs) < 0;

    /// <summary>
    /// Determines if <paramref name="lhs"/> is greater than or equal to <paramref name="rhs"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator >=(in BigRatio lhs, TInt rhs) => lhs.CompareTo(rhs) >= 0;

    /// <summary>
    /// Determines if <paramref name="lhs"/> is less than or equal to <paramref name="rhs"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator <=(in BigRatio lhs, TInt rhs) => lhs.CompareTo(rhs) <= 0;

    /// <summary>
    /// Determines if <paramref name="lhs"/> is greater than <paramref name="rhs"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator >(TInt lhs, in BigRatio rhs) => rhs.CompareTo(lhs) < 0;

    /// <summary>
    /// Determines if <paramref name="lhs"/> is less than <paramref name="rhs"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator <(TInt lhs, in BigRatio rhs) => rhs.CompareTo(lhs) > 0;

    /// <summary>
    /// Determines if <paramref name="lhs"/> is greater than or equal to <paramref name="rhs"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator >=(TInt lhs, in BigRatio rhs) => rhs.CompareTo(lhs) <= 0;

    /// <summary>
    /// Determines if <paramref name="lhs"/> is less than or equal to <paramref name="rhs"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator <=(TInt lhs, in BigRatio rhs) => rhs.CompareTo(lhs) >= 0;

    /// <inheritdoc/>
    public int CompareTo(TInt other) => Numerator.CompareTo(other * _denominator);
    #endregion
    #endregion

    #region Deconstruct
    /// <summary>
    /// Deconstructs this instance into a numerator and a denominator.
    /// </summary>
    /// <param name="Numerator"></param>
    /// <param name="Denominator"></param>
    public void Deconstruct(out TInt Numerator, [Positive] out TInt Denominator)
    {
        Numerator = this.Numerator;
        Denominator = this.Denominator;
    }
    #endregion

    #region Conversions
    /// <summary>
    /// Implicitly converts an integer to an equivalent ratio (with a denominator of 1).
    /// </summary>
    /// <param name="ratio"></param>
    public static implicit operator BigRatio(Ratio32 ratio) => new(ratio.Numerator, ratio._denominator);

    /// <summary>
    /// Implicitly converts an integer to an equivalent ratio (with a denominator of 1).
    /// </summary>
    /// <param name="numerator"></param>
    public static implicit operator BigRatio(TInt numerator) => Create(numerator);

    /// <summary>
    /// Implicitly converts an integer to an equivalent ratio (with a denominator of 1).
    /// </summary>
    /// <param name="numerator"></param>
    public static implicit operator BigRatio(long numerator) => Create(numerator);

    /// <summary>
    /// Implicitly converts an integer to an equivalent ratio (with a denominator of 1).
    /// </summary>
    /// <param name="numerator"></param>
    public static implicit operator BigRatio(ulong numerator) => Create(numerator);

    /// <summary>
    /// Implicitly converts an integer to an equivalent ratio (with a denominator of 1).
    /// </summary>
    /// <param name="numerator"></param>
    public static implicit operator BigRatio(int numerator) => Create(numerator);
    #endregion

    #region ToString
    /// <summary>
    /// Gets a string that represents the current instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"{Numerator} {RatioFormatting.ComponentSeparator} {Denominator}";
    #endregion

    #region Helpers
    private static void Reduce(ref TInt Numerator, ref TInt Denominator)
    {
        var gcd = TInt.Abs(TInt.GreatestCommonDivisor(Numerator, Denominator));
        Numerator /= gcd;
        Denominator /= gcd;
    }

    /// <summary>
    /// Simplifies the numerator and denominator so that the denominator is non-negative.
    /// </summary>
    /// <param name="Numerator"></param>
    /// <param name="Denominator"></param>
    /// <exception cref="OverflowException">
    /// The negation of the numerator or denominator overflowed (which can occur if either is
    /// <see cref="TInt.MinValue"/>).
    /// </exception>
    private static void MakeDenominatorNonNegative(ref TInt Numerator, ref TInt Denominator)
    {
        if (Denominator.Sign < 0)
        {
            Numerator = -Numerator;
            Denominator = -Denominator;
        }
    }
    #endregion
    #endregion
}
