using System.Runtime.Serialization;

namespace VerticalSliceModularMonolith.Shared.Exceptions;

[Serializable]
public sealed class BadRequestException : Exception
{
    public BadRequestException()
    {
    }

    private BadRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public BadRequestException(string message)
        : base(message)
    {
    }

    public BadRequestException(string message, Exception inner)
        : base(message, inner)
    {
    }

    public static void ThrowIfNull(object argument, string message)
    {
        if (argument is null)
        {
            throw new BadRequestException(message);
        }
    }

    public static void ThrowIf(bool val, string message)
    {
        if (val)
        {
            throw new BadRequestException(message);
        }
    }
}
