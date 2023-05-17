using Microsoft.CodeAnalysis;

namespace ScalarKit.SourceGenerator.UnionTypeGenerator;

public static class UnionDiagnosticDiscriptors
{
	private const string UnionGenerator = "UnionGenerator";

	public static readonly DiagnosticDescriptor DuplicateUnionType = new(
		id: "SKG001",
		title: nameof(DuplicateUnionType),
		messageFormat: "Type '{0}' is already defined in union '{1}'",
		category: UnionGenerator,
		DiagnosticSeverity.Error,
		isEnabledByDefault: true
	);

	public static readonly DiagnosticDescriptor NullUnionType = new(
		id: "SKG002",
		title: nameof(NullUnionType),
		messageFormat: "Nullable and/or null are invalid union types",
		category: UnionGenerator,
		DiagnosticSeverity.Error,
		isEnabledByDefault: true
	);

	public static readonly DiagnosticDescriptor ObjectUnionType = new(
		id: "SKG003",
		title: nameof(ObjectUnionType),
		messageFormat: "Object is an invalid union type",
		category: UnionGenerator,
		DiagnosticSeverity.Error,
		isEnabledByDefault: true
	);

	public static readonly DiagnosticDescriptor SwitchNotExhaustive = new(
		id: "SKG004",
		title: nameof(SwitchNotExhaustive),
		messageFormat: "The switch expression does not handle all possible values of its input type (it is not exhaustive). For example, the pattern '{0}' is not covered.",
		category: UnionGenerator,
		DiagnosticSeverity.Error,
		isEnabledByDefault: true
	);

	public static readonly DiagnosticDescriptor UnionCastAlwaysNull = new(
		id: "SKG005",
		title: nameof(UnionCastAlwaysNull),
		messageFormat: "Conversion from union '{0}' to '{1}' always results in null",
		category: UnionGenerator,
		DiagnosticSeverity.Warning,
		isEnabledByDefault: true
	);

	public static readonly DiagnosticDescriptor UnionNeverOfType = new(
		id: "SKG006",
		title: nameof(UnionNeverOfType),
		messageFormat: "The given expression never matches the provided pattern",
		category: UnionGenerator,
		DiagnosticSeverity.Warning,
		isEnabledByDefault: true
	);
}
