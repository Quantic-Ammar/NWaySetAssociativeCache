
namespace TradeDeskNWaySetAssociativeCache
{
    public interface ICacheIndexer<T>
    {
        int GetCacheIndex(T key);
    }
}
