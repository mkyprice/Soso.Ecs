using Raylib_cs;
using SosoEcs.Benchmarks.BadEcs;
using SosoEcs.Benchmarks.Extensions;
using System.Numerics;

namespace SosoEcs.Benchmarks
{
	public class SlowEcsDemoWindow : Window
	{
		

		class Physics : ISystem
		{
			public override void Update()
			{
				foreach (var id in _ecs.EntitiesWith(typeof(Transform), typeof(RigidBody)))
				{
					ref var t0 = ref _ecs.GetComponent<Transform>(id);
					ref var t1 = ref _ecs.GetComponent<RigidBody>(id);
					t0.Position += t1.Velocity * Time.Dt;
					if (t0.Position.X < 0 || t0.Position.X > EntitiesWindow.Width) t1.Velocity.X *= -1;
					if (t0.Position.Y < 0 || t0.Position.Y > EntitiesWindow.Height) t1.Velocity.Y *= -1;
				}
			}
		}

		class Renderer : ISystem
		{
			public override void Render()
			{
				foreach (var id in _ecs.EntitiesWith(typeof(Transform), typeof(RectShape2D)))
				{
					ref var t0 = ref _ecs.GetComponent<Transform>(id);
					ref var t1 = ref _ecs.GetComponent<RectShape2D>(id);
					Raylib.DrawRectangle((int)t0.Position.X, (int)t0.Position.Y, t1.Width, t1.Height, t1.Tint);
				}
			}
		}
		private static SlowEcsDemo _ecs;
		
		public SlowEcsDemoWindow() : base("Slow ECS")
		{
		}
		protected override void Load()
		{
			_ecs = new SlowEcsDemo();

			for (int i = 0; i < 10_000; i++)
			{
				int entity = _ecs.CreateEntity(
					);
				_ecs.AddComponent(entity, new Transform()
				{
					Position = new Vector2(Random.Shared.Next(Width), Random.Shared.Next(Height))
				});
				_ecs.AddComponent(entity, new RectShape2D()
				{
					Width = 16,
					Height = 16,
					Tint = ColorExtension.GetRandomColor()
				});
				_ecs.AddComponent(entity, new RigidBody()
				{
					Velocity = new Vector2(Random.Shared.NextSingle(), Random.Shared.NextSingle()) * 256 - new Vector2(128)
				});
			}

			_ecs.AddSystem(new Physics());
			_ecs.AddSystem(new Renderer());
		}
		protected override void Unload()
		{
		}
		protected override void Update()
		{
			_ecs.Update();
		}
		protected override void Render()
		{
			_ecs.Draw();
		}
	}
}
