using SosoEcs.SourceGen.Extensions.Core;
using System.Text;

namespace SosoEcs.SourceGen.Extensions.Systems
{
	public static class SystemRunnerExtension
	{
		public static StringBuilder AppendBaseNamespace(this StringBuilder sb)
		{
			sb.AppendLine($"namespace {Namespaces.BASE};");
			return sb;
		}
		
		public static StringBuilder AppendSystemRunnersUsings(this StringBuilder sb)
		{
			sb.AppendLine($"using {Namespaces.ISYSTEMS};");
			sb.AppendLine($"using {Namespaces.COMPONENTS_CORE};");
			sb.AppendLine($"using System.Collections.Generic;");
			sb.AppendLine($"using System;");
			sb.AppendLine($"using System.Linq;");
			sb.AppendLine($"using System.Threading.Tasks;");
			return sb;
		}

		public static StringBuilder CreateSystemRunnersRef(this StringBuilder sb, bool parallel, bool entitySystem, int amount)
		{
			StringBuilder interfaceGenerics = new StringBuilder();
			StringBuilder archetypeGetGenerics = new StringBuilder();
			for (int i = 0; i < amount; i++)
			{
				string generic = "T" + i;
				interfaceGenerics.Append(generic);
				archetypeGetGenerics.Append($"typeof({generic})");
				
				sb.AppendFunc(parallel, entitySystem, "TS system", interfaceGenerics);
				sb.AppendLine("{");
				sb.AppendSystemRunnerLoop(i, "system.Update", parallel, entitySystem);
				sb.AppendLine("}");
				

				interfaceGenerics.Append(", ");
				archetypeGetGenerics.Append(", ");
			}
			return sb;
		}
		
		public static StringBuilder CreateSystemRunners(this StringBuilder sb, bool parallel, bool entitySystem, int amount)
		{
			StringBuilder interfaceGenerics = new StringBuilder();
			StringBuilder archetypeGetGenerics = new StringBuilder();
			for (int i = 0; i < amount; i++)
			{
				string generic = "T" + i;
				interfaceGenerics.Append(generic);
				archetypeGetGenerics.Append($"typeof({generic})");

				sb.AppendFunc(parallel, entitySystem, String.Empty, interfaceGenerics)
					.AppendLine("{")
					.AppendLine("var system = new TS();")
					.AppendSystemRunnerLoop(i, "system.Update", parallel, entitySystem)
					.AppendLine("}");
				

				interfaceGenerics.Append(", ");
				archetypeGetGenerics.Append(", ");
			}
			
			return sb;
		}

		private static StringBuilder AppendFunc(this StringBuilder sb, bool parallel, bool entitySystem, string args, StringBuilder interfaceGenerics)
		{
			string functionName = "Run";
			if (parallel)
			{
				functionName += "Parallel";
			}
			if (entitySystem)
			{
				functionName += "Entity";
			}
			sb.AppendLine($"public void {functionName}<TS, {interfaceGenerics}>({args}) where TS : struct, ");
			sb.AppendISystemName(entitySystem);
			sb.Append($"<{interfaceGenerics}>");
			return sb;
		}

		public static StringBuilder AppendSystemRunnerLoop(this StringBuilder sb, int amount, string function, bool parallel, bool entitySystem)
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
			sb.AppendLine($"{function}(");
			if (entitySystem)
			{
				sb.Append("archetype.GetEntity(i), ");
			}
			sb.Append($"{update});");
			sb.AppendLine("}");
			if (parallel) sb.Append(");");
			sb.AppendLine("}");
			return sb;
		}
	}
}
