using Rem.Core.Attributes;
using Rem.Core.Numerics.Digits;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Numerics;

/// <summary>
/// A digit representation of a ratio.
/// </summary>
public sealed record class RatioDigitRep
{
    #region Constants
    private const string DefaultSeparator = " ";
    private const string DefaultRadixPoint = ".";
    private const string DefaultDigitFormat = "D";
    private const string RepeatOpener = "[";
    private const string RepeatCloser = "]";
    #endregion

    #region Properties
    /// <summary>
    /// Gets the base of the representation.
    /// </summary>
    [GreaterThanOrEqualToInteger(2)] public BigInteger Base { get; }

    /// <summary>
    /// Gets whether or not this instance represents a negative value.
    /// </summary>
    public bool IsNegative { get; }

    /// <summary>
    /// Gets the whole number portion of the representation.
    /// </summary>
    public DigitList Whole { get; }

    /// <summary>
    /// Gets whether or not this representation has a fractional component.
    /// </summary>
    public bool HasFractional => Terminating.Count != 0 || Repeating is not null;

    /// <summary>
    /// Gets the terminating fractional portion of the representation (past the radix point).
    /// </summary>
    public DigitList Terminating { get; }

    /// <summary>
    /// Gets whether or not this representation has a repeat.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Repeating))] public bool HasRepeat => Repeating is not null;

    /// <summary>
    /// Gets the repeating fractional portion of the representation (past the radix point and the terminating
    /// fractional portion), or <see langword="null"/> if there is no repeat.
    /// </summary>
    public DigitList? Repeating { get; }
    #endregion

    #region Constructor
    /// <summary>
    /// Constructs a new instance of the <see cref="RatioDigitRep"/> class with the specified components.
    /// </summary>
    /// <param name="IsNegative"></param>
    /// <param name="Base"></param>
    /// <param name="Whole"></param>
    /// <param name="Terminating"></param>
    /// <param name="Repeating"></param>
    internal RatioDigitRep(
        bool IsNegative,
        [GreaterThanOrEqualToInteger(2)] BigInteger Base, DigitList Whole, DigitList Terminating, DigitList? Repeating)
    {
        this.IsNegative = IsNegative;
        this.Base = Base;
        this.Whole = Whole;
        this.Terminating = Terminating;
        this.Repeating = Repeating;
    }
    #endregion

    #region Equality
    /// <summary>
    /// Determines if this instance is equal to another object of the same type.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(RatioDigitRep? other) => other is not null
                                                    && IsNegative == other.IsNegative
                                                    && Base == other.Base
                                                    && Whole == other.Whole
                                                    && Terminating == other.Terminating
                                                    && Repeating == other.Repeating;

    /// <summary>
    /// Gets a hash code for the current instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => HashCode.Combine(IsNegative, Base, Whole, Terminating, Repeating);
    #endregion

    #region ToString
    /// <summary>
    /// Gets a string that represents the current instance.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => ToString(numberFormat: DefaultDigitFormat);

    /// <summary>
    /// Gets a string that represents the current instance with the digits and base formatted using the provided
    /// number format.
    /// </summary>
    /// <inheritdoc cref="ToString(string?, string?, string?, string?, string, IFormatProvider?)"/>
    public string ToString(
        string? numberFormat,
        string? digitSeparator = DefaultSeparator, string? componentSeparator = DefaultSeparator,
        string radixPoint = DefaultRadixPoint,
        IFormatProvider? numberFormatProvider = null)
        => ToString(
            digitSeparator: digitSeparator, componentSeparator: componentSeparator,
            radixPoint: radixPoint,
            digitFormat: numberFormat, baseFormat: numberFormat,
            numberFormatProvider: numberFormatProvider);

    /// <summary>
    /// Gets a string that represents the current instance with the digits and base formatted using the provided
    /// number formats.
    /// </summary>
    /// <param name="digitFormat">
    /// The format to use to format the digits.
    /// -or-
    /// A <see langword="null"/> reference to use the default format for the type of the digits.
    /// </param>
    /// <param name="baseFormat">
    /// The format to use to format the base.
    /// -or-
    /// A <see langword="null"/> reference to use the default format for the base.
    /// </param>
    /// <param name="digitSeparator">
    /// A separator to use to separate the digits when formatting.
    /// -or-
    /// a <see langword="null"/> reference to not include separators.
    /// </param>
    /// <param name="componentSeparator">
    /// A separator to use to separate the components of the representation and the repeat start and end characters
    /// when formatting.
    /// -or-
    /// a <see langword="null"/> reference to not include separators.
    /// </param>
    /// <param name="radixPoint">
    /// The string to use to represent the radix point.
    /// </param>
    /// <param name="numberFormatProvider">
    /// The provider to use to format the value.
    /// -or-
    /// a <see langword="null"/> reference to obtain the numeric format information from the current locale setting
    /// of the operating system.
    /// </param>
    /// <returns></returns>
    public string ToString(
        string? digitFormat, string? baseFormat,
        string? digitSeparator = DefaultSeparator,
        string? componentSeparator = DefaultSeparator,
        string radixPoint = DefaultRadixPoint,
        IFormatProvider? numberFormatProvider = null)
    {
        var needsBase = Whole.Count > 1 || HasFractional;
        var result = Whole.Count switch
        {
            0 => $"{formatNegSign()}0",
            1 => $"{formatNegSign()}{Whole[0].ToString(digitFormat, numberFormatProvider)}",
            _ => $"{formatNegSign()}"
                    + Whole.FormatAsList(separator: digitSeparator, digitFormat: digitFormat, numberFormatProvider),
        };

        if (HasFractional)
        {
            result += $"{componentSeparator}{radixPoint}";

            if (Terminating.Count > 0)
            {
                result += componentSeparator
                            + Terminating.FormatAsList(
                                separator: digitSeparator, digitFormat: digitFormat, numberFormatProvider);
            }

            if (HasRepeat)
            {
                result += $"{componentSeparator}{RepeatOpener}{componentSeparator}";
                result += Repeating.FormatAsList(separator: digitSeparator, digitFormat: digitFormat, numberFormatProvider);
                result += $"{componentSeparator}{RepeatCloser}";
            }
        }

        if (needsBase) result += $" (Base {Base.ToString(baseFormat, numberFormatProvider)})";

        return result;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        string formatNegSign() => IsNegative ? $"-" : string.Empty;
    }
    #endregion
}
