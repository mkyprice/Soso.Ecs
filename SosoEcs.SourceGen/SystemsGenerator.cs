﻿using Microsoft.CodeAnalysis;
using SosoEcs.SourceGen.Extensions;
using SosoEcs.SourceGen.Extensions.Core;
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
				StringBuilder systems = FileInitalizer.Init().CreateSystems(QUANTITY);
				StringBuilder runners = FileInitalizer.Init()
					.AppendSystemRunnersUsings()
					.AppendLine("public partial class World")
					.AppendLine("{")
					.CreateSystemRunners(QUANTITY)
					.CreateSystemRunnersRef(QUANTITY)
					.AppendLine("}");
				
				ctx.AddSource("ISystem.g", systems.ParseCs());
				ctx.AddSource("Systems.g", runners.ParseCs());
			});
		}
	}
}
