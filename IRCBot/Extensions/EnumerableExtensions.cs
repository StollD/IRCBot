using System;
using System.Collections.Generic;

namespace IRCBot
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (object o in enumerable)
                action.Invoke((T)o);
            return enumerable;
        }
    }
}