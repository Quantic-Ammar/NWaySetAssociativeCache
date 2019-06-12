using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace TradeDeskNWaySetAssociativeCache
{
    public class NWaySetAssociativeCache<K, V, EvictionPolicy> : INWaySetAssociativeCache<K, V, EvictionPolicy> 
        where K :  IEquatable<K> 
        where EvictionPolicy: IEvictionPolicy<K,V>, new()
    {
        ConcurrentDictionary<int, CacheLine<K, V>> CacheMap;
        ICacheIndexer<K> CacheIndexer;
        int N;
        IEvictionPolicy<K, V> EvictionPolicyInstance;

        public NWaySetAssociativeCache(int n, ICacheIndexer<K> cacheIndexer)
        {
            CacheIndexer = cacheIndexer;
            N = n;
            CacheMap = new ConcurrentDictionary<int, CacheLine<K, V>>();
            EvictionPolicyInstance = new EvictionPolicy();
        }


        public void Clear()
        {
            CacheMap.Clear();
        }


        public V Get(K key)
        {
            var isFound = TryGet(key, out var value);
            if (!isFound)
                throw new KeyNotFoundException($"Key : {key} not found");
            return value;
        }


        public bool TryGet(K key, out V value)
        {
            var index = CacheIndexer.GetCacheIndex(key);
            var isFound = CacheMap.TryGetValue(index, out var cacheLine);
            if (isFound)
            {
                if (cacheLine.Get(key, out value))
                    return true;
            }
            value = default(V);
            return false;

        }

        public bool AddOrUpdate(K key, V value)
        {
            var index = CacheIndexer.GetCacheIndex(key);
            if (!CacheMap.TryGetValue(index, out var cacheLine))
            {
                cacheLine = new CacheLine<K, V>(N, EvictionPolicyInstance);
                CacheMap.TryAdd(index, cacheLine);
            }
            cacheLine.AddOrUpdate(key, value);
            return true;
        }

        public bool IsExist(K key, int index)
        {
            if (CacheMap.TryGetValue(index, out var cacheLine))
            {
                return cacheLine.IsExist(key);
            }
            return false;
        }

        public void Add(K key, V value)
        {
            var index = CacheIndexer.GetCacheIndex(key);
            if (IsExist(key, index))
                throw new ArgumentException($"Key : {key} already exists");
            AddOrUpdate(key, value);
        }

        public int Count
        {
            get
            {
                var count = 0;
                foreach (var cacheLine in CacheMap.Values)
                    count += cacheLine.Count();
                return count;
            }
            
        }
    }
}