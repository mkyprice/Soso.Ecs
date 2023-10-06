using SosoEcs.Components.Core;
using System;
using System.Collections.Generic;

namespace SosoEcs
{
	public partial class World
	{
		private readonly Dictionary<Entity, Archetype> _entities = new Dictionary<Entity, Archetype>();
		private readonly List<Archetype> _archetypes = new List<Archetype>();
		
		public Entity CreateEntity(params object[] components)
		{
			Entity entity = new Entity(this);

			_entities[entity] = Archetype.Empty;
			_entities[entity].SetComponents(entity);
			SetComponents(entity, components);

			return entity;
		}

		public void SetComponents(Entity entity, params object[] components)
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
				Archetype archetype = GetOrCreateArchetype(types);
				entityArchetype.MoveTo(entity, archetype);
				entityArchetype = archetype;
				_entities[entity] = archetype;
			}
			entityArchetype.SetComponents(entity, components);
		}

		public ref T GetComponent<T>(Entity entity) => ref _entities[entity].Get<T>(entity);

		private Archetype GetOrCreateArchetype(params Type[] types) => GetOrCreateArchetype(types as IEnumerable<Type>);
		private Archetype GetOrCreateArchetype(IEnumerable<Type> types)
		{
			foreach (Archetype archetype in _archetypes)
			{
				if (archetype.Is(types)) return archetype;
			}
			Archetype newArch = new Archetype(types);
			_archetypes.Add(newArch);
			return newArch;
		}
	}
}
