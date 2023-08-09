using System;
using System.Collections.Generic;

namespace CodeBase.Utility.Extension
{
    public static class ListExtension
    {
        private static readonly Random _random = new Random();

        public static T Random<T>(this IList<T> list) => list[_random.Next(list.Count)];

        public static T Ind<T>(this List<T> list, int index)
        {
            index %= list.Count;
            if (index < 0)
                index = list.Count + index;
            return list[index];
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = _random.Next(n + 1);
                (list[n], list[k]) = (list[k], list[n]);
            }
        }

        public static IList<T> Shuffled<T>(this IList<T> list)
        {
            var result = new List<T>(list);
            result.Shuffle();
            return result;
        }

        public static string ToString<T>(this IList<T> list, string separator) =>
            string.Join(separator, list);

        public static bool IsEmpty<T>(this IList<T> list) {  return list.Count == 0; }
        public static bool IsNotEmpty<T>(this IList<T> list) { return list.Count > 0; }
        public static bool IsNullOrEmpty<T>(this IList<T> list) { return list == null || list.Count == 0; }

        public static int LastIndex<T>(this IList<T> list) { return list.Count - 1; }
        public static T Last<T>(this IList<T> list) { return list[list.LastIndex()]; }
    }
}