using Raylib_cs;
using SosoEcs.Benchmarks.Extensions;
using SosoEcs.Systems;
using System.Numerics;

namespace SosoEcs.Benchmarks
{
	struct Transform
	{
		public Vector2 Position;
	}
	struct RigidBody
	{
		public Vector2 Velocity;
	}
	struct Shape2D
	{
		public int Width, Height;
		public Color Tint;
	}

	struct Physics : ISystem<Transform, RigidBody>
	{
		public void Update(ref Transform t0, ref RigidBody t1)
		{
			t0.Position += t1.Velocity * Time.Dt;
			if (t0.Position.X < 0 || t0.Position.X > EntitiesWindow.Width) t1.Velocity.X *= -1;
			if (t0.Position.Y < 0 || t0.Position.Y > EntitiesWindow.Height) t1.Velocity.Y *= -1;
		}
	}

	struct Renderer : ISystem<Transform, Shape2D>
	{
		public void Update(ref Transform t0, ref Shape2D t1)
		{
			Raylib.DrawRectangle((int)t0.Position.X, (int)t0.Position.Y, t1.Width, t1.Height, t1.Tint);
		}
	}
	
	public class EntitiesWindow : Window
	{
		private World Ecs;
		public static readonly int Width = 1280, Height = 720;
		public EntitiesWindow() : base(Width, Height, "Graphical Benchmark")
		{
		}
		protected override void Load()
		{
			Ecs = new World();

			for (int i = 0; i < 500000; i++)
			{
				Ecs.CreateEntity(
					new Transform()
					{
						Position = new Vector2(Random.Shared.Next(720))
					},
					new Shape2D()
					{
						Width = 16,
						Height = 16,
						Tint = ColorExtension.GetRandomColor()
					},new RigidBody()
					{
						Velocity = new Vector2(Random.Shared.NextSingle(), Random.Shared.NextSingle()) * 128
					});
			}
		}
		protected override void Unload()
		{
		}
		protected override void Update()
		{
			Ecs.ParallelRun<Physics, Transform, RigidBody>();
		}
		protected override void Render()
		{
			Ecs.Run<Renderer, Transform, Shape2D>();
		}
	}
}
