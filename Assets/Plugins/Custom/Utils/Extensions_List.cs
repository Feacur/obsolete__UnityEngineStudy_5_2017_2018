using System;
using System.Collections.Generic;

namespace Custom.Utils
{
    ///
    /// LINQ-like conveniencies
    ///
    public static partial class Extensions
    {
        public static bool IsEmpty<T>(this List<T> source)
        {
            return source.Count == 0;
        }

        public static T First<T>(this List<T> source)
        {
            return (source.Count > 0) ? source[0] : default(T);
        }

        public static T Last<T>(this List<T> source)
        {
            return (source.Count > 0) ? source[source.Count - 1] : default(T);
        }

        //
        //
        //

        public static int Count<T>(this List<T> source, Predicate<T> predicate)
        {
            int count = 0;
            for (int i = 0; i < source.Count; i++)
            {
                if (predicate(source[i])) { count++; }
            }
            return count;
        }
    }
}
