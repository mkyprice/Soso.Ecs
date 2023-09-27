using SosoEcs.Components.Core;

namespace SosoEcs
{
	public class World
	{
		public readonly ComponentLibrary Library = new ComponentLibrary();
		private readonly HashSet<Entity> _entities = new HashSet<Entity>();
		
		public Entity CreateEntity()
		{
			Entity entity = new Entity(this);

			_entities.Add(entity);

			return entity;
		}

		public void GetEntities(in Query query, Span<Entity> entities, int start = 0)
		{
			query.DoQuery(this, entities, start);
		}
	}
}
