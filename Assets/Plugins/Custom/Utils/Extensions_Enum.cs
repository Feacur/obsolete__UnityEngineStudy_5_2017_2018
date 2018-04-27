using System;

namespace Custom.Utils
{
	public static partial class Extensions
	{
		public static T[] GetValues<T>() where T : struct
		{
			if (!typeof(T).IsEnum)
			{
				return null;
			}

			return Enum.GetValues(typeof(T)) as T[];
		}
	}
}
