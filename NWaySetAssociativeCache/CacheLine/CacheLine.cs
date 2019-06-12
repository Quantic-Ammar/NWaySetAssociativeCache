using System;
using System.Collections.Generic;

namespace TradeDeskNWaySetAssociativeCache
{
    public class CacheLine<K, V> where K : IEquatable<K>
    {
        int Capacity;
        Dictionary<K, LinkedListNode<CacheLineItem<K, V>>> CacheMap = new Dictionary<K, LinkedListNode<CacheLineItem<K, V>>>();
        LinkedList<CacheLineItem<K, V>> LineList = new LinkedList<CacheLineItem<K, V>>();

        IEvictionPolicy<K,V> EvictionPolicy;
        
        object Lock = new object();

        public CacheLine(int capacity, IEvictionPolicy<K,V> evictionPolicy)
        {
            Capacity = capacity;
            EvictionPolicy = evictionPolicy;
        }
        public bool IsExist(K key)
        {
            lock (Lock)
            {
                return CacheMap.ContainsKey(key);
            }
        }

        public bool Get(K key, out V value)
        {
            LinkedListNode<CacheLineItem<K, V>> node;
            lock (Lock)
            {
                if (CacheMap.TryGetValue(key, out node))
                {
                    ProcessAccessed(node);
                    value = node.Value.Data;
                    return true;
                }
            }
            value = default(V);
            return false;
        }

        public bool Remove(K key)
        {
            LinkedListNode<CacheLineItem<K, V>> node;
            lock (Lock)
            {
                if (CacheMap.TryGetValue(key, out node))
                {
                    LineList.Remove(node);
                    CacheMap.Remove(key);
                    return true;
                }
               
            }
            return false;
        }

        public void AddOrUpdate(K key, V val)
        {
            lock (Lock)
            {
                if (processUpdate(key, val))
                    return;

                LinkedListNode<CacheLineItem<K, V>> node = new LinkedListNode<CacheLineItem<K, V>>(new CacheLineItem<K, V>(key, val));
           
                if (CacheMap.Count >= Capacity)
                {
                    var evictedNode = EvictionPolicy.GetItemToEvict(LineList);
                    RemoveFromCache(evictedNode);
                }
                LineList.AddLast(node);
                CacheMap.Add(key, node);
            }
            
        }

        public int Count()
        {
            lock (Lock)
            {
                return CacheMap.Count;
            }
        }

        bool processUpdate(K key, V val)
        {
            if (CacheMap.TryGetValue(key, out var existingNode))
            {
                existingNode.Value.Data = val;
                ProcessAccessed(existingNode);
                return true;
            }
            return false;
        }

        void ProcessAccessed(LinkedListNode<CacheLineItem<K, V>> node)
        {
            if(node != LineList.Last)
            {
                // O(1) operations
                LineList.Remove(node);
                LineList.AddLast(node);
            }
        }

        void RemoveFromCache(LinkedListNode<CacheLineItem<K, V>> node)
        {
            LineList.Remove(node); // O(1) operation
            if (node != null)
            {
                CacheMap.Remove(node.Value.Tag);
            }
        }

    }
}
