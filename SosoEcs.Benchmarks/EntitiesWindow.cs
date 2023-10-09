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
	struct RectShape2D
	{
		public int Width, Height;
		public Color Tint;
		public float Z;
	}
	struct CircleShape2D
	{
		public int Radius;
		public Color Tint;
		public float Z;
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

	struct Renderer : ISystem<Transform, RectShape2D>, ISystem<Transform, CircleShape2D>
	{
		private static PriorityQueue<Action, float> _drawQueue = new PriorityQueue<Action, float>(10_000);
		public Renderer()
		{
		}
		public void Update(ref Transform t0, ref RectShape2D t1)
		{
			Vector2 pos = t0.Position;
			int width = t1.Width;
			int height = t1.Height;
			Color tint = t1.Tint;
			float z = t1.Z;
			Raylib.DrawRectangle((int)pos.X, (int)pos.Y, width, height, tint);
			// _drawQueue.Enqueue(() => Raylib.DrawRectangle((int)pos.X, (int)pos.Y, width, height, tint), z);
		}
		public void Update(ref Transform t0, ref CircleShape2D t1)
		{
			Vector2 pos = t0.Position;
			float radius = t1.Radius;
			Color tint = t1.Tint;
			float z = t1.Z;
			Raylib.DrawCircle((int)pos.X, (int)pos.Y, radius, tint);
			// _drawQueue.Enqueue(() => Raylib.DrawCircle((int)pos.X, (int)pos.Y, radius, tint), z);
		}

		public void Draw()
		{
			while (_drawQueue.Count > 0)
			{
				_drawQueue.Dequeue()();
			}
		}
	}
	
	public class EntitiesWindow : Window
	{
		private World Ecs;
		private List<Entity> _entities = new List<Entity>(100_000);
		public EntitiesWindow() : base("Graphical Benchmark")
		{
		}
		protected override void Load()
		{
			Ecs = new World();

			for (int i = 0; i < _entities.Capacity; i++)
			{
				_entities.Add(Ecs.CreateEntity(
					new Transform()
					{
						Position = new Vector2(Random.Shared.Next(Width), Random.Shared.Next(Height))
					}
					,new RigidBody()
					{
						Velocity = new Vector2(Random.Shared.NextSingle(), Random.Shared.NextSingle()) * 256 - new Vector2(128)
					}));
				_entities[^1].Set(new RectShape2D()
				{
					Width = 16,
					Height = 16,
					Tint = ColorExtension.GetRandomColor(),
					Z = i
				});
				// if (i % 2 == 0)
				// {
				// 	_entities[^1].Set(new RectShape2D()
				// 	{
				// 		Width = 16,
				// 		Height = 16,
				// 		Tint = ColorExtension.GetRandomColor(),
				// 		Z = i
				// 	});
				// }
				// else
				// {
				// 	_entities[^1].Set(new CircleShape2D()
				// 	{
				// 		Radius = 4,
				// 		Tint = ColorExtension.GetRandomColor(),
				// 		Z = i
				// 	});
				// }
			}
		}
		protected override void Unload()
		{
		}
		protected override void Update()
		{
			Ecs.RunParallel<Physics, Transform, RigidBody>();
		}
		
		protected override void Render()
		{
			Renderer renderer = new Renderer();
			Ecs.Run<Renderer, Transform, RectShape2D>(renderer);
			Ecs.Run<Renderer, Transform, CircleShape2D>(renderer);
			// renderer.Draw();
		}
	}
}
