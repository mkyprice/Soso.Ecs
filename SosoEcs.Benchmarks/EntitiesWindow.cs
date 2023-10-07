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
	}
	struct CircleShape2D
	{
		public int Radius;
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

	struct RectRenderer : ISystem<Transform, RectShape2D>
	{
		public void Update(ref Transform t0, ref RectShape2D t1)
		{
			Raylib.DrawRectangle((int)t0.Position.X, (int)t0.Position.Y, t1.Width, t1.Height, t1.Tint);
		}
	}

	struct CircleRenderer : ISystem<Transform, CircleShape2D>
	{
		public void Update(ref Transform t0, ref CircleShape2D t1)
		{
			Raylib.DrawCircle((int)t0.Position.X, (int)t0.Position.Y, t1.Radius, t1.Tint);
		}
	}
	
	public class EntitiesWindow : Window
	{
		private World Ecs;
		private List<Entity> _entities = new List<Entity>(10_000);
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
				if (i % 2 == 0)
				{
					_entities[^1].Set(new RectShape2D()
					{
						Width = 16,
						Height = 16,
						Tint = ColorExtension.GetRandomColor()
					});
				}
				else
				{
					_entities[^1].Set(new CircleShape2D()
					{
						Radius = 16,
						Tint = ColorExtension.GetRandomColor()
					});
				}
			}
		}
		protected override void Unload()
		{
		}
		protected override void Update()
		{
			Ecs.ParallelRun<Physics, Transform, RigidBody>();
			
			// if (_entities.Count > 0)
			// {
			// 	int index = Random.Shared.Next(_entities.Count);
			// 	Ecs.Destroy(_entities[index]);
			// 	_entities.RemoveAt(index);
			// }
		}
		protected override void Render()
		{
			Ecs.Run<RectRenderer, Transform, RectShape2D>();
			Ecs.Run<CircleRenderer, Transform, CircleShape2D>();
		}
	}
}
