using SosoEcs.Tests.Components;
using SosoEcs.Tests.Systems;
using System.Diagnostics;

namespace SosoEcs.Tests;

public class Tests
{
	private World World;
	
	[SetUp]
	public void Setup()
	{
		World = new World();
	}

	[Test]
	public void Test1()
	{
		Entity entity = World.CreateEntity(new TestCompA()
		{
			Value = "Hello"
		});

		ref TestCompA c = ref entity.Get<TestCompA>();
		Assert.IsTrue(c.Value.Equals("Hello"));
		c.Value = "World";
		Assert.IsTrue(entity.Get<TestCompA>().Value.Equals("World"));
	}

	[Test]
	public void Test2()
	{
		Entity entity = World.CreateEntity(new TestCompA()
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
	public void SystemsTest0()
	{
		Entity entity = World.CreateEntity(new TestCompA()
		{
			Value = "Hello"
		});
		
		entity.Set(new TestCompB()
		{
			Value = "World"
		});
		
		World.Run<TestSystemA, TestCompA, TestCompB>();
	}
}
