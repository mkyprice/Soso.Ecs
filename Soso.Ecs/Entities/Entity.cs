namespace Soso.Ecs
{
	public readonly struct Entity
	{
		public readonly int Id;
		public readonly EcsWorld World;
		
		private static int _nextId = 1;

		public Entity(EcsWorld world)
		{
			Id = _nextId;
			_nextId++;
			World = world;
			World.CreateEntity(this);
		}

		#region Component Helpers

		public Entity Set<T>(T component)
		{
			World.SetComponent(this, (T)component);
			return this;
		}
		public ref T Get<T>() => ref World.GetComponent<T>(this);
		public bool Contains<T>() => World.Contains<T>(this);
		public void Remove<T>() => World.Remove<T>(this);

		#endregion

		#region Overrides
		
		public override string ToString() => $"{base.ToString()} ID: {Id}";
		public override int GetHashCode() => Id;
		public override bool Equals(object? obj) => obj is Entity e && e == this;
		public static bool operator ==(Entity lhs, Entity rhs) => lhs.Id == rhs.Id;
		public static bool operator !=(Entity lhs, Entity rhs) => lhs.Id != rhs.Id;

		#endregion
	}
}
