using System.Text.Json;

namespace ScalarKit;

public sealed record JSON : IScalar<JSON, string>
{
    public string Value { get; }

    private JSON(string json)
        => Value = json;

    public static implicit operator JSON(string json)
    {
        JsonDocument.Parse(json);

        return new JSON(json);
    }

    public override string ToString()
        => Value;
}
