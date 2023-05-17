using System.Text.Json;

namespace ScalarKit;

public sealed record JSON : IScalar<JSON, string>
{
	private JSON(string json)
		=> Value = json;

	public string Value { get; }

	public static implicit operator JSON(string json)
	{
		JsonDocument.Parse(json);

		return new JSON(json);
	}

	public override string ToString()
		=> Value;

	public static bool TryFrom(string primitive, out JSON scalar) => throw new NotImplementedException();
}
