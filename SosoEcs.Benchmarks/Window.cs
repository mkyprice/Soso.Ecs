using Raylib_cs;

namespace SosoEcs.Benchmarks
{
	public abstract class Window
	{
		public Window(int width, int height, string title)
		{
			Raylib.InitWindow(width, height, title);
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
