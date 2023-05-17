using Microsoft.CodeAnalysis;

namespace ScalarKit.SourceGenerator.UnionTypeGenerator;

internal readonly record struct UnionDecleration
{
	public string DeclerationName { get; init; }

	public string Name { get; init; }

	public bool IsClass { get; init; }

	public bool IsGeneric { get; init; }

	public string[] GenericArguments { get; init; }

	public string? Namespace { get; init; }

	public (string name, bool isInterface)[] Types { get; init; }

	public static UnionDecleration ExtractUnionData(GeneratorAttributeSyntaxContext context)
	{
		var unionAttrData = context.Attributes.First(ad => ad.AttributeClass!.Name == "UnionAttribute");

		var unionTypes = new (string name, bool isInterface)[] { ((unionAttrData.ConstructorArguments[0].Value as ITypeSymbol)!.ToDisplayString(), (unionAttrData.ConstructorArguments[0].Value as ITypeSymbol)!.TypeKind is TypeKind.Interface) };

		if (unionAttrData.ConstructorArguments.Length > 1)
			unionTypes = unionTypes
				.Concat(unionAttrData.ConstructorArguments[1].Values
					.Select(t => ((t.Value as ITypeSymbol)!.ToDisplayString(), (t.Value as ITypeSymbol)!.TypeKind is TypeKind.Interface)))
				.ToArray();

		// check if the target is generic and get its generic arguments
		if (context.TargetSymbol is INamedTypeSymbol namedTypeSymbol && namedTypeSymbol.IsGenericType)
			unionTypes = unionTypes
				.Concat(namedTypeSymbol.TypeArguments.Select(t => (t.ToDisplayString(), t.TypeKind is TypeKind.Interface)))
				.ToArray();

		var isClass = context.TargetSymbol.Kind == SymbolKind.NamedType
			&& ((INamedTypeSymbol)context.TargetSymbol).TypeKind == TypeKind.Class;

		return new()
		{
			DeclerationName = ((INamedTypeSymbol)context.TargetSymbol).ToMinimalDisplayString(
				context.SemanticModel,
				context.TargetSymbol.Locations[0].SourceSpan.Start,
				SymbolDisplayFormat.MinimallyQualifiedFormat),
			Name = context.TargetSymbol.Name,
			IsClass = isClass,
			IsGeneric = context.TargetSymbol is INamedTypeSymbol unionType && unionType.IsGenericType,
			GenericArguments = (context.TargetSymbol as INamedTypeSymbol)!
				.TypeArguments
				.Select(t => t.Name)
				.ToArray(),
			Namespace = context.TargetSymbol.ContainingNamespace.IsGlobalNamespace
				? null
				: context.TargetSymbol.ContainingNamespace.ToDisplayString(),
			Types = unionTypes
		};
	}
}
