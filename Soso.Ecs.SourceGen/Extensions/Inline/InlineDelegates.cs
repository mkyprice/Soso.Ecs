using System.Text;
using Soso.Ecs.SourceGen.Extensions.Core;

namespace Soso.Ecs.SourceGen.Extensions.Inline
{
	public static class InlineDelegates
	{
		public static readonly string INLINE_REQUEST = "InlineRequest";
		public static readonly string INLINE_REQUEST_ENTITY = INLINE_REQUEST + "Entity";
		public static readonly string CLASS = "Requests";

		public static StringBuilder AppendQueryNamespace(this StringBuilder sb)
		{
			sb.AppendLine($"namespace {Namespaces.INLINES};");
			return sb;
		}
		
		public static StringBuilder AppendQueries(this StringBuilder sb, bool entity, int count)
		{
			StringBuilder generics = new StringBuilder();
			StringBuilder args = new StringBuilder();

			string name = entity ? INLINE_REQUEST_ENTITY : INLINE_REQUEST;
			if (entity)
			{
				args.Append("Entity entity, ");
			}
			
			for (int i = 0; i < count; i++)
			{
				string generic = "T" + i;
				generics.Append(generic);
				args.Append($"ref {generic} t{i}");
				sb.Append($"public delegate void {name}<{generics}>({args});");
				generics.Append(", ");
				args.Append(", ");
			}
			return sb;
		}
	}
}
