using System.Runtime.CompilerServices;

namespace SosoEcs.Components.Extensions
{
	public static class TypeExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int GetHash(Type[] types)
		{
			uint hash = 0;
			foreach (Type type in types)
			{
				hash += (uint)type.Name.GetHashCode();
			}
			return (int)hash;
		}
	}
}
