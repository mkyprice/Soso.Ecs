using SosoEcs.Systems;
using SosoEcs.Tests.Components;

namespace SosoEcs.Tests.Systems
{
	public readonly struct TestSystemA : ISystem<TestCompA, TestCompB>
	{
		public void Update(ref TestCompA t0, ref TestCompB t1)
		{
			Console.WriteLine($"{t0.Value} + {t1.Value}");
		}
	}
}
