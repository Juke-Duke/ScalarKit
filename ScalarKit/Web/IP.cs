using ScalarKit.Exceptions;

namespace ScalarKit;

public readonly record struct IP : IScalar<IP, string>
{
    public string Value { get; }

    private IP(string ip)
        => Value = ip;

    public static implicit operator IP(string ip)
    {
        try { return (IPV4)ip; }
            catch (Exception) { }
        try { return (IPV6)ip; }
            catch (Exception) { }

        throw new InvalidIPException(ip);
    }

    public static implicit operator IP(IPV4 ipv4)
        => new(ipv4.Value);

    public static implicit operator IP(IPV6 ipv6)
        => new(ipv6.Value);

    public override string ToString()
        => Value;
}
