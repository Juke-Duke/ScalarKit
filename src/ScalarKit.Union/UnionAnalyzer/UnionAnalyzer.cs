using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

using static ScalarKit.SourceGenerator.UnionTypeGenerator.UnionDiagnosticDiscriptors;

namespace ScalarKit.SourceGenerator.UnionTypeGenerator;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class UnionTypeAnalyzer : DiagnosticAnalyzer
{
	private const string UnionAttributeDisplayName = "ScalarKit.SourceGenerator.UnionTypeGenerator.UnionAttribute";

	public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(
		DuplicateUnionType,
		NullUnionType,
		ObjectUnionType,
		SwitchNotExhaustive,
		UnionCastAlwaysNull,
		UnionNeverOfType
	);

	public override void Initialize(AnalysisContext context)
	{
		context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
		context.EnableConcurrentExecution();
		context.RegisterSyntaxNodeAction(AnalyzeType, SyntaxKind.ClassDeclaration, SyntaxKind.StructDeclaration);
		context.RegisterOperationAction(AnalyzeSwitchCall, OperationKind.Invocation);
		context.RegisterOperationAction(AnalyzeIsPattern, OperationKind.Invocation);
		context.RegisterOperationAction(AnalyzeAsPattern, OperationKind.Invocation);
	}

	private void AnalyzeType(SyntaxNodeAnalysisContext context)
	{
		var unionType = (INamedTypeSymbol)context.SemanticModel.GetDeclaredSymbol(context.Node)!;

		if (!IsUnionType(unionType, out var unionAttribute))
			return;

		var typeNames = new HashSet<string>();

		var types = new[] { unionAttribute.ConstructorArguments[0].Value }
			.Concat(unionAttribute.ConstructorArguments[1].Values
				.Select(v => v.Value))
			.Cast<INamedTypeSymbol>()
			.ToArray();

		foreach (var type in types)
		{
			if (type is null || type.Name is nameof(Nullable))
				context.ReportDiagnostic(
					Diagnostic.Create(
						descriptor: NullUnionType,
						location: unionType.Locations[0],
						messageArgs: unionType.Name
					)
				);
			else if (type.SpecialType is SpecialType.System_Object)
				context.ReportDiagnostic(
					Diagnostic.Create(
						descriptor: ObjectUnionType,
						location: unionType.Locations[0],
						messageArgs: unionType.Name
					)
				);
			else if (!typeNames.Add(type.ToDisplayString()))
				context.ReportDiagnostic(
					Diagnostic.Create(
						descriptor: DuplicateUnionType,
						location: unionType.Locations[0],
						messageArgs: new[]
						{
							type.ToMinimalDisplayString(
								context.SemanticModel,
								context.Node.GetLocation().SourceSpan.Start,
								SymbolDisplayFormat.MinimallyQualifiedFormat
							),
							unionType.Name
						}
					)
				);
		}
	}

	private void AnalyzeSwitchCall(OperationAnalysisContext context)
	{
		const string Switch = "Switch";

		var invocation = (IInvocationOperation)context.Operation;
		var method = invocation.TargetMethod;

		if (method.Name is not Switch)
			return;

		var containedType = (INamedTypeSymbol)method.ContainingSymbol!;

		if (!IsUnionType(containedType, out var _))
			return;

		if (DefinedCatchAllCase(invocation))
			return;

		var unhandledType = (invocation.Arguments
			.First(a => a.IsImplicit)
			.Parameter?.Type as INamedTypeSymbol)?
			.TypeArguments[0];

		if (unhandledType!.SpecialType is not SpecialType.System_Object)
			context.ReportDiagnostic(
				Diagnostic.Create(
					descriptor: SwitchNotExhaustive,
					location: invocation.Syntax.GetLocation(),
					messageArgs: unhandledType.ToMinimalDisplayString(
						invocation.SemanticModel!,
						context.Operation.Syntax.GetLocation().SourceSpan.Start,
						SymbolDisplayFormat.MinimallyQualifiedFormat
					)
				)
			);
	}

	private void AnalyzeAsPattern(OperationAnalysisContext context)
	{
		const string As = "As";

		var invocation = (IInvocationOperation)context.Operation;
		var method = invocation.TargetMethod;

		if (method.Name is not As)
			return;

		var containedType = (INamedTypeSymbol)method.ContainingSymbol!;

		if (!IsUnionType(containedType, out var unionAttribute))
			return;

		if (!CanCastTo(containedType, (INamedTypeSymbol)method.TypeArguments[0], unionAttribute))
			context.ReportDiagnostic(
				Diagnostic.Create(
					descriptor: UnionCastAlwaysNull,
					location: invocation.Syntax.GetLocation(),
					messageArgs: new[]
					{
						containedType.ToMinimalDisplayString(
							invocation.SemanticModel!,
							context.Operation.Syntax.GetLocation().SourceSpan.Start,
							SymbolDisplayFormat.MinimallyQualifiedFormat
						),
						method.TypeArguments[0].ToMinimalDisplayString(
							invocation.SemanticModel!,
							context.Operation.Syntax.GetLocation().SourceSpan.Start,
							SymbolDisplayFormat.MinimallyQualifiedFormat
						)
					}
				)
			);
	}

	private void AnalyzeIsPattern(OperationAnalysisContext context)
	{
		const string Is = "Is";

		var invocation = (IInvocationOperation)context.Operation;
		var method = invocation.TargetMethod;

		if (method.Name is not Is)
			return;

		var containedType = (INamedTypeSymbol)method.ContainingSymbol!;

		if (!IsUnionType(containedType, out var unionAttribute))
			return;

		if (!CanCastTo(containedType, (INamedTypeSymbol)method.TypeArguments[0], unionAttribute))
			context.ReportDiagnostic(
				Diagnostic.Create(
					descriptor: UnionNeverOfType,
					location: invocation.Syntax.GetLocation(),
					messageArgs: null
				)
			);
	}

	private static bool IsUnionType(
		INamedTypeSymbol typeSymbol,
		[NotNullWhen(true)] out AttributeData? unionAttribute)
	{
		unionAttribute = null;

		if (typeSymbol.TypeKind is not (TypeKind.Class or TypeKind.Struct))
			return false;

		return (unionAttribute = typeSymbol
			.GetAttributes()
			.FirstOrDefault(a => a.AttributeClass?
				.ToDisplayString() == UnionAttributeDisplayName))
			is not null;
	}

	private static bool DefinedCatchAllCase(IInvocationOperation invocation)
		=> !invocation.Arguments.First(a => a.Parameter?.Name is "catchAll").IsImplicit;

	private static bool CanCastTo(INamedTypeSymbol unionType, INamedTypeSymbol type, AttributeData unionAttribute)
		=> unionType.TypeArguments.Contains(type)
		|| unionAttribute.ConstructorArguments[1].Values
			.Append(unionAttribute.ConstructorArguments[0])
			.Select(v => v.Value)
			.Cast<INamedTypeSymbol>()
			.Contains(type, SymbolEqualityComparer.Default);
}
