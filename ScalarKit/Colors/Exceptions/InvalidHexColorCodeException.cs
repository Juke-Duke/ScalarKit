namespace ScalarKit.Exceptions;

public sealed class InvalidHexColorCodeException : Exception
{
    public InvalidHexColorCodeException()
        : base("The hex color code is invalid.") { }

    public InvalidHexColorCodeException(string hexColorCode)
        : base($"The hex color code is invalid: {hexColorCode}") { }
}
