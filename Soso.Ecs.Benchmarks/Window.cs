using Raylib_cs;

namespace Soso.Ecs.Benchmarks
{
	public abstract class Window
	{
		public static readonly int Width = 1280, Height = 720;
		public Window(string title)
		{
			Raylib.InitWindow(Width, Height, title);
		}

		public void Run()
		{
			Load();
			while (Raylib.WindowShouldClose() == false)
			{
				Time.Update();
				Update();
				
				Raylib.BeginDrawing();
				Raylib.ClearBackground(Color.BLACK);
				Render();
				Raylib.DrawFPS(32, 32);
				Raylib.EndDrawing();
			}

			Unload();
			Raylib.CloseWindow();
		}

		protected abstract void Load();
		protected abstract void Unload();
		protected abstract void Update();
		protected abstract void Render();
	}
}
