namespace Soso.Ecs.Benchmarks.BadEcs
{
	public abstract class ISystem
	{
		public bool IsPausable { get; set; } = true;
		public virtual void PreUpdate() { }
		public virtual void Update() { }
		public virtual void PostUpdate() { }
		public virtual void Render() { }

		public override bool Equals(object? obj)
		{
			return obj?.GetType() == GetType();
		}

		public override int GetHashCode()
		{
			return GetType().Name.GetHashCode();
		}
	}
}
