using System;
using System.Collections.Generic;
using System.Linq;

namespace SosoEcs.Queries
{
	public class Query
	{
		private readonly HashSet<Type> _with = new HashSet<Type>();
		private readonly HashSet<Type> _none = new HashSet<Type>();

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

		public IEnumerable<Type> GetTypes() => _with.Except(_none);
	}
}
