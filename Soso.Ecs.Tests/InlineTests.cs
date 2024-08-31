using Soso.Ecs.Tests.Components;

namespace Soso.Ecs.Tests
{
	public class InlineTests
	{
		[Test]
		public void InlineSystem()
		{
			EcsWorld world = new EcsWorld();

			Entity a = new Entity(world);
			a.Set(new TestCompA()
			{
				Value = string.Empty
			});
			
			world.Run((ref TestCompA a) =>
			{
				a.Value = "Hello";
			});
			
			Assert.That(a.Get<TestCompA>().Value, Is.EqualTo("Hello"));
		}
		
		[Test]
		public void InlineEntitySystem()
		{
			EcsWorld world = new EcsWorld();

			Entity a = new Entity(world);
			a.Set(new TestCompA()
			{
				Value = string.Empty
			});
			
			world.Run((Entity entity, ref TestCompA comp) =>
			{
				Assert.That(entity, Is.EqualTo(a));
				comp.Value = "Hello";
			});
			
			Assert.That(a.Get<TestCompA>().Value, Is.EqualTo("Hello"), "Failed to component");
		}
	}
}
