using System.Text;
using Soso.Ecs.SourceGen.Extensions.Core;

namespace Soso.Ecs.SourceGen.Extensions.Systems
{
	public static class ISystemExtension
	{
		public static StringBuilder AppendSystemsNamespace(this StringBuilder sb)
		{
			sb.AppendLine($"namespace {Namespaces.ISYSTEMS};");
			return sb;
		}

		public static StringBuilder AppendISystemName(this StringBuilder sb, bool entitySystem)
		{
			sb.Append("ISystem");
			if (entitySystem)
			{
				sb.Append("Entity");
			}
			return sb;
		}
		
		public static StringBuilder AppendSystems(this StringBuilder sb, int amount, bool entity)
		{
			string interfaceGenerics = string.Empty;
			string updateGenerics = string.Empty;
			for (int i = 0; i < amount; i++)
			{
				string generic = "T" + i;
				if (i > 0)
				{
					interfaceGenerics += ", ";
					updateGenerics += ", ";
				}
				interfaceGenerics += generic;
				sb.Append("public interface ").AppendISystemName(entity);
				sb.Append($"<{interfaceGenerics}>");
				sb.AppendLine("{");
				
				// Update
				sb.AppendLine($"public void Update(");
				if (entity)
				{
					sb.Append("in Entity entity, ");
				}
				sb.Append($"{updateGenerics}ref {generic} t{i}");
				sb.Append(");");
				
				sb.AppendLine("}");
				updateGenerics += $"ref {generic} t{i}";
			}
			return sb;
		}
	}
}
