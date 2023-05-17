using System.Text.Json;
using System.Text.Json.Serialization;

namespace ScalarKit.Extensions.Serialization;

public sealed class ScalarConverter<TScalar, TPrimitive> : JsonConverter<TScalar>
	where TScalar : notnull, IScalar<TScalar, TPrimitive>
	where TPrimitive : notnull
{
	public override TScalar Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		=> (TPrimitive)JsonSerializer.Deserialize(ref reader, typeof(TPrimitive), options)!;

	public override void Write(Utf8JsonWriter writer, TScalar value, JsonSerializerOptions options)
		=> JsonSerializer.Serialize(writer, value.Value, options);
}
