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
	internal class Archetype
	{
		public static readonly Archetype Empty = new Archetype(Array.Empty<Type>());
		public int Size { get; private set; }
		public readonly HashSet<Type> Types;
		
		private int _length = 256;
		private readonly Dictionary<Entity, int> _entityIndicies = new Dictionary<Entity, int>();
		private readonly Dictionary<int, Entity> _indexToEntity = new Dictionary<int, Entity>();
		private readonly Array[] _components;
		private readonly Dictionary<Type, int> _componentsIndicies = new Dictionary<Type, int>();

		public Archetype(IEnumerable<Type> types)
		{
			Type[] typeArray = types as Type[] ?? types.ToArray();
			Types = new HashSet<Type>(typeArray);
			_components = new Array[typeArray.Length];
			for (int i = 0; i < _components.Length; i++)
			{
				_components[i] = Array.CreateInstance(typeArray[i], _length);
				_componentsIndicies[typeArray[i]] = i;
			}
		}

		public void MoveTo(Entity entity, Archetype archetype)
		{
			if (_entityIndicies.TryGetValue(entity, out int entityIndex))
			{
				foreach (Array t in _components)
				{
					object component = t.GetValue(entityIndex);
					archetype.Set(entity, component);
				}
				Remove(entity);
			}
		}

		public bool Has<T>() => Types.Contains(typeof(T));
		public bool Has(in Type type) => Types.Contains(type);
		public bool Has(in Type[] types)
		{
			foreach (Type type in types)
			{
				if (Types.Contains(type) == false) return false;
			}
			return true;
		}
		
		/// <summary>
		/// Same as As but uses component objects rather than type
		/// </summary>
		/// <param name="components"></param>
		/// <returns></returns>
		public bool IsComponents(params object[] components)
		{
			foreach (object component in components)
			{
				if (Types.Contains(component.GetType()) == false) return false;
			}
			return true;
		}
		public bool Is(params Type[] components) => Is(components as IEnumerable<Type>);
		public bool Is(IEnumerable<Type> components)
		{
			foreach (Type component in components)
			{
				if (Types.Contains(component) == false) return false;
			}
			return true;
		}

		public void Set(Entity entity, object component)
		{
			Type type = component.GetType();
			Debug.Assert(_componentsIndicies.ContainsKey(type), $"Archetype does not contain component type {type}");
			if (_entityIndicies.ContainsKey(entity) == false)
			{
				_entityIndicies[entity] = Size;
				_indexToEntity[Size] = entity;
				Size++;
				if (Size >= _length) Resize(_length * 2);
			}
			int componentIndex = _componentsIndicies[type];
			int entityIndex = _entityIndicies[entity];
			_components[componentIndex].SetValue(component, entityIndex);
		}

		public bool Remove(Entity entity)
		{
			if (_entityIndicies.TryGetValue(entity, out int entityIndex) == false) return false;
			Size--;

			_entityIndicies.Remove(entity);
			_indexToEntity.Remove(entityIndex);
			foreach (Array components in _components)
			{
				components.SetValue(components.GetValue(Size), entityIndex);
				components.SetValue(default, Size);
			}
			
			return true;
		}

		private void Resize(int length)
		{
			_length = length;
			Console.WriteLine("Resizing archetype arrays to {0}", _length);
			foreach (var comp in _componentsIndicies)
			{
				Array tmp = Array.CreateInstance(comp.Key, _length);
				Array src = _components[comp.Value];
				Array.Copy(src, tmp, src.Length);
				_components[comp.Value] = tmp;
			}
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Entity GetEntity(int index) => _indexToEntity[index];
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ref T Get<T>(Entity entity) => ref Get<T>(_entityIndicies[entity]);
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ref T Get<T>(int index) => ref GetArray<T>()[index];
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<T> GetSpan<T>() => GetArray<T>();
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T[] GetArray<T>() => Unsafe.As<T[]>(_components[_componentsIndicies[typeof(T)]]);
	}
}
