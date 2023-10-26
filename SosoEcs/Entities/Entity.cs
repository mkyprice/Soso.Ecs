namespace SosoEcs
{
	public readonly struct Entity
	{
		public readonly int Id;
		public readonly EcsWorld EcsWorld;
		
		private static int _nextId = 1;

		public Entity(EcsWorld ecsWorld)
		{
			Id = _nextId;
			_nextId++;
			EcsWorld = ecsWorld;
			EcsWorld.CreateEntity(this);
		}

		#region Component Helpers

		public Entity Set<T>(T component)
		{
			EcsWorld.SetComponent(this, component);
			return this;
		}
		public ref T Get<T>() => ref EcsWorld.GetComponent<T>(this);
		public bool Contains<T>() => EcsWorld.Contains<T>(this);
		public void Remove<T>() => EcsWorld.Remove<T>(this);

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
