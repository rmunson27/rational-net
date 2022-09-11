using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Numerics.FloatingPoint;

/// <summary>
/// Internal polyfills for earlier platforms.
/// </summary>
internal static class BitConversions
{
    /// <summary>
    /// Gets the bytes of a <see cref="float"/> as a <see cref="uint"/>.
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    public static uint SingleToUInt32Bits(float f)
#if NET5_0_OR_GREATER
        => unchecked((uint)BitConverter.SingleToInt32Bits(f));
#else
        => BitConverter.ToUInt32(BitConverter.GetBytes(f), 0);
#endif

    /// <summary>
    /// Gets a <see cref="float"/> from the bits of a <see cref="uint"/>.
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public static float UInt32BitsToSingle(uint i)
#if NET5_0_OR_GREATER
        => BitConverter.Int32BitsToSingle(unchecked((int)i));
#else
        => BitConverter.ToSingle(BitConverter.GetBytes(i), 0);
#endif

#if NET5_0_OR_GREATER
    /// <summary>
    /// Gets the bits of a <see cref="Half"/> as a <see cref="ushort"/>.
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    public static ushort HalfToUInt16Bits(Half h)
#if NET6_0_OR_GREATER
        => BitConverter.HalfToUInt16Bits(h);
#else
        => Unsafe.As<Half, ushort>(ref h);
#endif

    /// <summary>
    /// Gets a <see cref="Half"/> from the bits of a <see cref="ushort"/>.
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public static Half UInt16BitsToHalf(ushort i)
#if NET6_0_OR_GREATER
        => BitConverter.UInt16BitsToHalf(i);
#else
        => Unsafe.As<ushort, Half>(ref i);
#endif
#endif
}
