using SosoEcs.Components.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SosoEcs
{
	public partial class EcsWorld
	{
		private readonly Dictionary<Entity, Archetype> _entities = new Dictionary<Entity, Archetype>();
		private readonly List<Archetype> _archetypes = new List<Archetype>();
		
		/// <summary>
		/// Create a new entity with optional components
		/// </summary>
		/// <param name="components"></param>
		/// <returns></returns>
		public Entity CreateEntity(params object[] components)
		{
			Entity entity = new Entity(this);
			
			SetComponents(entity, components);

			return entity;
		}

		/// <summary>
		/// Destroy an entity and remove all components
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public bool Destroy(Entity entity)
		{
			if (_entities.TryGetValue(entity, out var archetype) == false) return false;

			_entities.Remove(entity);
			archetype.Remove(entity);
			return true;
		}

		/// <summary>
		/// Set or add multiple components at once
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="components"></param>
		public void SetComponents(Entity entity, object[] components)
		{
			if (components.Length <= 0) return;
			
			Archetype entityArchetype = _entities[entity];
			if (entityArchetype.IsComponents(components) == false)
			{
				// Create or find archetype
				HashSet<Type> types = new HashSet<Type>(entityArchetype.Types);
				for (int i = 0; i < components.Length; i++)
				{
					types.Add(components[i].GetType());
				}
				entityArchetype = MoveEntity(entity, types);
			}
			foreach (object component in components)
			{
				entityArchetype.Set(entity, component);
			}
		}

		/// <summary>
		/// Set or add a component to an entity
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="component"></param>
		public void SetComponent<T>(Entity entity, T component)
		{
			Archetype archetype = _entities[entity];
			Type type = typeof(T);
			if (archetype.Has(type) == false)
			{
				// Create or find archetype
				HashSet<Type> types = new HashSet<Type>(archetype.Types);
				types.Add(type);
				archetype = MoveEntity(entity, types);
			}
			archetype.SetAs(entity, component, type);
		}

		/// <summary>
		/// Get ref to a component. Must ensure component exists
		/// </summary>
		/// <param name="entity"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public ref T GetComponent<T>(Entity entity) => ref _entities[entity].Get<T>(entity);

		/// <summary>
		/// Remove a component from an entity
		/// </summary>
		/// <param name="entity"></param>
		/// <typeparam name="T"></typeparam>
		public void Remove<T>(in Entity entity)
		{
			Archetype archetype = _entities[entity];
			HashSet<Type> types = archetype.Types;
			types.Remove(typeof(T));
			MoveEntity(entity, types);
		}

		/// <summary>
		/// Check if entity has a component type
		/// </summary>
		/// <param name="entity"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public bool Contains<T>(in Entity entity) => _entities[entity].Has<T>();
		
		/// <summary>
		/// Create an entity in the world
		/// </summary>
		/// <param name="entity"></param>
		internal void CreateEntity(Entity entity)
		{
			Debug.Assert(entity.World == this, "Tried to add an entity to the wrong world");

			_entities[entity] = Archetype.Empty;
		}

		/// <summary>
		/// Move entity to new archetype
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="types"></param>
		/// <returns>New archetype</returns>
		private Archetype MoveEntity(in Entity entity, in HashSet<Type> types)
		{
			Archetype entityArchetype = _entities[entity];
			Archetype newArch = GetOrCreateArchetype(types);
			entityArchetype.MoveTo(entity, newArch);
			_entities[entity] = newArch;
			return newArch;
		}

		/// <summary>
		/// Gets or creates exact archetype with given types
		/// </summary>
		/// <param name="types"></param>
		/// <returns></returns>
		internal Archetype GetOrCreateArchetype(IEnumerable<Type> types)
		{
			Type[] typeArray = types as Type[] ?? types.ToArray();
			foreach (Archetype archetype in _archetypes)
			{
				if (archetype.Is(typeArray)) return archetype;
			}
			Archetype newArch = new Archetype(typeArray);
			_archetypes.Add(newArch);
			return newArch;
		}

		/// <summary>
		/// Yields enumerable of all archetypes with requests types
		/// </summary>
		/// <param name="types"></param>
		/// <returns></returns>
		private IEnumerable<Archetype> GetArchetypes(params Type[] types)
		{
			// TODO: Get rid of loops. Create Bitwise type operations
			foreach (Archetype archetype in _archetypes)
			{
				if (archetype.Has(types)) yield return archetype;
			}
		}
	}
}
