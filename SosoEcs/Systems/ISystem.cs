namespace SosoEcs.Systems
{
	public interface ISystem
	{
		public void Update(in Entity entity);
	}

	public interface ISystem<T0>
	{
		public void Update(ref T0 t0);
	}

	public interface ISystem<T0, T1>
	{
		public void Update(ref T0 t0, ref T1 t1);
	}
}
