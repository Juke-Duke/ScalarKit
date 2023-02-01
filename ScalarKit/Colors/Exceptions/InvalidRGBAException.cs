namespace ScalarKit.Exceptions;

public sealed class InvalidRGBAException : Exception
{
    public InvalidRGBAException()
        : base("Invalid RGBA value.") { }

    public InvalidRGBAException(string rgb)
        : base($"Invalid RGB value: {rgb}.") { }
}
