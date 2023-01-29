namespace ScalarKit.Exceptions;

public sealed class InvalidDurationException : Exception
{
    public InvalidDurationException()
        : base("The duration is invalid.") { }

    public InvalidDurationException(string duration)
        : base($"The duration is invalid: {duration}.") { }
}
