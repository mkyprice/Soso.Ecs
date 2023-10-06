using SosoEcs.Tests.Components;
using SosoEcs.Tests.Systems;

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

	[Test]
	public void SystemsTest0()
	{
		World world = new World();
		for (int i = 0; i < 100; i++)
		{
			Entity entity = world.CreateEntity(new TestCompA()
			{
				Value = $"Hello{i}"
			});
		
			entity.Set(new TestCompB()
			{
				Value = "World"
			});
		}
		
		world.Run<TestSystemA, TestCompA, TestCompB>();
	}

	[Test]
	public void ComponentSystemNotFound()
	{
		World world = new World();
		world.CreateEntity(new TestCompA()
		{
			Value = $"Hello"
		});
		
		world.Run<TestSystemA, TestCompA, TestCompB>();
	}
}
