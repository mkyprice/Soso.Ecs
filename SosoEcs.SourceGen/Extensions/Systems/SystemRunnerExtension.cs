using SosoEcs.SourceGen.Extensions.Core;
using System.Text;

namespace SosoEcs.SourceGen.Extensions.Systems
{
	public static class SystemRunnerExtension
	{
		public static StringBuilder AppendSystemRunnersUsings(this StringBuilder sb)
		{
			sb.AppendLine($"using {Namespaces.ISYSTEMS};");
			sb.AppendLine($"using {Namespaces.COMPONENTS_CORE};");
			sb.AppendLine($"using {Namespaces.QUERIES};");
			sb.AppendLine($"using System.Collections.Generic;");
			sb.AppendLine($"using System;");
			sb.AppendLine($"using System.Linq;");
			sb.AppendLine($"using System.Threading.Tasks;");
			sb.AppendLine($"namespace {Namespaces.BASE};");
			return sb;
		}

		public static StringBuilder CreateSystemRunnersRef(this StringBuilder sb, bool parallel, int amount)
		{
			StringBuilder interfaceGenerics = new StringBuilder();
			StringBuilder archetypeGetGenerics = new StringBuilder();
			string funcName = parallel ? "ParallelRun" : "Run";
			for (int i = 0; i < amount; i++)
			{
				string generic = "T" + i;
				interfaceGenerics.Append(generic);
				archetypeGetGenerics.Append($"typeof({generic})");
				
				sb.AppendLine($"public void {funcName}<TS, {interfaceGenerics}>(TS system) where TS : struct, ISystem<{interfaceGenerics}>");
				sb.AppendLine("{");
				sb.AppendSystemRunnerLoop(i, parallel);
				sb.AppendLine("}");
				

				interfaceGenerics.Append(", ");
				archetypeGetGenerics.Append(", ");
			}
			return sb;
		}
		
		public static StringBuilder CreateSystemRunners(this StringBuilder sb, bool parallel, int amount)
		{
			StringBuilder interfaceGenerics = new StringBuilder();
			StringBuilder archetypeGetGenerics = new StringBuilder();
			string funcName = parallel ? "ParallelRun" : "Run";
			for (int i = 0; i < amount; i++)
			{
				string generic = "T" + i;
				interfaceGenerics.Append(generic);
				archetypeGetGenerics.Append($"typeof({generic})");
				
				sb.AppendLine($"public void {funcName}<TS, {interfaceGenerics}>() where TS : struct, ISystem<{interfaceGenerics}>");
				sb.AppendLine("{");
				sb.AppendLine("var system = new TS();");
				
				sb.AppendSystemRunnerLoop(i, parallel);
				
				sb.AppendLine("}");
				

				interfaceGenerics.Append(", ");
				archetypeGetGenerics.Append(", ");
			}
			
			return sb;
		}

		private static StringBuilder AppendSystemRunnerLoop(this StringBuilder sb, int amount, bool parallel)
		{
			StringBuilder update = new StringBuilder();
			StringBuilder archetypes = new StringBuilder();
			StringBuilder arrays = new StringBuilder();
			for (int i = 0; i <= amount; i++)
			{
				arrays.AppendLine($"T{i}[] t{i}s = archetype.GetArray<T{i}>();");
				update.Append($"ref t{i}s[i]");
				archetypes.Append($"typeof(T{i})");
				if (i < amount)
				{
					update.Append(", ");
					archetypes.Append(", ");
				}
			}
			sb.AppendLine($"foreach (var archetype in GetArchetypes({archetypes}))");
			sb.AppendLine("{");
			sb.AppendLine(arrays.ToString());
			if (parallel) sb.AppendLine("Parallel.For(0, archetype.Size, i =>");
			else sb.AppendLine("for (int i = 0; i < archetype.Size; i++)");
			sb.AppendLine("{");
			sb.AppendLine($"system.Update({update});");
			sb.AppendLine("}");
			if (parallel) sb.Append(");");
			sb.AppendLine("}");
			return sb;
		}
	}
}
