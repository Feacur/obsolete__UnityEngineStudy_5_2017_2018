using System;

namespace Custom.Utils
{
    public static partial class Extensions
    {
        public static T First<T>(this T[] source)
        {
            return (source.Length > 0) ? source[0] : default(T);
        }

        public static T Last<T>(this T[] source)
        {
            return (source.Length > 0) ? source[source.Length - 1] : default(T);
        }

        public static T Find<T>(this T[] source, Predicate<T> match)
        {
            return Array.Find(source, match);
        }

        public static T[] FindAll<T>(this T[] source, Predicate<T> match)
        {
            return Array.FindAll(source, match);
        }

        public static bool Exists<T>(this T[] source, Predicate<T> match)
        {
            return Array.Exists(source, match);
        }

        public static bool IsEmpty<T>(this T[] source)
        {
            return source.Length == 0;
        }

        public static int Count<T>(this T[] source, Predicate<T> match)
        {
            int count = 0;
            for (int i = 0; i < source.Length; i++)
            {
                if (match(source[i]))
                {
                    count++;
                }
            }

            return count;
        }
    }
}
