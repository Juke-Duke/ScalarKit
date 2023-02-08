using System.Text.RegularExpressions;

namespace ScalarKit;

public readonly record struct IPv4 : IScalar<IPv4, string>
{
	private static readonly Regex VALID_CRITERIA = new(
		@"^(?:(?:(?:0?0?[0-9]|0?[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])\.){3}(?:0?0?[0-9]|0?[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(?:\/(?:[0-9]|[1-2][0-9]|3[0-2]))?)$"
	);

	private IPv4(string ipv4)
		=> Value = ipv4;

	public string Value { get; }

	public static implicit operator IPv4(string ipv4)
		=> VALID_CRITERIA.IsMatch(ipv4)
			? new IPv4(ipv4)
			: throw new FormatException($"{nameof(IPv4)} must be a valid IPv4 address.");

	public override string ToString()
		=> Value;

	public static bool TryFrom(string primitive, out IPv4 scalar) => throw new NotImplementedException();
}
