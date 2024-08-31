namespace Soso.Ecs.Tests.Components
{
	public class BaseTestComp
	{
		public virtual string GetName()
		{
			return nameof(BaseTestComp);
		}
	}

	public class InheritanceComp : BaseTestComp
	{
		public override string GetName()
		{
			return nameof(InheritanceComp);
		}
	}
}
