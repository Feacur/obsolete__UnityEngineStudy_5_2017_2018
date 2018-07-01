using System;

namespace Custom.Utils
{
	///
	/// LINQ-like conveniencies
	///
	public static partial class Extensions
	{
		public static bool IsEmpty<T>(this T[] source)
		{
			return source.Length == 0;
		}

		public static T First<T>(this T[] source)
		{
			return (source.Length > 0) ? source[0] : default(T);
		}

		public static T Last<T>(this T[] source)
		{
			return (source.Length > 0) ? source[source.Length - 1] : default(T);
		}

		//
		//
		//

		public static T Find<T>(this T[] source, Predicate<T> match)
		{
			return Array.Find(source, match);
		}

		public static int FindIndex<T>(this T[] source, Predicate<T> match)
		{
			return Array.FindIndex(source, match);
		}

		public static T FindLast<T>(this T[] source, Predicate<T> match)
		{
			return Array.FindLast(source, match);
		}

		public static int FindLastIndex<T>(this T[] source, Predicate<T> match)
		{
			return Array.FindLastIndex(source, match);
		}

		public static T[] FindAll<T>(this T[] source, Predicate<T> match)
		{
			return Array.FindAll(source, match);
		}

		//
		//
		//

		public static bool Exists<T>(this T[] source, Predicate<T> match)
		{
			return Array.Exists(source, match);
		}

		public static TOutput[] ConvertAll<TInput, TOutput>(this TInput[] source, Converter<TInput, TOutput> converter)
		{
			return Array.ConvertAll(source, converter);
		}

		public static bool TrueForAll<T>(this T[] source, Predicate<T> match)
		{
			return Array.TrueForAll(source, match);
		}

		//
		//
		//

		public static int Count<T>(this T[] source, Predicate<T> match)
		{
			int count = 0;
			for (int i = 0; i < source.Length; i++)
			{
				if (match(source[i])) { count++; }
			}
			return count;
		}
	}
}
