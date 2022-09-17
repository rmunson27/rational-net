using Rem.Core.Numerics.Digits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemTest.Core.Numerics;

/// <summary>
/// Represents the structure of the (internally constructible) <see cref="RatioDigitRep"/> class, with the supplied
/// whole and partial
/// </summary>
/// <param name="IsNegative"></param>
/// <param name="Whole"></param>
/// <param name="Terminating"></param>
/// <param name="Repeating"></param>
internal sealed record class ExpectedRatioDigitRep(
    bool IsNegative, DigitList Whole, DigitList Terminating, DigitList? Repeating = null);

