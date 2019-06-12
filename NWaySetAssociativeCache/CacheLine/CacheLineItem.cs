
namespace TradeDeskNWaySetAssociativeCache
{
    public class CacheLineItem<K, V>
    {
        public CacheLineItem(K k, V v)
        {
            Tag = k;
            Data = v;
        }
        // Disable modification by clients of the lib
        public K Tag { get; internal set; } 
        public V Data { get; internal set; }
    }
}
