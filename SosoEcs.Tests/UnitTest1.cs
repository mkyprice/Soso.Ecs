using SosoEcs.Systems;
using SosoEcs.Tests.Components;

namespace SosoEcs.Tests;

public class Tests
{
	[Test]
	public void SetValue()
	{
		World world = new World();
		Entity entity = world.CreateEntity(new TestCompA()
		{
			Value = "Hello"
		});

		ref TestCompA c = ref entity.Get<TestCompA>();
		Assert.IsTrue(c.Value.Equals("Hello"));
		c.Value = "World";
		Assert.IsTrue(entity.Get<TestCompA>().Value.Equals("World"));
	}

	[Test]
	public void MultipleComponents()
	{
		World world = new World();
		Entity entity = world.CreateEntity(new TestCompA()
		{
			Value = "Hello"
		});

		entity.Set(new TestCompB()
		{
			Value = "World"
		});
		
		ref TestCompA c = ref entity.Get<TestCompA>();
		Assert.IsTrue(c.Value.Equals("Hello"));
		Assert.IsTrue(entity.Get<TestCompB>().Value.Equals("World"));
	}

	[Test]
	public void RemoveTest()
	{
		World world = new World();
		Entity entity = world.CreateEntity(new TestCompA()
		{
			Value = "Hello"
		});

		entity.Set(new TestCompB()
		{
			Value = "World"
		});
		
		ref TestCompA c = ref entity.Get<TestCompA>();
		Assert.IsTrue(c.Value.Equals("Hello"));
		Assert.IsTrue(entity.Get<TestCompB>().Value.Equals("World"));

		entity.Remove<TestCompA>();
		
		Assert.IsFalse(entity.Contains<TestCompA>());
		Assert.IsTrue(entity.Contains<TestCompB>());
	}

	struct CountingSystem : ISystem<TestCompA, TestCompB>
	{
		public void Update(ref TestCompA t0, ref TestCompB t1)
		{
			t0.Number += t1.Number;
		}
	}
	[Test]
	public void CountingSystemsTest()
	{
		World world = new World();
		List<Entity> entities = new List<Entity>();
		for (int i = 0; i < 100; i++)
		{
			Entity entity = world.CreateEntity(new TestCompA()
			{
				Number = 0
			});
		
			entity.Set(new TestCompB()
			{
				Number = i
			});
			entities.Add(entity);
		}
		
		world.Run<CountingSystem, TestCompA, TestCompB>();

		for (int i = 0; i < entities.Count; i++)
		{
			Assert.IsTrue(i == entities[i].Get<TestCompA>().Number, "System did not assign value {0}", i);
		}
	}


	struct ComponentNotFound : ISystem<TestCompA, TestCompB>
	{
		public void Update(ref TestCompA t0, ref TestCompB t1)
		{
			Assert.IsFalse(true, "Update was hit");
		}
	}
	[Test]
	public void ComponentSystemNotFound()
	{
		World world = new World();
		world.CreateEntity(new TestCompA()
		{
			Value = $"Hello"
		});
		
		world.Run<ComponentNotFound, TestCompA, TestCompB>();
	}
}
