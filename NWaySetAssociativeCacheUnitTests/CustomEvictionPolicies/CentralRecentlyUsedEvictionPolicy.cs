using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeDeskNWaySetAssociativeCache;

namespace NWaySetAssociativeCacheUnitTests
{
    public class CentralRecentlyUsedEvictionPolicy<K, V> : IEvictionPolicy<K, V> where K : IConvertible, IEquatable<K>
    {
        public LinkedListNode<CacheLineItem<K, V>> GetItemToEvict(LinkedList<CacheLineItem<K, V>> lineList)
        {
            var centralRecentlyAccessedItemPosition = lineList.Count / 2;
            var curNode = lineList.First;
            for (int i = 0; i < centralRecentlyAccessedItemPosition; i++)
                curNode = curNode.Next;
            return curNode;

        }

    }
}
