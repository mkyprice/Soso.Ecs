namespace Soso.Ecs.Benchmarks.BadEcs.Internal
{
	internal interface IComponentList
	{
		public bool Contains(int id);
		public bool Remove(int id);
		public bool Add(int id, object component);
		public void Set(int id, object component);
		public IEnumerable<int> GetEntities();
	}

	internal class ComponentList<T> : IComponentList
	{
		public int Count => _count;

		private T[] _components;
		private int _count = 0;
		private int _capacity = 10;

		public ComponentList()
		{
			_components = new T[_capacity];
		}

		/// <summary>
		/// Lookup the index given the ID
		/// </summary>
		private readonly Dictionary<int, int> IdToIndex = new Dictionary<int, int>();
		/// <summary>
		/// Lookup the ID given the index
		/// </summary>
		private readonly Dictionary<int, int> IndexToId = new Dictionary<int, int>();

		public bool Add(int id, T component)
		{
			if (IdToIndex.ContainsKey(id) == false)
			{
				Set(id, component);
				return true;
			}
			return false;
		}

		public bool Add(int id, object component)
		{
			if (component.GetType() != typeof(T))
				throw new Exception(string.Format("Tried to add component of wrong type!", component.GetType()));
			T c = (T)component;
			return Add(id, c);
		}

		public void Set(int id, T component)
		{
			if (IdToIndex.ContainsKey(id) == false)
			{
				IdToIndex.Add(id, _count);
				IndexToId.Add(_count, id);
				_components[_count] = component;
				_count++;
				if (_count >= _capacity)
				{
					_capacity = _capacity == 0 ? 1 : _capacity * 2;
					T[] newArray = new T[_capacity];
					_components.CopyTo(newArray, 0);
					_components = newArray;
				}
			}
			else
			{
				_components[IdToIndex[id]] = component;
			}
		}

		public void Set(int id, object component)
		{
			if (component.GetType() != typeof(T))
				throw new Exception(string.Format("Tried to add component of wrong type!", component.GetType()));
			T c = (T)component;
			Set(id, c);
		}

		public bool Remove(int id)
		{
			bool can_remove = IdToIndex.ContainsKey(id);
			if (can_remove)
			{
				T component = _components[IdToIndex[id]];
				int idx = IdToIndex[id];
				int swap_idx = _count - 1;
				int swap_id = IndexToId[swap_idx];
				IndexToId[idx] = swap_id;
				IdToIndex[swap_id] = idx;
				(_components[idx], _components[swap_idx]) = (_components[swap_idx], _components[idx]);

				_count--;
				IdToIndex.Remove(id);
				IndexToId.Remove(swap_idx);

				if (_count < _capacity / 2 && _capacity > 10)
				{
					_capacity = _capacity == 0 ? 1 : _capacity / 2;
					T[] newArray = new T[_capacity];
					Array.Copy(_components, 0, newArray, 0, newArray.Length);
					_components = newArray;
				}
			}
			return can_remove;
		}

		public ref T Get(int id)
		{
			if (IdToIndex.ContainsKey(id) == false)
				throw new Exception(string.Format("Component list of type {0} does not contain entity {1}", typeof(T), id));
			return ref _components[IdToIndex[id]];
		}

		public bool Contains(int id) => IdToIndex.ContainsKey(id);

		public IEnumerable<int> GetEntities()
		{
			return IdToIndex.Keys;
		}
	}
}
