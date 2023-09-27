using System.Runtime.CompilerServices;

namespace SosoEcs.Components.Core
{
	public class Components<T> : IComponents
	{
		public int Count => _count;
		
		private int _count = 0;
		private T[] _components = new T[128];
		private readonly Dictionary<Entity, int> _entityComponentMap = new Dictionary<Entity, int>();

		public IEnumerable<Entity> GetEntities() => _entityComponentMap.Keys;

		public void Set(Entity entity, T component)
		{
			if (_entityComponentMap.TryGetValue(entity, out int index))
			{
				_components[index] = component;
				return;
			}

			_entityComponentMap[entity] = _count;
			_components[_count] = component;

			_count++;
			if (_components.Length <= _count)
			{
				T[] tmp = new T[_components.Length * 2];
				Buffer.BlockCopy(_components, 0, tmp, 0, _components.Length);
				_components = tmp;
				Console.WriteLine("Expanded component list {0} to {1}", typeof(T), _components.Length * 2);
			}
		}

		public bool Remove(Entity entity)
		{
			if (_entityComponentMap.TryGetValue(entity, out int index))
			{
				_entityComponentMap.Remove(entity);
				_components[index] = _components[_count - 1];
				_count--;
				return true;
			}
			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Contains(Entity entity) => _entityComponentMap.ContainsKey(entity);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ref T GetComponent(Entity entity)
		{
			if (_entityComponentMap.TryGetValue(entity, out int index) == false)
				throw new ArgumentException($"ComponentList<{typeof(T).Name}> does not contain entity {entity}");
			return ref _components[index];
		}
	}
}
