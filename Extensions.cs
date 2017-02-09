using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParseTsgReplays
{
    public static class Extensions
    {
        public static HashSet<T> ToSet<T>(this IEnumerable<T> src)
        {
            var set = new HashSet<T>();
            src.All(set.Add);
            return set;
        }
    }
}
