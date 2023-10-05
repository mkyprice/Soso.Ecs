using SosoEcs.Components.Core;
using SosoEcs.Queries;

namespace SosoEcs
{
	public partial class World
	{
		private readonly Dictionary<Entity, Archetype> _entities = new Dictionary<Entity, Archetype>();
		private readonly List<Archetype> Archetypes = new List<Archetype>();
		
		public Entity CreateEntity(params object[] components)
		{
			Entity entity = new Entity(this);

			_entities[entity] = new Archetype(Array.Empty<Type>());
			_entities[entity].SetComponents(entity);
			SetComponents(entity, components);

			return entity;
		}

		public void SetComponents(Entity entity, params object[] components)
		{
			if (components.Length <= 0) return;
			
			Archetype entityArchetype = _entities[entity];
			if (entityArchetype.Is(components.GetType()) == false)
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
			foreach (Archetype archetype in Archetypes)
			{
				if (archetype.Is(types)) return archetype;
			}
			Archetype newArch = new Archetype(types);
			Archetypes.Add(newArch);
			return newArch;
		}

		public void GetEntities(in Query query, Span<Entity> entities, int start = 0)
		{
			query.DoQuery(this, entities, start);
		}
	}
}
