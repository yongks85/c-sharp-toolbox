using System.Collections.Generic;
using System.Linq;
using Ardalis.GuardClauses;

namespace Utilities;

public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        var n = list.Count;
        while (n > 1)
        {
            n--;
            var k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }

    public static IEnumerable<T> TakeRandom<T>(this IList<T> list, int input)
    {
        Guard.Against.AgainstExpression(amt => list.Count > amt, input, "input is bigger than list size");
        return list.OrderBy(x => ThreadSafeRandom.ThisThreadsRandom.Next()).Take(input);
    }
}