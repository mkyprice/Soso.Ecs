using Raylib_cs;

namespace Soso.Ecs.Benchmarks.Extensions
{
	public static class ColorExtension
	{
		private static readonly Color[] RandomColors = new []{ Color.RED, Color.BLUE, Color.GOLD, Color.GREEN, };
		public static Color GetRandomColor() => RandomColors[Random.Shared.Next(RandomColors.Length)];
	}
}
