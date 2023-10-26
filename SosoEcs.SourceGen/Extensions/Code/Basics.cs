using System.Text;

namespace SosoEcs.SourceGen.Extensions.Code
{
	public static class Basics
	{
		public static StringBuilder AppendFor(this StringBuilder sb, string indexer, string length)
		{
			sb.AppendLine($"for (int {indexer} = 0; {indexer} < {length}; {indexer}++)");
			return sb;
		}
	}
}
