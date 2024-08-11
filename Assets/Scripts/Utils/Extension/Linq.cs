using System;
using System.Collections.Generic;
using System.Linq;

namespace VVVVVV.Utils.Extension;

public static class Linq
{
    public static void ForEach<T>(this IEnumerable<T> _enumerable, Action<T> action)
    {
        _enumerable.ToList().ForEach(action);
    }
}