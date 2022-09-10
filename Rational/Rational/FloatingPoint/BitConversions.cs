using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Numerics.FloatingPoint;

/// <summary>
/// Internal polyfills for platforms earlier than .NET 5.0.
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
}
