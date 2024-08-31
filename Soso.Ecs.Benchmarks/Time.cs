using Raylib_cs;

namespace Soso.Ecs.Benchmarks
{
	public static class Time
	{
		public static float Dt;

		public static void Update() => Dt = Raylib.GetFrameTime();
	}
}
