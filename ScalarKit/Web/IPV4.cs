using System.Text.RegularExpressions;
using ScalarKit.Exceptions;

namespace ScalarKit;

public readonly record struct IPV4 : IScalar<IPV4, string>
{
    private static readonly Regex VALID_CRITERIA = new Regex(@"^(?:(?:(?:0?0?[0-9]|0?[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])\.){3}(?:0?0?[0-9]|0?[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(?:\/(?:[0-9]|[1-2][0-9]|3[0-2]))?)$");

    public string Value { get; }

    private IPV4(string ipv4)
        => Value = ipv4;

    public static implicit operator IPV4(string ipv4)
        => VALID_CRITERIA.IsMatch(ipv4)
            ? new IPV4(ipv4)
            : throw new InvalidIPV4Exception(ipv4);

    public static implicit operator IP(IPV4 ipv4)
        => ipv4.Value;
}
