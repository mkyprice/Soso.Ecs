using SosoEcs.SourceGen.Extensions.Systems;
using System.Text;

namespace SosoEcs.SourceGen.Extensions.Query
{
	public static class QuerySystemRunner
	{
		public static StringBuilder AppendQuerySystemRunner(this StringBuilder sb, int quantity)
		{
			StringBuilder generics = new StringBuilder();
			for (int i = 0; i < quantity; i++)
			{
				//foreach (var archetype in GetArchetypes(typeof(T0)))
				// {
				// T0[] t0s = archetype.GetArray<T0>();
				// for (int i = 0; i < archetype.Size; i++)
				// {
				// system.Update(ref t0s[i]);
				// }
				// }
				string generic = "T" + i;
				generics.Append(generic);
				sb.Append($"public void InlineQuery<{generics}>(Query.{Queries.INLINE_REQUEST}<{generics}> query)");
				sb.Append("{");

				sb.AppendSystemRunnerLoop(i, "query", false, false);

				sb.Append("}");

				generics.Append(", ");
			}
			return sb;
		}
	}
}
