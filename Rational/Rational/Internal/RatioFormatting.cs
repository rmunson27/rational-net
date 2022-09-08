using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Numerics.Internal;

/// <summary>
/// Internal static functionality relating to general ratio string formatting.
/// </summary>
internal static class RatioFormatting
{
    /// <summary>
    /// The character that separates the numerator and denominator in <see cref="object.ToString"/> overrides of
    /// ratio types.
    /// </summary>
    public const char ComponentSeparator = '/';
}
