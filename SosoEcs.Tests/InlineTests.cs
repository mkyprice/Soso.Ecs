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
			
			world.RunInline((ref TestCompA a) =>
			{
				a.Value = "Hello";
			});
			
			Assert.That(a.Get<TestCompA>().Value, Is.EqualTo("Hello"));
		}
	}
}
