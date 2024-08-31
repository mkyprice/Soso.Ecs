using System.Collections.Generic;

namespace Soso.Ecs.Utils
{
	/// <summary>
	/// A bi-directional dictionary
	/// </summary>
	/// <typeparam name="T1"></typeparam>
	/// <typeparam name="T2"></typeparam>
	public class Map<T1, T2> 
		where T2 : notnull 
		where T1 : notnull
	{
		private readonly Dictionary<T1, T2> _forward = new Dictionary<T1, T2>();
		private readonly Dictionary<T2, T1> _backward = new Dictionary<T2, T1>();

		public T2 this[T1 key]
		{
			get => _forward[key];
			set
			{
				_forward[key] = value;
				_backward[value] = key;
			}
		}

		public T1 this[T2 key]
		{
			get => _backward[key];
			set
			{
				_backward[key] = value;
				_forward[value] = key;
			}
		}

		public bool TryGetValue(T1 key, out T2 value) => _forward.TryGetValue(key, out value);
		public bool TryGetValue(T2 key, out T1 value) => _backward.TryGetValue(key, out value);

		public bool ContainsKey(T1 key) => _forward.ContainsKey(key);
		public bool ContainsKey(T2 key) => _backward.ContainsKey(key);

		public bool Remove(T1 key)
		{
			if (_forward.TryGetValue(key, out T2 value))
			{
				_forward.Remove(key);
				_backward.Remove(value);
				return true;
			}
			return false;
		}
		public bool Remove(T2 key) => _backward.TryGetValue(key, out T1 value) && Remove(value);
	}
}
