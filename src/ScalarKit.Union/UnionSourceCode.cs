using System.Text;

namespace ScalarKit.SourceGenerator.UnionTypeGenerator;

internal static class UnionSourceCode
{
	public static string UnionType(UnionDecleration unionData)
		=> $$"""
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;

    {{UnionDecleration(ref unionData)}}
    {
        private readonly object currentType;

    #pragma warning disable CS0693
        public T? As<T>() where T : class
        {
            if (currentType is T typeValue)
                return typeValue;

            return null;
        }

        public T? As<T>(bool isValueType = true) where T : struct
        {
            if (currentType is T typeValue)
                return typeValue;

            return null;
        }

        public bool Is<T>([NotNullWhen(true)] out T? value) where T : class
            => (value = As<T>()) is not null;

        public bool Is<T>([NotNullWhen(true)] out T? value) where T : struct
            => (value = As<T>(true)) is not null;
    #pragma warning restore CS0693

    {{TypeCreations(ref unionData)}}

    {{TypeSwitch(unionData.Types.Select(t => t.name).ToArray())}}

        public override bool Equals(object? value)
            => Equals(currentType, value);

        public override int GetHashCode()
            => HashCode.Combine(currentType);

        public override string? ToString()
            => currentType?.ToString();

        public static bool operator ==({{unionData.DeclerationName}} left, {{unionData.DeclerationName}} right)
            => Equals(left.currentType, right.currentType);

        public static bool operator !=({{unionData.DeclerationName}} left, {{unionData.DeclerationName}} right)
            => !Equals(left.currentType, right.currentType);
    }
    """;

	public static string TypeCreations(ref UnionDecleration unionData)
	{
		var typeCreations = new StringBuilder();

		for (var i = 0; i < unionData.Types.Length; ++i)
		{
			if (unionData.Types[i].name == $"{unionData.Namespace}.{unionData.Name}" || unionData.Types[i].isInterface)
			{
				typeCreations.AppendLine($"""
                public {unionData.Name}({unionData.Types[i].name} type{i + 1}Value)
                    => currentType = type{i + 1}Value;

            """);
				continue;
			}

			typeCreations.AppendLine($"""
                private {unionData.Name}({unionData.Types[i].name} type{i + 1}Value)
                    => currentType = type{i + 1}Value;

                public static implicit operator {unionData.DeclerationName}({unionData.Types[i].name} type{i + 1}Value)
                    => new(type{i + 1}Value);

            """);
		}

		return typeCreations.Remove(typeCreations.Length - 2, 2).ToString();
	}

	public static string UnionDecleration(ref UnionDecleration unionData)
		=> $"""
        {(unionData.Namespace is not null ? $"namespace {unionData.Namespace};" : "")}

        #nullable enable
        [CompilerGenerated]
        {(unionData.IsClass ? "sealed " : "")}partial {(unionData.IsClass ? "class" : "struct")} {unionData.DeclerationName}{(unionData.IsGeneric ? "\n\t" + string.Join("\n\t", unionData.GenericArguments
			.Select(ga => $"where {ga} : notnull")) : "")}
        """;

	private static string TypeSwitch(string[] types)
	{
		var typeMatching = new StringBuilder().AppendLine("""
            public TResult Switch<TResult>(
        """);

		for (var i = 0; i < types.Length; ++i)
			typeMatching.AppendLine($"""
                    Func<{types[i]}, TResult>? onType{i + 1} = null,
            """);

		typeMatching.AppendLine("""
                Func<object, TResult>? catchAll = null
            )
            {
        """);

		typeMatching.Append(TypeSwitchCallFunc(types));

		typeMatching.AppendLine("""
            }

        """);

		typeMatching.AppendLine("""
            public void Switch(
        """);

		for (var i = 0; i < types.Length; ++i)
			typeMatching.AppendLine($"""
                    Action<{types[i]}>? onType{i + 1} = null,
            """);

		typeMatching.AppendLine("""
                Action<object>? catchAll = null
            )
            {
        """);

		typeMatching.Append(TypeSwitchCallAction(types));

		typeMatching.Append("""
            }
        """);

		return typeMatching.ToString();

		static string TypeSwitchCallFunc(string[] types)
		{
			var typeSwitchCall = new StringBuilder();

			for (var i = 0; i < types.Length; ++i)
				typeSwitchCall.AppendLine($"""
                        if (onType{i + 1} is not null && currentType is {types[i]} type{i + 1}Value)
                            return onType{i + 1}!(type{i + 1}Value);

                """);

			typeSwitchCall.AppendLine($"""
                        return catchAll!(currentType);
                """);

			return typeSwitchCall.ToString();
		}

		static string TypeSwitchCallAction(string[] types)
		{
			var typeSwitchCall = new StringBuilder();

			for (var i = 0; i < types.Length; ++i)
				typeSwitchCall.AppendLine($$"""
                        if (onType{{i + 1}} is not null && currentType is {{types[i]}} type{{i + 1}}Value)
                        {
                            onType{{i + 1}}!(type{{i + 1}}Value);
                            return;
                        }

                """);

			typeSwitchCall.AppendLine($"""
                        catchAll!(currentType);
                """);

			return typeSwitchCall.ToString();
		}
	}
}
