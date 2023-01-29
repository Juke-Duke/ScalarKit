using ScalarKit.Exceptions;

namespace ScalarKit;

public readonly record struct IP : IScalar<IP, string>
{
    public string Value { get; }

    private IP(string ip)
        => Value = ip;

    public static implicit operator IP(string ip)
    {
        try { return (IPV4)ip; } catch (InvalidIPV4Exception) { }
        try { return (IPV6)ip; } catch (InvalidIPV6Exception) { }

        throw new InvalidIPException(ip);
    }
}
