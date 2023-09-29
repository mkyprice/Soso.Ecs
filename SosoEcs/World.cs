using SosoEcs.Components.Core;
using SosoEcs.Components.Extensions;
using SosoEcs.Queries;

namespace SosoEcs
{
	public class World
	{
		private readonly Dictionary<Entity, Archetype> _entities = new Dictionary<Entity, Archetype>();
		private readonly List<Archetype> Archetypes = new List<Archetype>();
		
		public Entity CreateEntity(params object[] components)
		{
			Entity entity = new Entity(this);

			SetComponents(entity, components);

			return entity;
		}

		public void SetComponents(Entity entity, params object[] components)
		{
			if (components.Length <= 0) return;

			Type[] types = new Type[components.Length]; // TODO: Static array
			for (int i = 0; i < components.Length; i++)
			{
				types[i] = components[i].GetType();
			}
			Archetype archetype = GetOrCreateArchetype(types);
			archetype.SetComponents(entity, components);
			_entities[entity] = archetype;
		}

		public ref T GetComponent<T>(Entity entity) => ref _entities[entity].Get<T>(entity);

		private Archetype GetOrCreateArchetype(Type[] types)
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
