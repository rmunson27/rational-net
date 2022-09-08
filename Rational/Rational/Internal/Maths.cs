using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Numerics.Internal;

/// <summary>
/// Internal mathematics helpers.
/// </summary>
internal static class Maths
{
    /// <summary>
    /// Gets the GCD of the values passed in.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static int GCD(int x, int y)
    {
        while (y != 0)
        {
            var newY = x % y;
            x = y;
            y = newY;
        }

        // Avoid taking the absolute value of the min value - it will divide out anyways, as this case can only
        // happen when x and y are min values
        return x == int.MinValue ? int.MinValue : Math.Abs(x);
    }
}
