
using System;

namespace TradeDeskNWaySetAssociativeCache
{
    public class BoundedCacheIndexer<T> : ICacheIndexer<T>
    {
        int Capacity;
        public BoundedCacheIndexer(int capacity)
        {
            Capacity = capacity;
        }
        public int GetCacheIndex(T key)
        {
            if (key == null)
                return 0;
            return Math.Abs(key.GetHashCode()) % Capacity;
        }
    }
}
