namespace ScalarKit;

public readonly record struct IP : IScalar<IP, string>
{
	private IP(string ip)
		=> Value = ip;

	public string Value { get; }

	public static implicit operator IP(string ip)
	{
		try
		{
			return (IPv4)ip;
		}
		catch { }

		try
		{
			return (IPv6)ip;
		}
		catch { }

		throw new FormatException($"{nameof(IP)} must be a valid IPv4 or IPv6 address.");
	}

	public override string ToString()
		=> Value;

	public static bool TryFrom(string primitive, out IP scalar) => throw new NotImplementedException();

	public static implicit operator IP(IPv4 ipv4)
		=> new(ipv4.Value);

	public static implicit operator IP(IPv6 ipv6)
		=> new(ipv6.Value);
}
