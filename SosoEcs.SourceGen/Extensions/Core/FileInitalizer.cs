using System.Text;

namespace SosoEcs.SourceGen.Extensions.Core
{
	public static class FileInitalizer
	{
		public static StringBuilder Init()
		{
			StringBuilder sb = new StringBuilder()
				.AppendTopComments();

			return sb;
		}
	}
}
