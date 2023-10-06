
using SosoEcs;
using SosoEcs.Systems;
using System.Numerics;

class Program
{
	struct Transform
	{
		public Vector2 Position;
	}
	struct RigidBody
	{
		public Vector2 Velocity;
	}

	struct Physics : ISystem<Transform, RigidBody>
	{
		public void Update(ref Transform t0, ref RigidBody t1)
		{
			t0.Position += t1.Velocity;
			Console.WriteLine(t0.Position);
		}
	}
	
	static void Main(string[] args)
	{
		World ecs = new World();

		for (int i = 0; i < 10; i++)
		{
			var entity = ecs.CreateEntity();
			entity.Set(new Transform());
			entity.Set(new RigidBody()
			{
				Velocity = new Vector2(Random.Shared.NextSingle(), Random.Shared.NextSingle())
			});
		}

		bool quit = false;
		while (quit == false)
		{
			ecs.Run<Physics, Transform, RigidBody>();

			if (Console.KeyAvailable)
			{
				quit = Console.ReadKey().Key == ConsoleKey.Q;
			}
		}
	}
}