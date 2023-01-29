namespace ScalarKit.Exceptions;

public sealed class InvalidIPV6Exception : Exception
{
    public InvalidIPV6Exception()
        : base("The IPV6 is invalid.") { }

    public InvalidIPV6Exception(string ipv6)
        : base($"The IPV6 is invalid: {ipv6}.") { }
}
