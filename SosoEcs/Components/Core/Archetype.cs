using SosoEcs.Utils;
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
		private readonly Map<Entity, int> _entityIndexMap = new Map<Entity, int>();
		private readonly Array[] _components;
		private readonly Dictionary<Type, int> _componentTypeIndicies = new Dictionary<Type, int>();

		public Archetype(IEnumerable<Type> types)
		{
			Type[] typeArray = types as Type[] ?? types.ToArray();
			Types = new HashSet<Type>(typeArray);
			_components = new Array[typeArray.Length];
			for (int i = 0; i < _components.Length; i++)
			{
				_components[i] = Array.CreateInstance(typeArray[i], _length);
				_componentTypeIndicies[typeArray[i]] = i;
			}
		}

		/// <summary>
		/// Move an entity with all of its components to another archetype
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="archetype"></param>
		public void MoveTo(Entity entity, Archetype archetype)
		{
			if (_entityIndexMap.TryGetValue(entity, out int entityIndex))
			{
				foreach (KeyValuePair<Type,int> componentTypeIndicy in _componentTypeIndicies)
				{
					Array t = _components[componentTypeIndicy.Value];
					object component = t.GetValue(entityIndex);
					archetype.SetAs(entity, component, componentTypeIndicy.Key);
				}
				Remove(entity);
			}
		}

		/// <summary>
		/// Check if type exists in archetype
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
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
		/// Same as Is but uses component objects rather than type
		/// </summary>
		/// <param name="components"></param>
		/// <returns></returns>
		public bool IsComponents(params object[] components)
		{
			if (components.Length != Types.Count) return false;
			foreach (object component in components)
			{
				if (Types.Contains(component.GetType()) == false) return false;
			}
			return true;
		}
		/// <summary>
		/// Check if archetype types is the same as given
		/// </summary>
		/// <param name="components"></param>
		/// <returns></returns>
		public bool Is(params Type[] components) => Is(components as IEnumerable<Type>);
		public bool Is(IEnumerable<Type> components)
		{
			Type[] types = components as Type[] ?? components.ToArray();
			if (types.Count() != Types.Count) return false;
			foreach (Type component in types)
			{
				if (Types.Contains(component) == false) return false;
			}
			return true;
		}

		/// <summary>
		/// Set an entities' component
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="component"></param>
		public void Set(Entity entity, object component)
		{
			Type type = component.GetType();
			SetAs(entity, component, type);
		}

		/// <summary>
		/// Set an entities' component
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="component"></param>
		public void SetAs(Entity entity, object component, Type type)
		{
			Debug.Assert(_componentTypeIndicies.ContainsKey(type), $"Archetype does not contain component type {type}");
			if (_entityIndexMap.ContainsKey(entity) == false)
			{
				_entityIndexMap[entity] = Size;
				Size++;
				if (Size >= _length) Resize(_length * 2);
			}
			int componentIndex = _componentTypeIndicies[type];
			int entityIndex = _entityIndexMap[entity];
			_components[componentIndex].SetValue(component, entityIndex);
		}
		
		/// <summary>
		/// Remove an entity
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public bool Remove(Entity entity)
		{
			if (_entityIndexMap.TryGetValue(entity, out int entityIndex) == false) return false;
			
			_entityIndexMap.Remove(entity);
			
			Size--;
			
			if (_entityIndexMap.TryGetValue(Size, out Entity switchEntity))
			{
				_entityIndexMap[switchEntity] = entityIndex;
			}
			
			foreach (Array components in _components)
			{
				object? component = components.GetValue(Size);
				components.SetValue(component, entityIndex);
				components.SetValue(default, Size);
			}
			
			return true;
		}

		/// <summary>
		/// Resize archetype to given size
		/// </summary>
		/// <param name="length"></param>
		private void Resize(int length)
		{
			Debug.Assert(length > _length, "Cannot currently shrink archetype");
			_length = length;
			Console.WriteLine("Resizing archetype arrays to {0}", _length);
			foreach (var comp in _componentTypeIndicies)
			{
				Array tmp = Array.CreateInstance(comp.Key, _length);
				Array src = _components[comp.Value];
				Array.Copy(src, tmp, src.Length);
				_components[comp.Value] = tmp;
			}
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Entity GetEntity(int index) => _entityIndexMap[index];
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ref T Get<T>(Entity entity) => ref Get<T>(_entityIndexMap[entity]);
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ref T Get<T>(int index) => ref GetArray<T>()[index];
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<T> GetSpan<T>() => GetArray<T>();
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T[] GetArray<T>() => Unsafe.As<T[]>(_components[_componentTypeIndicies[typeof(T)]]);
	}
}
