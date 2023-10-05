using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using SosoEcs.SourceGen.Extensions.Systems;
using System.Text;

namespace SosoEcs.SourceGen
{
	[Generator]
	public class ISystemsGenerator : IIncrementalGenerator
	{
		public void Initialize(IncrementalGeneratorInitializationContext context)
		{
			context.RegisterPostInitializationOutput(ctx =>
			{
				StringBuilder sb = new StringBuilder();
				string source = sb.CreateSystems(10).ToString();
				ctx.AddSource("ISystem.g", SourceText.From(source, Encoding.UTF8));
			});	
			
		}
	}
}
