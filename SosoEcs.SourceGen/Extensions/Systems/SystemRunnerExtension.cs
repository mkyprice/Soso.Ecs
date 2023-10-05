using SosoEcs.SourceGen.Extensions.Core;
using System.Text;

namespace SosoEcs.SourceGen.Extensions.Systems
{
	public static class SystemRunnerExtension
	{
		public static StringBuilder CreateSystemRunners(this StringBuilder sb, int amount)
		{
			sb.AppendLine($"using {Namespaces.ISYSTEMS};");
			sb.AppendLine($"using {Namespaces.COMPONENTS_CORE};");
			sb.AppendLine($"using {Namespaces.QUERIES};");
			sb.AppendLine($"namespace {Namespaces.BASE};");
			sb.AppendLine("public partial class World");
			sb.AppendLine("{");
			StringBuilder interfaceGenerics = new StringBuilder();
			StringBuilder archetypeGetGenerics = new StringBuilder();
			StringBuilder updateGenerics = new StringBuilder();
			for (int i = 0; i < amount; i++)
			{
				string generic = "T" + i;
				interfaceGenerics.Append(generic);
				archetypeGetGenerics.Append($"typeof({generic})");
				updateGenerics.Append($"ref archetype.Get<T{i}>(i)");
				
				sb.AppendLine($"public void Run<TS, {interfaceGenerics.ToString()}>() where TS : struct, ISystem<{interfaceGenerics.ToString()}>");
				sb.AppendLine("{");
				sb.AppendLine("var system = new TS();");
				sb.AppendLine($"Archetype archetype = GetOrCreateArchetype({archetypeGetGenerics.ToString()});");
				sb.AppendLine("for (int i = 0; i < archetype.Size; i++)");
				sb.AppendLine("{");
				sb.AppendLine($"system.Update({updateGenerics.ToString()});");
				sb.AppendLine("}");
				sb.AppendLine("}");
				

				interfaceGenerics.Append(", ");
				archetypeGetGenerics.Append(", ");
				updateGenerics.Append(", ");
			}
			
			sb.AppendLine("}");
			return sb;
		}
	}
}
