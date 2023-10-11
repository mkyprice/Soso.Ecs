using SosoEcs.SourceGen.Extensions.Systems;
using System.Text;

namespace SosoEcs.SourceGen.Extensions.Inline
{
	public static class InlineSystemRunner
	{
		public static StringBuilder AppendInlineSystemRunner(this StringBuilder sb, int quantity)
		{
			const string PARAM_NAME = "request";
			StringBuilder generics = new StringBuilder();
			for (int i = 0; i < quantity; i++)
			{
				string generic = "T" + i;
				generics.Append(generic);
				sb.Append($"public void RunInline<{generics}>({InlineDelegates.CLASS}.{InlineDelegates.INLINE_REQUEST}<{generics}> {PARAM_NAME})");
				sb.Append("{");

				sb.AppendSystemRunnerLoop(i, PARAM_NAME, false, false);

				sb.Append("}");

				generics.Append(", ");
			}
			return sb;
		}
	}
}
