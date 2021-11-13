using System;

namespace Utilities
{
    public static class ThreadSafeRandom
    {
        [ThreadStatic] private static Random _local;

        public static Random ThisThreadsRandom
        {
            get { return _local ??= new Random(
                unchecked(Environment.TickCount * 31 + Environment.CurrentManagedThreadId)); }
        }
    }
}
