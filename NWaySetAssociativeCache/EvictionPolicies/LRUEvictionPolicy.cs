using System;
using System.Collections.Generic;

namespace TradeDeskNWaySetAssociativeCache
{
    public class LRUEvictionPolicy<K, V> : IEvictionPolicy<K, V> where K :  IEquatable<K>
    {
        public LinkedListNode<CacheLineItem<K, V>> GetItemToEvict(LinkedList<CacheLineItem<K, V>> lineList)
        {
            return lineList.First;
        }

    }
}
