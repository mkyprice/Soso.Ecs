using Microsoft.CodeAnalysis;
using SosoEcs.SourceGen.Extensions;
using SosoEcs.SourceGen.Extensions.Systems;
using System.Text;

namespace SosoEcs.SourceGen
{
	[Generator]
	public sealed class SystemsGenerator : IIncrementalGenerator
	{
		public const int QUANTITY = 10;
		
		public void Initialize(IncrementalGeneratorInitializationContext context)
		{
			context.RegisterPostInitializationOutput(ctx =>
			{
				StringBuilder systems = new StringBuilder().CreateSystems(QUANTITY);
				StringBuilder runners = new StringBuilder().CreateSystemRunners(QUANTITY);
				
				ctx.AddSource("ISystem.g", systems.ParseCS());
				ctx.AddSource("Systems.g", runners.ParseCS());
			});
		}
	}
}
