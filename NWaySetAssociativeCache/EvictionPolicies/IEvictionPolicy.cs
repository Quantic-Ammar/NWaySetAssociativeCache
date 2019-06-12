using System;
using System.Collections.Generic;

namespace TradeDeskNWaySetAssociativeCache
{
    public interface IEvictionPolicy<K, V> where K : IEquatable<K>
    {
        /// <summary>
        /// CacheLineItem's set is internal so value itself is immutable for the clients of this lib willing to provide their own eviction policy
        /// LinkedList itself is open to modification by external clients
        /// LinkedList is a sealed class and does not implement IReadOnlyCollection<T> so if there wass a requirement to make it readonly, 
        /// I needed to implement my own version of LinkedList (probably exposing only keys) which I felt was out of scope for this assignment
        /// Another way was to use IEnumerable but that would lose First and Last (making accessing Last an O(N) operation which is used by MRU)
        /// Between performance vs external client unable to modify list: I chose performance and also to not implement my own LinkedList, because after all client owns the data anyways
        /// </summary>
        /// <param name="lineList"></param>
        /// <returns>Node to evict</returns>
        LinkedListNode<CacheLineItem<K, V>> GetItemToEvict(LinkedList<CacheLineItem<K, V>> lineList);
    }
}
