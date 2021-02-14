using System.Collections.Concurrent;

namespace engenious.Utility
{
    /// <summary>
    /// A simple memory pool for generic types.
    /// </summary>
    /// <typeparam name="T">The type to pool.</typeparam>
    public static class MemoryPool<T>
        where T : class, new()
    {
        private static readonly ConcurrentStack<T> AvailableItems;

        static MemoryPool()
        {
            AvailableItems = new ConcurrentStack<T>();
        }
        /// <summary>
        /// Tries to get a object from the memory pool if available;
        /// otherwise a new element is returned that can be returned to the pool.
        /// <remarks>Objects acquired with this method need to be returned using <seealso cref="Release"/> function.</remarks>
        /// </summary>
        /// <returns></returns>
        public static T Acquire()
        {
            if (!AvailableItems.TryPop(out var el))
                el = new T();
            return el;
        }
        /// <summary>
        /// Returns a previously acquired object back into the memory pool.
        /// </summary>
        /// <param name="rel">The object to return</param>
        public static void Release(T rel)
        {
            AvailableItems.Push(rel);
        }
    }
}