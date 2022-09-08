using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Rem.Core.Numerics;

/// <summary>
/// The exception thrown when an attempt is made to construct a rational number with a zero denominator.
/// </summary>
public class ZeroDenominatorException : DivideByZeroException
{
    /// <summary>
    /// Constructs a new instance of the <see cref="ZeroDenominatorException"/> class with a default error message.
    /// </summary>
    public ZeroDenominatorException() : base("Denominator cannot be zero.") { }

    /// <summary>
    /// Constructs a new instance of the <see cref="ZeroDenominatorException"/> class with the supplied error message.
    /// </summary>
    /// <param name="message"></param>
    public ZeroDenominatorException(string? message) : base(message) { }

    /// <summary>
    /// Constructs a new instance of the <see cref="ZeroDenominatorException"/> class with the supplied error message
    /// and inner exception.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public ZeroDenominatorException(string? message, Exception? innerException) : base(message, innerException) { }

    /// <summary>
    /// Constructs a new instance of the <see cref="ZeroDenominatorException"/> class from the serialization data
    /// passed in (serialization constructor).
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    protected ZeroDenominatorException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
