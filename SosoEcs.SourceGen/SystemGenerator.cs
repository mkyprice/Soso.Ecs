using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using SosoEcs.SourceGen.Extensions.Systems;
using System.Text;

namespace SosoEcs.SourceGen
{
	[Generator]
	public sealed class SystemGenerator : IIncrementalGenerator
	{
		public void Initialize(IncrementalGeneratorInitializationContext context)
		{
			context.RegisterPostInitializationOutput(ctx =>
			{
				string source = new StringBuilder().CreateSystemRunners(10).ToString();
				ctx.AddSource("Systems.g", SourceText.From(source, Encoding.UTF8));
			});
	}
	}
}
