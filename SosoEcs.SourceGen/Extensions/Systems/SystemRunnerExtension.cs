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
			sb.AppendLine($"namespace {Namespaces.BASE};");
			return sb;
		}

		public static StringBuilder CreateSystemRunnersRef(this StringBuilder sb, int amount)
		{
			StringBuilder interfaceGenerics = new StringBuilder();
			StringBuilder archetypeGetGenerics = new StringBuilder();
			StringBuilder updateGenerics = new StringBuilder();
			for (int i = 0; i < amount; i++)
			{
				string generic = "T" + i;
				interfaceGenerics.Append(generic);
				archetypeGetGenerics.Append($"typeof({generic})");
				updateGenerics.Append($"ref archetype.Get<T{i}>(i)");
				
				sb.AppendLine($"public void Run<TS, {interfaceGenerics}>(ref TS system) where TS : struct, ISystem<{interfaceGenerics}>");
				sb.AppendLine("{");
				sb.AppendArchetype(archetypeGetGenerics.ToString());
				sb.AppendSystemRunnerLoop(updateGenerics.ToString());
				sb.AppendLine("}");
				

				interfaceGenerics.Append(", ");
				archetypeGetGenerics.Append(", ");
				updateGenerics.Append(", ");
			}
			return sb;
		}
		
		public static StringBuilder CreateSystemRunners(this StringBuilder sb, int amount)
		{
			StringBuilder interfaceGenerics = new StringBuilder();
			StringBuilder archetypeGetGenerics = new StringBuilder();
			StringBuilder updateGenerics = new StringBuilder();
			for (int i = 0; i < amount; i++)
			{
				string generic = "T" + i;
				interfaceGenerics.Append(generic);
				archetypeGetGenerics.Append($"typeof({generic})");
				updateGenerics.Append($"ref archetype.Get<T{i}>(i)");
				
				sb.AppendLine($"public void Run<TS, {interfaceGenerics}>() where TS : struct, ISystem<{interfaceGenerics}>");
				sb.AppendLine("{");
				sb.AppendLine("var system = new TS();");
				
				sb.AppendArchetype(archetypeGetGenerics.ToString());
				sb.AppendSystemRunnerLoop(updateGenerics.ToString());
				
				sb.AppendLine("}");
				

				interfaceGenerics.Append(", ");
				archetypeGetGenerics.Append(", ");
				updateGenerics.Append(", ");
			}
			
			return sb;
		}

		private static StringBuilder AppendSystemRunnerLoop(this StringBuilder sb, string updateGenerics)
		{
			sb.AppendLine("for (int i = 0; i < archetype.Size; i++)");
			sb.AppendLine("{");
			sb.AppendLine($"system.Update({updateGenerics});");
			sb.AppendLine("}");
			return sb;
		}

		private static StringBuilder AppendArchetype(this StringBuilder sb, string archetypeGetGenerics)
		{
			sb.AppendLine($"Archetype archetype = GetOrCreateArchetype({archetypeGetGenerics});");
			return sb;
		}
	}
}
