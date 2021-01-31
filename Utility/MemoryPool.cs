using System.Collections.Concurrent;

namespace engenious.Utility
{
    public static class MemoryPool<T>
        where T : class, new()
    {
        private static System.Collections.Concurrent.ConcurrentStack<T> _availableItems;

        static MemoryPool()
        {
            _availableItems = new ConcurrentStack<T>();
        }
        public static T Acquire()
        {
            if (!_availableItems.TryPop(out var el))
                el = new T();
            return el;
        }
        public static void Release(T rel)
        {
            _availableItems.Push(rel);
        }
    }
}