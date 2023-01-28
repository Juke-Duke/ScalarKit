using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ScalarKit.Extensions.Serialization;

public static class JsonSerializerOptionsExtensions
{
    public static JsonSerializerOptions AddScalarConvertersFromAssembly(this JsonSerializerOptions options, params Assembly[] assemblies)
    {
        options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

        var scalars = assemblies
            .Append(typeof(IScalar<,>).Assembly)
            .SelectMany(a => a.GetTypes()
            .Where(t => !t.IsAbstract && t.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IScalar<,>))));

        foreach (var scalar in scalars)
        {
            var scalarType = scalar;
            var primitiveType = scalar.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IScalar<,>)).GetGenericArguments()[1];

            var converterType = typeof(ScalarConverter<,>).MakeGenericType(scalarType, primitiveType);

            options.Converters.Add((JsonConverter)Activator.CreateInstance(converterType)!);
        }

        return options;
    }

    public static JsonSerializerOptions WriteIndented(this JsonSerializerOptions options)
    {
        options.WriteIndented = true;

        return options;
    }
}
