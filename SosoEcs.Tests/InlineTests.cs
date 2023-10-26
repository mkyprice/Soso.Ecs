using SosoEcs.Tests.Components;

namespace SosoEcs.Tests
{
	public class InlineTests
	{
		[Test]
		public void InlineSystem()
		{
			EcsWorld ecsWorld = new EcsWorld();

			Entity a = new Entity(ecsWorld);
			a.Set(new TestCompA()
			{
				Value = string.Empty
			});
			
			ecsWorld.Run((ref TestCompA a) =>
			{
				a.Value = "Hello";
			});
			
			Assert.That(a.Get<TestCompA>().Value, Is.EqualTo("Hello"));
		}
		
		[Test]
		public void InlineEntitySystem()
		{
			EcsWorld ecsWorld = new EcsWorld();

			Entity a = new Entity(ecsWorld);
			a.Set(new TestCompA()
			{
				Value = string.Empty
			});
			
			ecsWorld.Run((Entity entity, ref TestCompA comp) =>
			{
				Assert.That(entity, Is.EqualTo(a));
				comp.Value = "Hello";
			});
			
			Assert.That(a.Get<TestCompA>().Value, Is.EqualTo("Hello"), "Failed to component");
		}
	}
}
