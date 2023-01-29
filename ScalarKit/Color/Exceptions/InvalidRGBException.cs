namespace ScalarKit.Exceptions;

public sealed class InvalidRGBException : Exception
{
    public InvalidRGBException()
        : base("Invalid RGB value.") { }

    public InvalidRGBException(string rgb)
        : base($"Invalid RGB value: {rgb}.") { }
}
