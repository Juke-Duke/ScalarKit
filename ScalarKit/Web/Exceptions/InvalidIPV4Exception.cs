namespace ScalarKit.Exceptions;

public sealed class InvalidIPV4Exception : Exception
{
    public InvalidIPV4Exception()
        : base("The IPV4 is invalid.") { }

    public InvalidIPV4Exception(string ipv4)
        : base($"The IPV4 is invalid: {ipv4}.") { }
}
