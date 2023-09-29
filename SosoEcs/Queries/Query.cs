namespace SosoEcs.Queries
{
	public class Query
	{
		private readonly List<Type> _with = new List<Type>();
		private readonly List<Type> _none = new List<Type>();

		public Query With<T>()
		{
			_with.Add(typeof(T));
			return this;
		}

		public Query None<T>()
		{
			_none.Add(typeof(T));
			return this;
		}

		public void DoQuery(World world, Span<Entity> entities, int start = 0)
		{
			if (_with.Count <= 0) return;
			
			// HashSet<Entity> hashEntities = new HashSet<Entity>(world.Library.GetEntities(_with.First()));
			//
			// foreach (Type type in _with)
			// {
			// 	hashEntities.IntersectWith(world.Library.GetEntities(type));
			// }
			// foreach (Type type in _none)
			// {
			// 	hashEntities.ExceptWith(world.Library.GetEntities(type));
			// }
			// int idx = 0;
			// foreach (Entity entity in hashEntities)
			// {
			// 	if (start + idx >= entities.Length)
			// 	{
			// 		Console.WriteLine($"Not enough room in entity span query {this}");
			// 		return;
			// 	}
			// 	entities[start + idx] = entity;
			// 	idx++;
			// }
		}
	}
}
