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
	
	public class EntitiesWindow : Window
	{
		private EcsWorld Ecs;
		private List<Entity> _entities = new List<Entity>(100_000);
		public EntitiesWindow() : base("Graphical Benchmark")
		{
		}
		protected override void Load()
		{
			Ecs = new EcsWorld();

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
			Ecs.Run((ref Transform t, ref RectShape2D rect) =>
			{
				Raylib.DrawRectangle((int)t.Position.X, (int)t.Position.Y, rect.Width, rect.Height, rect.Tint);
			});
		}
	}
}
