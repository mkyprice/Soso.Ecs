using Microsoft.CodeAnalysis;
using Soso.Ecs.SourceGen.Extensions;
using Soso.Ecs.SourceGen.Extensions.Systems;
using System.Text;
using Soso.Ecs.SourceGen.Extensions.Core;
using Soso.Ecs.SourceGen.Extensions.Inline;

namespace Soso.Ecs.SourceGen
{
	[Generator]
	public sealed class SystemsGenerator : IIncrementalGenerator
	{
		public const int QUANTITY = 20;
		public const string ECS_WORLD_CLASS = "EcsWorld";
		
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
					.AppendBaseNamespace()
					.AppendLine($"public partial class {ECS_WORLD_CLASS}")
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

				StringBuilder queries = FileInitalizer.Init()
					.AppendQueryNamespace()
					.AppendLine($"public partial class {InlineDelegates.CLASS}")
					.AppendLine("{")
					.AppendQueries(false, QUANTITY)
					.AppendQueries(true, QUANTITY)
					.AppendLine("}");
				
				StringBuilder queriesRunner = FileInitalizer.Init()
					.AppendSystemRunnersUsings()
					.AppendLine($"using {Namespaces.INLINES};")
					.AppendBaseNamespace()
					.AppendLine($"public partial class {ECS_WORLD_CLASS}")
					.AppendLine("{")
					.AppendInlineSystemRunner(false, QUANTITY)
					.AppendInlineSystemRunner(true, QUANTITY)
					.AppendLine("}");
				
				ctx.AddSource("ISystem.g", systems.ParseCs());
				ctx.AddSource("Systems.g", runners.ParseCs());
				ctx.AddSource("InlineDelegates.g", queries.ParseCs());
				ctx.AddSource("InlineSystems.g", queriesRunner.ParseCs());
			});
		}
	}
}
