using System;

namespace TradeDeskNWaySetAssociativeCache
{
    public interface INWaySetAssociativeCache<K, V, EvictionPolicy>
        where K : IEquatable<K>
        where EvictionPolicy : IEvictionPolicy<K, V>, new()
    {
        V Get(K key);
        bool TryGet(K key, out V value);
       
        bool AddOrUpdate(K key, V value);
        void Add(K key, V value);

        void Clear();
        bool IsExist(K key, int index);

        int Count { get; }
    }
}
