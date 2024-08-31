using Soso.Ecs.Benchmarks.BadEcs.Internal;

namespace Soso.Ecs.Benchmarks.BadEcs
{
	public class SlowEcsDemo : IDisposable
	{
		#region Public Functions

		/// <summary>
		/// Create a new entity
		/// </summary>
		/// <returns></returns>
		public int CreateEntity()
		{
			int id = _nextEntity++;
			_entities.Add(id);
			return id;
		}

		/// <summary>
		/// Destroy an entity. Removes all components associated with it
		/// </summary>
		/// <param name="id"></param>
		public void Remove(int id)
		{
			if (_entities.Remove(id) == false)
			{
				Console.WriteLine("ERROR: entity {0} was removed but was not present in the entity collection", id);
			}
			foreach (IComponentList list in _components.Values)
			{
				list.Remove(id);
			}
		}

		/// <summary>
		/// Add component to entity
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="id"></param>
		/// <param name="component"></param>
		/// <returns></returns>
		public bool AddComponent<T>(int id, T component)
		{
			if (_entities.Contains(id) == false)
				Console.WriteLine("ERROR: trying to add components to an entity that does not exist. Entity: {0}", id);
			if (typeof(T).IsInterface)
				return GetOrCreateList(component.GetType()).Add(id, component);
			return GetOrCreateList<T>().Add(id, component);
		}

		/// <summary>
		/// Add or override a component on an entity
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="id"></param>
		/// <param name="component"></param>
		public void SetComponent<T>(int id, T component)
		{
			if (_entities.Contains(id) == false)
				Console.WriteLine("ERROR: trying to add components to an entity that does not exist. Entity: {0}", id);
			if (typeof(T).IsInterface)
				GetOrCreateList(component.GetType()).Set(id, component);
			else 
				GetOrCreateList<T>().Set(id, component);
		}

		/// <summary>
		/// Remove component from entity
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="id"></param>
		/// <returns></returns>
		public bool RemoveComponent<T>(int id)
		{
			return GetOrCreateList<T>().Remove(id);
		}

		/// <summary>
		/// Get a reference to the component
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="id"></param>
		/// <returns></returns>
		public ref T GetComponent<T>(int id)
		{
			if (_entities.Contains(id) == false)
				Console.WriteLine("ERROR: trying to find components for an entity that does not exist. Entity: {0}", id);
			return ref GetOrCreateList<T>().Get(id);
		}

		/// <summary>
		/// Check if a component is attached to an entity
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="id"></param>
		/// <returns></returns>
		public bool HasComponent<T>(int id)
		{
			return TryGetList<T>(out var list) && list.Contains(id);
		}

		/// <summary>
		/// Get a list of all entities the given component types
		/// </summary>
		/// <param name="components"></param>
		/// <returns></returns>
		public IEnumerable<int> EntitiesWith(params Type[] components)
		{
			HashSet<int>? entities = null;
			for (int i = 0; i < components.Length; i++)
			{
				if (TryGetList(components[i], out IComponentList list))
				{
					if (i == 0 || entities == null)
					{
						entities = new HashSet<int>(list.GetEntities());
					}
					else
					{
						// Add entities but remove entities that are not present
						entities.IntersectWith(list.GetEntities());
					}
				}
				else
				{
					// List does not exists so there will be no entities
					entities?.Clear();
					break;
				}
			}
			return entities ?? Enumerable.Empty<int>();
		}
		
		public void Update()
		{
			foreach (var system in _systems)
			{
				system.Update();
			}
		}
		

		public void Draw()
		{
			foreach (ISystem system in _systems)
			{
				system.Render();
			}
		}

		public bool AddSystem(ISystem system) => _systems.Add(system);
		public bool RemoveSystem(ISystem system) => _systems.Remove(system);
		public T? GetSystem<T>()
			where T : ISystem
		{
			Type type = typeof(T);
			return _systems.FirstOrDefault(s => s.GetType() == type) as T;
		}

		public void Dispose()
		{
			foreach (int entity in new List<int>(_entities))
			{
				Remove(entity);
			}
			_components.Clear();
			_systems.Clear();
		}

		#endregion

		#region Private Members

		private readonly HashSet<int> _entities = new HashSet<int>();
		private readonly Dictionary<int, IComponentList> _components = new Dictionary<int, IComponentList>();
		private readonly HashSet<ISystem> _systems = new HashSet<ISystem>();
		private int _nextEntity = 1;

		#endregion

		#region Private Functions

		private bool TryGetList(Type type, out IComponentList list)
		{
			int key = GetKey(type);
			return _components.TryGetValue(key, out list);
		}

		private ComponentList<T> GetOrCreateList<T>()
		{
			int key = GetKey<T>();
			if (_components.TryGetValue(key, out IComponentList? list) == false)
			{
				list = new ComponentList<T>();
				_components[key] = list;
			}
			return (ComponentList<T>)list;
		}

		private IComponentList GetOrCreateList(Type type)
		{
			int key = GetKey(type);
			IComponentList list;
			if (_components.TryGetValue(key, out list) == false)
			{
				Type list_type = typeof(ComponentList<>).MakeGenericType(type);
				list = (IComponentList)Activator.CreateInstance(list_type);
				_components[key] = list;
			}
			return list;
		}

		private bool TryGetList<T>(out ComponentList<T> list)
		{
			int key = GetKey<T>();
			if (_components.TryGetValue(key, out IComponentList l))
			{
				list = (ComponentList<T>)l;
				return true;
			}
			list = null;
			return false;
		}


		private int GetKey<T>() => GetKey(typeof(T));

		private int GetKey(in Type type)
		{
			return type.Name.GetHashCode();
		}

		#endregion
	}
}
