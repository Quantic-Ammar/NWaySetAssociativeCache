using TradeDeskNWaySetAssociativeCache;
using NUnit.Framework;

namespace NWaySetAssociativeCacheUnitTests
{
    [TestFixture]
    public class BoundedCacheIndexerTests
    {
        [Test]
        public void ShouldReturnValidBoundedIndexBasedOnCacheSizeAndIndexPassed()
        {
            var indexer = new BoundedCacheIndexer<string>(2);
            var i = indexer.GetCacheIndex("0");
            var j = indexer.GetCacheIndex("1");
            var k = indexer.GetCacheIndex("2");
            var l = indexer.GetCacheIndex(null);
            Assert.That(i == 0);
            Assert.That(j == 1);
            Assert.That(k == 0);
            Assert.That(l == 0);

        }
    }
}
