using Raylib_cs;

namespace SosoEcs.Benchmarks
{
	public static class Time
	{
		public static float Dt;

		public static void Update() => Dt = Raylib.GetFrameTime();
	}
}
