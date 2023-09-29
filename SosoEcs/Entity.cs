﻿namespace SosoEcs
{
	public readonly struct Entity
	{
		public readonly int Id;
		public readonly World World;
		
		private static int _nextId = 1;

		public Entity(World world)
		{
			Id = _nextId;
			_nextId++;
			World = world;
		}

		#region Component Helpers

		public void Set<T>(T component) => World.SetComponents(this, component);
		public ref T Get<T>() => ref World.GetComponent<T>(this);
		// public bool Contains<T>() => World.Library.Contains<T>(this);
		// public bool Remove<T>() => World.Library.Remove<T>(this);

		#endregion

		#region Overrides
		
		public override bool Equals(object? obj) => obj is Entity e && e.Id == this.Id;
		public override int GetHashCode() => Id;
		public override string ToString()
		{
			return $"{base.ToString()} ID: {Id}";
		}

		public static bool operator ==(Entity lhs, Entity rhs) => lhs.Equals(rhs);
		public static bool operator !=(Entity lhs, Entity rhs) => !lhs.Equals(rhs);

		#endregion
	}
}
