using SosoEcs.SourceGen.Extensions.Core;
using System.Text;

namespace SosoEcs.SourceGen.Extensions.Query
{
	public static class Queries
	{
		public static readonly string INLINE_REQUEST = "InlineRequest";

		public static StringBuilder AppendQueryNamespace(this StringBuilder sb)
		{
			sb.AppendLine($"namespace {Namespaces.BASE}.Queries;");
			return sb;
		}
		
		public static StringBuilder AppendQueries(this StringBuilder sb, int count)
		{
			StringBuilder generics = new StringBuilder();
			StringBuilder args = new StringBuilder();
			
			for (int i = 0; i < count; i++)
			{
				//public delegate void Inline<T0>(ref T0 t0);
				string generic = "T" + i;
				generics.Append(generic);
				args.Append($"ref {generic} t{i}");
				sb.Append($"public delegate void {INLINE_REQUEST}<{generics}>({args});");
				generics.Append(", ");
				args.Append(", ");
			}
			return sb;
		}
	}
}
