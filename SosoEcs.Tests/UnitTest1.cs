using SosoEcs.Tests.Components;
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
		Entity entity = World.CreateEntity(new TestComp()
		{
			Value = "Hello"
		});

		ref TestComp c = ref entity.Get<TestComp>();
		Console.WriteLine(c.Value);
		c.Value = "World";
		Console.WriteLine(entity.Get<TestComp>().Value);
	}
}
