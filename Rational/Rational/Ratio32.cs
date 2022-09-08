using Rem.Core.Attributes;
using Rem.Core.Numerics.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Numerics;

using TSuperInt = Int64;
using TInt = Int32;

/// <summary>
/// Represents a ratio of two <see cref="TInt"/> values.
/// </summary>
/// <remarks>
/// All instances of this type are fully simplified.
/// <para/>
/// The denominators of this type are always positive after construction (as the sign is propagated through the
/// numerator), so it should be noted that an attempt to construct an irreducible instance of the type with
/// <see cref="TInt.MinValue"/> as the denominator will result in an <see cref="OverflowException"/> that cannot be
/// avoided with an <see langword="unchecked"/> context.
/// </remarks>
public readonly record struct Ratio32 : IComparable<Ratio32>, IComparable<TInt>
{
    #region Constants
    /// <summary>
    /// The <see cref="Ratio32"/> representing negative one.
    /// </summary>
    public static readonly Ratio32 NegativeOne = Create(-1);

    /// <summary>
    /// The <see cref="Ratio32"/> representing zero.
    /// </summary>
    public static readonly Ratio32 Zero = default;

    /// <summary>
    /// The <see cref="Ratio32"/> representing one.
    /// </summary>
    public static readonly Ratio32 One = Create(1);
    #endregion

    #region Properties And Fields
    /// <summary>
    /// Gets the sign of this instance.
    /// </summary>
    public TInt Sign => Math.Sign(Numerator);

    /// <summary>
    /// Gets whether or not this instance is a whole number.
    /// </summary>
    public bool IsWhole => Denominator == 1;

    /// <summary>
    /// Gets the numerator of this instance.
    /// </summary>
    public TInt Numerator { get; }

    /// <summary>
    /// Gets the denominator of this instance.
    /// </summary>
    [Positive] public TInt Denominator => _denominator == 0 ? 1 : _denominator;
    [NonNegative] private readonly TInt _denominator;

    /// <summary>
    /// Gets the reciprocal (multiplicative inverse) of this instance.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="DivideByZeroException">This instance is zero.</exception>
    public Ratio32 Reciprocal
    {
        get
        {
            if (Numerator == 0) throw new DivideByZeroException("Attempted to find the reciprocal of zero.");

            var newDenominator = Numerator;
            var newNumerator = _denominator;
            MakeDenominatorNonNegative(ref newNumerator, ref newDenominator);
            return new(newNumerator, newDenominator);
        }
    }
    #endregion

    #region Constructors
    private Ratio32(TInt Numerator, [NonNegative] TInt Denominator)
    {
        this.Numerator = Numerator;
        _denominator = Denominator;
    }
    #endregion

    #region Methods
    #region Factory
    /// <summary>
    /// Creates a new <see cref="Ratio32"/> with the given numerator and denominator.
    /// </summary>
    /// <param name="Numerator"></param>
    /// <param name="Denominator"></param>
    /// <returns></returns>
    /// <exception cref="ZeroDenominatorException"><paramref name="Denominator"/> was zero.</exception>
    public static Ratio32 Create(TInt Numerator, [NonZero] TInt Denominator)
    {
        if (Denominator == 0) throw new ZeroDenominatorException();
        else if (Numerator == 0) return default; // 0

        Reduce(ref Numerator, ref Denominator);
        MakeDenominatorNonNegative(ref Numerator, ref Denominator);
        return new(Numerator, Denominator);
    }

    /// <summary>
    /// Creates a new <see cref="Ratio32"/> with the given numerator and a denominator of 1.
    /// </summary>
    /// <param name="Numerator"></param>
    /// <returns></returns>
    public static Ratio32 Create(TInt Numerator) => CreateInternalFormat(Numerator, 1);

    /// <summary>
    /// Creates a new <see cref="Ratio32"/> equivalent to 1 over the given denominator.
    /// </summary>
    /// <param name="Denominator"></param>
    /// <returns></returns>
    /// <exception cref="ZeroDenominatorException"><paramref name="Denominator"/> was zero.</exception>
    public static Ratio32 CreateOneOver([NonZero] TInt Denominator)
    {
        if (Denominator == 0) throw new ZeroDenominatorException();
        var Numerator = 1;
        MakeDenominatorNonNegative(ref Numerator, ref Denominator);
        return new(Numerator, Denominator);
    }

    /// <summary>
    /// Creates a new <see cref="Ratio32"/> in the internal format used by the library (with a numerator and
    /// denominator of 0 used for 0).
    /// </summary>
    /// <param name="Numerator"></param>
    /// <param name="Denominator"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Ratio32 CreateInternalFormat(TInt Numerator, [NonNegative] TInt Denominator)
        => new(Numerator, Numerator == 0 ? 0 : Denominator);
    #endregion

    #region Equality
    /// <summary>
    /// Determines if this instance is equal to another object of the same type.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Ratio32 other) => Numerator == other.Numerator && _denominator == other._denominator;

    /// <summary>
    /// Gets a hash code for this instance.
    /// </summary>
    /// <returns></returns>
    public override TInt GetHashCode() => HashCode.Combine(Numerator, _denominator);
    #endregion

    #region Arithmetic
    /// <summary>
    /// Computes the additive inverse of the ratio passed in.
    /// </summary>
    /// <param name="r"></param>
    /// <returns></returns>
    public static Ratio32 operator -(Ratio32 r) => new(-r.Numerator, r._denominator);
    #endregion

    #region Comparison
    #region Self
    /// <summary>
    /// Determines if <paramref name="lhs"/> is greater than <paramref name="rhs"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator >(Ratio32 lhs, Ratio32 rhs) => lhs.CompareTo(rhs) > 0;

    /// <summary>
    /// Determines if <paramref name="lhs"/> is less than <paramref name="rhs"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator <(Ratio32 lhs, Ratio32 rhs) => lhs.CompareTo(rhs) < 0;

    /// <summary>
    /// Determines if <paramref name="lhs"/> is greater than or equal to <paramref name="rhs"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator >=(Ratio32 lhs, Ratio32 rhs) => lhs.CompareTo(rhs) >= 0;

    /// <summary>
    /// Determines if <paramref name="lhs"/> is less than or equal to <paramref name="rhs"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator <=(Ratio32 lhs, Ratio32 rhs) => lhs.CompareTo(rhs) <= 0;

    /// <inheritdoc/>
    public int CompareTo(Ratio32 other)
    {
        TSuperInt thisNumerator = Numerator, thisDenominator = _denominator,
                  otherNumerator = other.Numerator, otherDenominator = other._denominator;
        return (thisNumerator * otherDenominator).CompareTo(otherNumerator * thisDenominator);
    }
    #endregion

    #region TInt
    /// <summary>
    /// Determines if <paramref name="lhs"/> is greater than <paramref name="rhs"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator >(Ratio32 lhs, TInt rhs) => lhs.CompareTo(rhs) > 0;

    /// <summary>
    /// Determines if <paramref name="lhs"/> is less than <paramref name="rhs"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator <(Ratio32 lhs, TInt rhs) => lhs.CompareTo(rhs) < 0;

    /// <summary>
    /// Determines if <paramref name="lhs"/> is greater than or equal to <paramref name="rhs"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator >=(Ratio32 lhs, TInt rhs) => lhs.CompareTo(rhs) >= 0;

    /// <summary>
    /// Determines if <paramref name="lhs"/> is less than or equal to <paramref name="rhs"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator <=(Ratio32 lhs, TInt rhs) => lhs.CompareTo(rhs) <= 0;

    /// <summary>
    /// Determines if <paramref name="lhs"/> is greater than <paramref name="rhs"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator >(TInt lhs, Ratio32 rhs) => rhs.CompareTo(lhs) < 0;

    /// <summary>
    /// Determines if <paramref name="lhs"/> is less than <paramref name="rhs"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator <(TInt lhs, Ratio32 rhs) => rhs.CompareTo(lhs) > 0;

    /// <summary>
    /// Determines if <paramref name="lhs"/> is greater than or equal to <paramref name="rhs"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator >=(TInt lhs, Ratio32 rhs) => rhs.CompareTo(lhs) <= 0;

    /// <summary>
    /// Determines if <paramref name="lhs"/> is less than or equal to <paramref name="rhs"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool operator <=(TInt lhs, Ratio32 rhs) => rhs.CompareTo(lhs) >= 0;

    /// <inheritdoc/>
    public int CompareTo(TInt other)
    {
        TSuperInt thisNumerator = Numerator, thisDenominator = _denominator,
                  otherNumerator = other;
        return thisNumerator.CompareTo(otherNumerator * thisDenominator);
    }
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
    /// <param name="numerator"></param>
    public static implicit operator Ratio32(int numerator) => Create(numerator);
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
        var gcd = Maths.GCD(Numerator, Denominator);
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
        if (Denominator < 0)
        {
            checked // Need to make sure the denominator doesn't overflow if it is negative
            {
                Numerator = -Numerator;
                Denominator = -Denominator;
            }
        }
    }
    #endregion
    #endregion
}
