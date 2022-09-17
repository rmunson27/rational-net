using Rem.Core.Attributes;
using Rem.Core.Numerics.Digits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Numerics;

#region Concrete
/// <inheritdoc/>
internal sealed class ByteRemainderMap : RemainderMap<byte>
{
    /// <inheritdoc/>
    public override bool AddIfNotExists(
        byte rem, [NonNegative] int index, [NonNegative] out int existingIndex)
    {
        if (Map.TryGetValue(rem, out existingIndex)) return false;
        else
        {
            Map.Add(rem, index);
            return true;
        }
    }

    /// <inheritdoc/>
    public override bool AddIfNotExists(
        ushort rem, [NonNegative] int index, [NonNegative] out int existingIndex)
    {
        var digitRem = (byte)rem;

        if (Map.TryGetValue(digitRem, out existingIndex)) return false;
        else
        {
            Map.Add(digitRem, index);
            return true;
        }
    }

    /// <inheritdoc/>
    public override bool AddIfNotExists(
        uint rem, [NonNegative] int index, [NonNegative] out int existingIndex)
    {
        var digitRem = (byte)rem;

        if (Map.TryGetValue(digitRem, out existingIndex)) return false;
        else
        {
            Map.Add(digitRem, index);
            return true;
        }
    }

    /// <inheritdoc/>
    public override bool AddIfNotExists(
        ulong rem, [NonNegative] int index, [NonNegative] out int existingIndex)
    {
        var digitRem = (byte)rem;

        if (Map.TryGetValue(digitRem, out existingIndex)) return false;
        else
        {
            Map.Add(digitRem, index);
            return true;
        }
    }

    /// <inheritdoc/>
    public override bool AddIfNotExists(
        [NonNegative] BigInteger rem, [NonNegative] int index, [NonNegative] out int existingIndex)
    {
        var digitRem = (byte)rem;

        if (Map.TryGetValue(digitRem, out existingIndex)) return false;
        else
        {
            Map.Add(digitRem, index);
            return true;
        }
    }
}

/// <inheritdoc/>
internal sealed class UShortRemainderMap : RemainderMap<ushort>
{
    /// <inheritdoc/>
    public override bool AddIfNotExists(
        byte rem, [NonNegative] int index, [NonNegative] out int existingIndex)
    {
        if (Map.TryGetValue(rem, out existingIndex)) return false;
        else
        {
            Map.Add(rem, index);
            return true;
        }
    }

    /// <inheritdoc/>
    public override bool AddIfNotExists(
        ushort rem, [NonNegative] int index, [NonNegative] out int existingIndex)
    {
        if (Map.TryGetValue(rem, out existingIndex)) return false;
        else
        {
            Map.Add(rem, index);
            return true;
        }
    }

    /// <inheritdoc/>
    public override bool AddIfNotExists(
        uint rem, [NonNegative] int index, [NonNegative] out int existingIndex)
    {
        var digitRem = (ushort)rem;

        if (Map.TryGetValue(digitRem, out existingIndex)) return false;
        else
        {
            Map.Add(digitRem, index);
            return true;
        }
    }

    /// <inheritdoc/>
    public override bool AddIfNotExists(
        ulong rem, [NonNegative] int index, [NonNegative] out int existingIndex)
    {
        var digitRem = (ushort)rem;

        if (Map.TryGetValue(digitRem, out existingIndex)) return false;
        else
        {
            Map.Add(digitRem, index);
            return true;
        }
    }

    /// <inheritdoc/>
    public override bool AddIfNotExists(
        [NonNegative] BigInteger rem, [NonNegative] int index, [NonNegative] out int existingIndex)
    {
        var digitRem = (ushort)rem;

        if (Map.TryGetValue(digitRem, out existingIndex)) return false;
        else
        {
            Map.Add(digitRem, index);
            return true;
        }
    }
}

/// <inheritdoc/>
internal sealed class UIntRemainderMap : RemainderMap<uint>
{
    /// <inheritdoc/>
    public override bool AddIfNotExists(
        byte rem, [NonNegative] int index, [NonNegative] out int existingIndex)
    {
        if (Map.TryGetValue(rem, out existingIndex)) return false;
        else
        {
            Map.Add(rem, index);
            return true;
        }
    }

    /// <inheritdoc/>
    public override bool AddIfNotExists(
        ushort rem, [NonNegative] int index, [NonNegative] out int existingIndex)
    {
        if (Map.TryGetValue(rem, out existingIndex)) return false;
        else
        {
            Map.Add(rem, index);
            return true;
        }
    }

    /// <inheritdoc/>
    public override bool AddIfNotExists(
        uint rem, [NonNegative] int index, [NonNegative] out int existingIndex)
    {
        if (Map.TryGetValue(rem, out existingIndex)) return false;
        else
        {
            Map.Add(rem, index);
            return true;
        }
    }

    /// <inheritdoc/>
    public override bool AddIfNotExists(
        ulong rem, [NonNegative] int index, [NonNegative] out int existingIndex)
    {
        var digitRem = (uint)rem;

        if (Map.TryGetValue(digitRem, out existingIndex)) return false;
        else
        {
            Map.Add(digitRem, index);
            return true;
        }
    }

    /// <inheritdoc/>
    public override bool AddIfNotExists(
        [NonNegative] BigInteger rem, [NonNegative] int index, [NonNegative] out int existingIndex)
    {
        var digitRem = (uint)rem;

        if (Map.TryGetValue(digitRem, out existingIndex)) return false;
        else
        {
            Map.Add(digitRem, index);
            return true;
        }
    }
}

/// <inheritdoc/>
internal sealed class ULongRemainderMap : RemainderMap<ulong>
{
    /// <inheritdoc/>
    public override bool AddIfNotExists(
        byte rem, [NonNegative] int index, [NonNegative] out int existingIndex)
    {
        if (Map.TryGetValue(rem, out existingIndex)) return false;
        else
        {
            Map.Add(rem, index);
            return true;
        }
    }

    /// <inheritdoc/>
    public override bool AddIfNotExists(
        ushort rem, [NonNegative] int index, [NonNegative] out int existingIndex)
    {
        if (Map.TryGetValue(rem, out existingIndex)) return false;
        else
        {
            Map.Add(rem, index);
            return true;
        }
    }

    /// <inheritdoc/>
    public override bool AddIfNotExists(
        uint rem, [NonNegative] int index, [NonNegative] out int existingIndex)
    {
        if (Map.TryGetValue(rem, out existingIndex)) return false;
        else
        {
            Map.Add(rem, index);
            return true;
        }
    }

    /// <inheritdoc/>
    public override bool AddIfNotExists(
        ulong rem, [NonNegative] int index, [NonNegative] out int existingIndex)
    {
        if (Map.TryGetValue(rem, out existingIndex)) return false;
        else
        {
            Map.Add(rem, index);
            return true;
        }
    }

    /// <inheritdoc/>
    public override bool AddIfNotExists(
        [NonNegative] BigInteger rem, [NonNegative] int index, [NonNegative] out int existingIndex)
    {
        var digitRem = (ulong)rem;

        if (Map.TryGetValue(digitRem, out existingIndex)) return false;
        else
        {
            Map.Add(digitRem, index);
            return true;
        }
    }
}

/// <inheritdoc/>
internal sealed class BigIntegerRemainderMap : RemainderMap<BigInteger>
{
    /// <inheritdoc/>
    public override bool AddIfNotExists(
        byte rem, [NonNegative] int index, [NonNegative] out int existingIndex)
    {
        if (Map.TryGetValue(rem, out existingIndex)) return false;
        else
        {
            Map.Add(rem, index);
            return true;
        }
    }

    /// <inheritdoc/>
    public override bool AddIfNotExists(
        ushort rem, [NonNegative] int index, [NonNegative] out int existingIndex)
    {
        if (Map.TryGetValue(rem, out existingIndex)) return false;
        else
        {
            Map.Add(rem, index);
            return true;
        }
    }

    /// <inheritdoc/>
    public override bool AddIfNotExists(
        uint rem, [NonNegative] int index, [NonNegative] out int existingIndex)
    {
        if (Map.TryGetValue(rem, out existingIndex)) return false;
        else
        {
            Map.Add(rem, index);
            return true;
        }
    }

    /// <inheritdoc/>
    public override bool AddIfNotExists(
        ulong rem, [NonNegative] int index, [NonNegative] out int existingIndex)
    {
        if (Map.TryGetValue(rem, out existingIndex)) return false;
        else
        {
            Map.Add(rem, index);
            return true;
        }
    }

    /// <inheritdoc/>
    public override bool AddIfNotExists(
        [NonNegative] BigInteger rem, [NonNegative] int index, [NonNegative] out int existingIndex)
    {
        Throw.IfArgNegative(rem, nameof(rem));

        if (Map.TryGetValue(rem, out existingIndex)) return false;
        else
        {
            Map.Add(rem, index);
            return true;
        }
    }
}
#endregion

#region Abstract
/// <summary>
/// Represents a map of <typeparamref name="TDigit"/> remainders to indexes in the digit list of a representation
/// being built for a rational.
/// </summary>
/// <typeparam name="TDigit">The type of remainders stored.</typeparam>
internal abstract class RemainderMap<TDigit> : RemainderMap where TDigit : IComparable<TDigit>, IEquatable<TDigit>
{
    /// <inheritdoc/>
    public sealed override int Count => Map.Count;

    /// <summary>
    /// Gets the dictionary representing the map.
    /// </summary>
    public Dictionary<TDigit, int> Map { get; } = new();
}

/// <summary>
/// Represents a map of remainders to indexes in the digit list of a representation being built for a rational.
/// </summary>
internal abstract class RemainderMap
{
    #region Abstract
    /// <summary>
    /// Gets the number of remainder-index pairs currently in the map.
    /// </summary>
    public abstract int Count { get; }

    /// <summary>
    /// Adds a remainder to the map if it does not already exist.
    /// </summary>
    /// <param name="rem">The remainder to add.</param>
    /// <param name="index">The index to map the remainder to.</param>
    /// <param name="existingIndex">
    /// An <see langword="out"/> parameter to set the index <paramref name="rem"/> is mapped to if it is already
    /// present in the map.
    /// </param>
    /// <returns>
    /// Whether or not the remainder was added.
    /// If <see langword="false"/>, the <paramref name="existingIndex"/> parameter will be set to the existing
    /// index value.
    /// </returns>
    public abstract bool AddIfNotExists(byte rem, [NonNegative] int index, [NonNegative] out int existingIndex);

    /// <inheritdoc cref="AddIfNotExists(ulong, int, out int)"/>
    public abstract bool AddIfNotExists(ushort rem, [NonNegative] int index, [NonNegative] out int existingIndex);

    /// <inheritdoc cref="AddIfNotExists(ulong, int, out int)"/>
    public abstract bool AddIfNotExists(uint rem, [NonNegative] int index, [NonNegative] out int existingIndex);

    /// <inheritdoc cref="AddIfNotExists(byte, int, out int)"/>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="rem"/> was too large to store in the map.
    /// </exception>
    public abstract bool AddIfNotExists(ulong rem, [NonNegative] int index, [NonNegative] out int existingIndex);

    /// <inheritdoc cref="AddIfNotExists(byte, int, out int)"/>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="rem"/> was too large to store in the map, or was negative.
    /// </exception>
    public abstract bool AddIfNotExists(
        [NonNegative] BigInteger rem, [NonNegative] int index, [NonNegative] out int existingIndex);
    #endregion

    #region Factories
    /// <summary>
    /// Gets a remainder map storing the smallest integral type that can represent remainders of the given denominator.
    /// </summary>
    /// <param name="d"></param>
    /// <returns></returns>
    public static RemainderMap NewFromDenominatorSize(ushort d) => NewFromDigitType(Bases.ShortestDigitType(d));

    /// <inheritdoc cref="NewFromDenominatorSize(ushort)"/>
    public static RemainderMap NewFromDenominatorSize(uint d) => NewFromDigitType(Bases.ShortestDigitType(d));

    /// <inheritdoc cref="NewFromDenominatorSize(ushort)"/>
    public static RemainderMap NewFromDenominatorSize(ulong d) => NewFromDigitType(Bases.ShortestDigitType(d));

    /// <inheritdoc cref="NewFromDenominatorSize(ushort)"/>
    public static RemainderMap NewFromDenominatorSize([NonNegative] BigInteger d)
        => NewFromDigitType(Bases.ShortestDigitType(d));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static RemainderMap NewFromDigitType(DigitType type) => type.Value switch
    {
        DigitType.Values.BigInteger => new BigIntegerRemainderMap(),
        DigitType.Values.ULong => new ULongRemainderMap(),
        DigitType.Values.UInt => new UIntRemainderMap(),
        DigitType.Values.UShort => new UShortRemainderMap(),
        _ => new ByteRemainderMap(),
    };
    #endregion
}
#endregion
