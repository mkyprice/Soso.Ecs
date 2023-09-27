using System.Runtime.CompilerServices;

namespace SosoEcs.Components.Core
{
	public class ComponentLibrary
	{
		private readonly Dictionary<Type, IComponents> _components = new Dictionary<Type, IComponents>();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Set<T>(Entity entity, T component) => GetCollection<T>().Set(entity, component);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ref T Get<T>(Entity entity) => ref GetCollection<T>().GetComponent(entity);
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Contains<T>(Entity entity) => GetCollection<T>().Contains(entity);
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Remove<T>(Entity entity) => GetCollection<T>().Remove(entity);

		public IEnumerable<Entity> GetEntities<T>() => GetCollection<T>().GetEntities();
		public IEnumerable<Entity> GetEntities(Type type) => GetCollection(type).GetEntities();
		
		public void Remove(Entity entity)
		{
			foreach (IComponents components in _components.Values)
			{
				components.Remove(entity);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private Components<T> GetCollection<T>()
		{
			Type type = typeof(T);
			IComponents components;
			if (_components.TryGetValue(type, out components) == false)
			{
				components = new Components<T>();
				_components[type] = components;
			}
			return components as Components<T>;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private IComponents GetCollection(Type type)
		{
			if (_components.TryGetValue(type, out IComponents components))
			{
				return components;
			}
			throw new Exception($"Cannot get non-existant collection {type.Name}");
		}
	}
}
