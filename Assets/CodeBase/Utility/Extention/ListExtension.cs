using System;
using System.Collections.Generic;

namespace CodeBase.Utility.Extension
{
    public static class ListExtension
    {
        private static readonly Random _random = new Random();

        public static T Random<T>(this IList<T> list) => list[_random.Next(list.Count)];

        /// <summary>  Give any index without IndexOutRange  </summary>
        public static T Ind<T>(this List<T> list, int index)
        {
            index = Math.Abs(index);
            return list[index % list.Count];
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

        public static string ContentToString<T>(this List<T> list, string separator = "\n")
        {
            string str = "";
            for (int i = 0; i < list.Count - 1; i++)
            {
                str += list[i].ToString() + separator;
            }

            str+=list[^1].ToString();

            return str;
        }
    }
}