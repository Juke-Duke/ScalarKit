using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace ScalarKit.Union.UnionAttributeGenerator;

[Generator(LanguageNames.CSharp)]
public sealed class UnionAttributeGenerator : IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
				$"{nameof(UnionAttributeSourceCode.UnionAttribute)}.g.cs",
				SourceText.From(UnionAttributeSourceCode.UnionAttribute, Encoding.UTF8)
			)
		);
	}
}
