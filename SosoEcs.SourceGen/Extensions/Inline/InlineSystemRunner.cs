using SosoEcs.SourceGen.Extensions.Systems;
using System.Text;

namespace SosoEcs.SourceGen.Extensions.Inline
{
	public static class InlineSystemRunner
	{
		public static StringBuilder AppendInlineSystemRunner(this StringBuilder sb, bool entity, int quantity)
		{
			const string PARAM_NAME = "request";
			StringBuilder generics = new StringBuilder();

			string name = entity ? InlineDelegates.INLINE_REQUEST_ENTITY : InlineDelegates.INLINE_REQUEST;
			
			for (int i = 0; i < quantity; i++)
			{
				string generic = "T" + i;
				generics.Append(generic);
				sb.Append($"public void Run<{generics}>({InlineDelegates.CLASS}.{name}<{generics}> {PARAM_NAME})");
				sb.Append("{");

				sb.AppendSystemRunnerLoop(i, PARAM_NAME, false, entity);

				sb.Append("}");

				generics.Append(", ");
			}
			return sb;
		}
	}
}
