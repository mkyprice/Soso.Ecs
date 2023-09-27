namespace SosoEcs.Components.Core
{
	public interface IComponents
	{
		public bool Remove(Entity entity);
		public IEnumerable<Entity> GetEntities();
	}
}
