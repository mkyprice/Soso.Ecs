using SosoEcs.Systems;
using SosoEcs.Tests.Components;

namespace SosoEcs.Tests;

public class Tests
{
	[Test]
	public void SetValue()
	{
		EcsWorld world = new EcsWorld();
		Entity entity = world.CreateEntity(new TestCompA()
		{
			Value = "Hello"
		});

		ref TestCompA c = ref entity.Get<TestCompA>();
		Assert.That(c.Value, Is.EqualTo("Hello"));
		c.Value = "World";
		Assert.That(entity.Get<TestCompA>().Value, Is.EqualTo("World"));
	}

	[Test]
	public void MultipleComponents()
    {
        EcsWorld world = new EcsWorld();
		Entity entity = world.CreateEntity(new TestCompA()
		{
			Value = "Hello"
		});

		entity.Set(new TestCompB()
		{
			Value = "World"
		});
		
		TestCompA c = entity.Get<TestCompA>();
        Assert.Multiple(() =>
        {
            Assert.That(c.Value, Is.EqualTo("Hello"));
            Assert.That(entity.Get<TestCompB>().Value, Is.EqualTo("World"));
        });
    }

    [Test]
	public void RemoveTest()
	{
		EcsWorld world = new EcsWorld();
		Entity entity = world.CreateEntity(new TestCompA()
		{
			Value = "Hello"
		});

		entity.Set(new TestCompB()
		{
			Value = "World"
		});
		
		ref TestCompA c = ref entity.Get<TestCompA>();
		Assert.That(c.Value, Is.EqualTo("Hello"));
		Assert.That(entity.Get<TestCompB>().Value, Is.EqualTo("World"));

		entity.Remove<TestCompA>();
		
		Assert.That(entity.Contains<TestCompA>(), Is.False);
		Assert.That(entity.Contains<TestCompB>(), Is.True);
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
		EcsWorld world = new EcsWorld();
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
			Assert.That(i, Is.EqualTo(entities[i].Get<TestCompA>().Number), "System did not assign value {0}", i);
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
		EcsWorld ecsWorld = new EcsWorld();
		ecsWorld.CreateEntity(new TestCompA()
		{
			Value = $"Hello"
		});
		
		ecsWorld.Run<ComponentNotFound, TestCompA, TestCompB>();
	}
}
