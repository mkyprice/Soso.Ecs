using SosoEcs.Tests.Components;

namespace SosoEcs.Tests
{
	public class InlineTests
	{
		[Test]
		public void InlineSystem()
		{
			World world = new World();

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
			World world = new World();

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
