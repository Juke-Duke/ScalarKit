using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace ScalarKit.SourceGenerator.UnionTypeGenerator;

[Generator(LanguageNames.CSharp)]
public sealed class UnionGenerator : IIncrementalGenerator
{
	private const string UnionAttributeDisplayName = "ScalarKit.Union.UnionAttribute";

	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		var unionTypes = context.SyntaxProvider
			.ForAttributeWithMetadataName(
				fullyQualifiedMetadataName: UnionAttributeDisplayName,
				predicate: static (node, _) => IsClassOrStruct(node),
				transform: static (node, _) => UnionDecleration.ExtractUnionData(node)
			);

		context.RegisterSourceOutput(unionTypes, (ctx, unionTypeData) => ctx.AddSource(
				$"{unionTypeData.Name}.g.cs",
				SourceText.From(UnionSourceCode.UnionType(unionTypeData), Encoding.UTF8)
			)
		);
	}

	private static bool IsClassOrStruct(SyntaxNode node)
		=> node is ClassDeclarationSyntax or StructDeclarationSyntax;
}
