using SosoEcs.SourceGen.Extensions.Core;
using System.Text;

namespace SosoEcs.SourceGen.Extensions.Systems
{
	public static class SystemRunnerExtension
	{
		/*
		 public void Run<TS, T0>() where TS : struct, ISystem<T0>
		{
			Archetype archetype = GetOrCreateArchetype(typeof(T0));
			var system = new TS();
			for (int i = 0; i < archetype.Size; i++)
			{
				system.Update(ref archetype.Get<T0>(i));
			}
		}
		 */
		public static StringBuilder CreateSystemRunners(this StringBuilder sb, int amount)
		{
			sb.AppendLine("using SosoEcs.Systems;");
			sb.AppendLine("using SosoEcs.Components.Core;");
			sb.AppendLine("using SosoEcs.Queries;");
			sb.AppendLine("namespace SosoEcs;");
			sb.AppendLine("public partial class World");
			sb.AppendLine("{");
			StringBuilder interfaceGenerics = new StringBuilder();
			StringBuilder archetypeGetGenerics = new StringBuilder();
			for (int i = 0; i < amount; i++)
			{
				string generic = "T" + i;
				interfaceGenerics.Append(generic);
				archetypeGetGenerics.Append($"typeof({generic})");
				
				StringBuilder updateGenerics = new StringBuilder();
				for (int j = 0; j <= i; j++)
				{
					updateGenerics.Append($"ref archetype.Get<T{j}>({i})");
					if (j < i) updateGenerics.Append(", ");
				}
				sb.AppendLine($"{Helpers.TAB}public void Run<TS, {interfaceGenerics.ToString()}>() where TS : struct, ISystem<{interfaceGenerics.ToString()}>");
				sb.AppendLine($"{Helpers.TAB}{{");
				sb.AppendLine($"{Helpers.TAB}{Helpers.TAB}var system = new TS();");
				sb.AppendLine($"{Helpers.TAB}{Helpers.TAB}Archetype archetype = GetOrCreateArchetype({archetypeGetGenerics.ToString()});");
				sb.AppendLine($"{Helpers.TAB}{Helpers.TAB}for (int i = 0; i < archetype.Size; i++)");
				sb.AppendLine($"{Helpers.TAB}{Helpers.TAB}{{");
				sb.AppendLine($"{Helpers.TAB}{Helpers.TAB}{Helpers.TAB}system.Update({updateGenerics.ToString()});");
				sb.AppendLine($"{Helpers.TAB}{Helpers.TAB}}}");
				sb.AppendLine($"{Helpers.TAB}}}");
				

				interfaceGenerics.Append(", ");
				archetypeGetGenerics.Append(", ");
			}
			
			sb.AppendLine("}");
			return sb;
		}
	}
}
