using SosoEcs.Components.Extensions;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SosoEcs.Components.Core
{
	/// <summary>
	/// Archetype is a data structure that holds all entities that
	/// have the same components
	/// </summary>
	public class Archetype
	{
		public int Size { get; private set; }
		private int Length = 64;
		private readonly Dictionary<Entity, int> _entityIndicies = new Dictionary<Entity, int>();
		private readonly Array[] _components;
		private readonly Dictionary<Type, int> _componentsIndicies = new Dictionary<Type, int>();
		private readonly HashSet<Type> _types;
		private readonly int _hash;

		public Archetype(params Type[] types)
		{
			if (types.Length <= 0) throw new Exception($"Archetype cannot be created with no types");

			_types = new HashSet<Type>(types);
			_hash = TypeExtensions.GetHash(types);
			_components = new Array[types.Length];
			for (int i = 0; i < _components.Length; i++)
			{
				_components[i] = Array.CreateInstance(types[i], Length);
				_componentsIndicies[types[i]] = i;
			}
		}

		public bool SetComponents(Entity entity, params object[] components)
		{
			if (_entityIndicies.ContainsKey(entity))
			{
				foreach (object component in components)
				{
					Set(entity, component);
				}
				return true;
			}

			_entityIndicies[entity] = Size;

			if (components.Length != _components.Length)
			{
				Console.WriteLine("WARNING: tried to add entity to archetype with wrong number of components");
				return false;
			}

			if (Size >= Length) Resize(Length * 2);

			foreach (object component in components)
			{
				Set(entity, component);
			}
			
			Size++;
			return true;
		}

		public bool Has<T>() => _types.Contains(typeof(T));
		public bool Is(Type[] types)
		{
			if (types.Length != _types.Count) return false;
			foreach (Type type in types)
			{
				if (_types.Contains(type) == false) return false;
			}
			return true;
		}

		private void Set(Entity entity, object component)
		{
			Type type = component.GetType();
			Debug.Assert(_componentsIndicies.ContainsKey(type), $"Archetype does not contain component type {type}");
			Debug.Assert(_entityIndicies.ContainsKey(entity), $"Archetype does not contain component type {type}");
			int componentIndex = _componentsIndicies[type];
			int entityIndex = _entityIndicies[entity];
			_components[componentIndex].SetValue(component, entityIndex);
		}

		public bool Remove(Entity entity)
		{
			if (_entityIndicies.TryGetValue(entity, out int entityIndex) == false) return false;
			Size--;

			foreach (Array components in _components)
			{
				components.SetValue(components.GetValue(Size), entityIndex);
				components.SetValue(default, Size);
			}
			
			return true;
		}

		private void Resize(int length)
		{
			Length = length;
			Console.WriteLine("Resizing archetype arrays to {0}", Length);
			foreach (var comp in _componentsIndicies)
			{
				Array tmp = Array.CreateInstance(comp.Key, Length);
				Buffer.BlockCopy(_components[comp.Value], 0, tmp, 0, _components[comp.Value].Length);
				_components[comp.Value] = tmp;
			}
		}
		
		public ref T Get<T>(Entity entity) => ref GetArray<T>()[_entityIndicies[entity]];
		public ref T Get<T>(int index) => ref GetArray<T>()[index];

		public T[] GetArray<T>() => Unsafe.As<T[]>(_components[_componentsIndicies[typeof(T)]]);

		public override int GetHashCode() => _hash;
	}
}
