namespace ScalarKit.Exceptions;

public sealed class InvalidIPException : Exception
{
    public InvalidIPException()
        : base("The IP is invalid.") { }

    public InvalidIPException(string ip)
        : base($"The IP is invalid: {ip}.") { }
}
