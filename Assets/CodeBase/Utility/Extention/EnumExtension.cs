using System;

namespace CodeBase.Utility.Extension
{
    public static class EnumExtension
    {
        static readonly Random _r = new();

        public static T GetRandom<T>() where T : Enum
        {
            var values = Enum.GetValues(typeof(T));
            return (T)values.GetValue(_r.Next(values.Length));
        }
    }
}