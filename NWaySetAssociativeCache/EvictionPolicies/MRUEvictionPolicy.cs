using System;
using System.Collections.Generic;

namespace TradeDeskNWaySetAssociativeCache
{
    public class MRUEvictionPolicy<K, V> : IEvictionPolicy<K, V> where K : IConvertible, IEquatable<K>
    {
        public LinkedListNode<CacheLineItem<K, V>> GetItemToEvict(LinkedList<CacheLineItem<K, V>> lineList)
        {
            return lineList.Last;
        } 

    }
}
