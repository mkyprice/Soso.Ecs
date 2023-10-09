using Microsoft.CodeAnalysis;
using SosoEcs.SourceGen.Extensions;
using SosoEcs.SourceGen.Extensions.Core;
using SosoEcs.SourceGen.Extensions.Systems;
using System.Text;

namespace SosoEcs.SourceGen
{
	[Generator]
	public sealed class SystemsGenerator : IIncrementalGenerator
	{
		public const int QUANTITY = 20;
		
		public void Initialize(IncrementalGeneratorInitializationContext context)
		{
			context.RegisterPostInitializationOutput(ctx =>
			{
				StringBuilder systems = FileInitalizer.Init()
					.AppendSystemsNamespace()
					.AppendSystems(QUANTITY, false)
					.AppendSystems(QUANTITY, true);
					
				StringBuilder runners = FileInitalizer.Init()
					.AppendSystemRunnersUsings()
					.AppendLine("public partial class World")
					.AppendLine("{")
					.CreateSystemRunners(false, false, QUANTITY)
					.CreateSystemRunners(false, true, QUANTITY)
					.CreateSystemRunners(true, false, QUANTITY)
					.CreateSystemRunners(true, true, QUANTITY)
					.CreateSystemRunnersRef(false, false, QUANTITY)
					.CreateSystemRunnersRef(false, true, QUANTITY)
					.CreateSystemRunnersRef(true, false, QUANTITY)
					.CreateSystemRunnersRef(true, true, QUANTITY)
					.AppendLine("}");
				
				ctx.AddSource("ISystem.g", systems.ParseCs());
				ctx.AddSource("Systems.g", runners.ParseCs());
			});
		}
	}
}
