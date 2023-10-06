using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SosoEcs.Components.Core
{
	/// <summary>
	/// Archetype is a data structure that holds all entities that
	/// have the same components
	/// </summary>
	public class Archetype
	{
		public static readonly Archetype Empty = new Archetype(Array.Empty<Type>());
		public int Size { get; private set; }
		private int Length = 64;
		private readonly Dictionary<Entity, int> _entityIndicies = new Dictionary<Entity, int>();
		private readonly Array[] _components;
		private readonly Dictionary<Type, int> _componentsIndicies = new Dictionary<Type, int>();
		public readonly HashSet<Type> Types;

		public Archetype(IEnumerable<Type> types)
		{
			Type[] typeArray = types.ToArray();
			Types = new HashSet<Type>(types);
			_components = new Array[typeArray.Length];
			for (int i = 0; i < _components.Length; i++)
			{
				_components[i] = Array.CreateInstance(typeArray[i], Length);
				_componentsIndicies[typeArray[i]] = i;
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

			if (Size >= Length) Resize(Length * 2);

			foreach (object component in components)
			{
				Set(entity, component);
			}
			
			Size++;
			return true;
		}

		public void MoveTo(Entity entity, Archetype archetype)
		{
			int entityIndex = _entityIndicies[entity];
			foreach (Array t in _components)
			{
				object component = t.GetValue(entityIndex);
				archetype.SetComponents(entity, component);
			}
			Remove(entity);
		}

		public bool Has<T>() => Types.Contains(typeof(T));
		public bool Has(in Type type) => Types.Contains(type);
		public bool Is(params Type[] components) => Is(components as IEnumerable<Type>);
		public bool Is(IEnumerable<Type> components)
		{
			foreach (Type component in components)
			{
				if (Types.Contains(component) == false) return false;
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
				Array src = _components[comp.Value];
				Array.Copy(src, tmp, src.Length);
				_components[comp.Value] = tmp;
			}
		}
		
		public ref T Get<T>(Entity entity) => ref GetArray<T>()[_entityIndicies[entity]];
		public ref T Get<T>(int index) => ref GetArray<T>()[index];
		public ref object Get(int index, Type type) => ref Unsafe.As<object[]>(_components[_componentsIndicies[type]])[index];

		public T[] GetArray<T>() => Unsafe.As<T[]>(_components[_componentsIndicies[typeof(T)]]);
	}
}
