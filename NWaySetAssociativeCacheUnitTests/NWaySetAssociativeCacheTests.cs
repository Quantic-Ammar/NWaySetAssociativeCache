using NUnit.Framework;
using System.Collections.Generic;
using TradeDeskNWaySetAssociativeCache;
using Moq;
using System;
using NWaySetAssociativeCacheUnitTests;

namespace TradeDeskNWaySetAssociativeCacheUnitTests
{
    [TestFixture]
    public class NWaySetAssociativeCacheTests
    {
        [Test]
        public void GetShouldThrowKeyNotFoundExceptionOnMissingKey()
        {
            INWaySetAssociativeCache<int, int, LRUEvictionPolicy<int, int>> cache = new NWaySetAssociativeCache<int, int, LRUEvictionPolicy<int, int>>(1, new BoundedCacheIndexer<int>(1));
           
            Assert.Throws<KeyNotFoundException>(() => cache.Get(1));
        }

        [Test]
        public void GetShouldReturnTheValidDataIfKeyExists()
        {
            INWaySetAssociativeCache<int, int, LRUEvictionPolicy<int, int>> cache = new NWaySetAssociativeCache<int, int, LRUEvictionPolicy<int, int>>(1, new BoundedCacheIndexer<int>(1));
            var key = 2;
            var expectedVal = 4;
            cache.Add(key, expectedVal);
            var val = cache.Get(2);
            Assert.That(val == expectedVal);
        }

        [Test]
        public void GetShouldReturnTheValidDataIfKeyExistsWith2WayCacheContainingTwoItemsInTheSameLine()
        {
            INWaySetAssociativeCache<int, int, LRUEvictionPolicy<int, int>> cache = new NWaySetAssociativeCache<int, int, LRUEvictionPolicy<int, int>>(2, new BoundedCacheIndexer<int>(1));
            var key = 2;
            var expectedVal = 4;
            cache.Add(key, expectedVal);
            cache.Add(3, 5);
            var val = cache.Get(2);
            Assert.That(val == expectedVal);
            
        }

        [Test]
        public void GetShouldReturnTheValidDataIfKeyExistsWith1WayCacheContainingItemsInEachCacheLine()
        {
            Mock<ICacheIndexer<int>> BoundedCacheIndexer = new Mock<ICacheIndexer<int>>();
            BoundedCacheIndexer.Setup(i => i.GetCacheIndex(2)).Returns(0);
            BoundedCacheIndexer.Setup(i => i.GetCacheIndex(3)).Returns(1);

            INWaySetAssociativeCache<int, int, LRUEvictionPolicy<int, int>> cache = new NWaySetAssociativeCache<int, int, LRUEvictionPolicy<int, int>>(1, BoundedCacheIndexer.Object);
            var key = 2;
            var expectedVal = 4;
            cache.Add(key, expectedVal);
            cache.Add(3, 5);
            
            var val = cache.Get(2);
            Assert.That(val == expectedVal);
        }

        [Test]
        public void GetShouldReturnTheValidDataIfKeyExistsWith2WayCacheContaining2ItemsInEachCacheLine()
        {
            Mock<ICacheIndexer<int>> BoundedCacheIndexer = new Mock<ICacheIndexer<int>>();
            BoundedCacheIndexer.Setup(i => i.GetCacheIndex(2)).Returns(0);
            BoundedCacheIndexer.Setup(i => i.GetCacheIndex(3)).Returns(1);
            BoundedCacheIndexer.Setup(i => i.GetCacheIndex(4)).Returns(0);
            BoundedCacheIndexer.Setup(i => i.GetCacheIndex(5)).Returns(1);

            INWaySetAssociativeCache<int, int, LRUEvictionPolicy<int, int>> cache = new NWaySetAssociativeCache<int, int, LRUEvictionPolicy<int, int>>(2, BoundedCacheIndexer.Object);
            cache.Add(3, 30);
            cache.Add(4, 40);
            cache.Add(5, 50);
            var key = 2;
            var expectedVal = 20;
            cache.Add(key, expectedVal);
           

            var val = cache.Get(2);
            Assert.That(val == expectedVal);
        }

        [Test]
        public void AddShouldThrowIfDuplicateKey()
        {
            Mock<ICacheIndexer<int>> BoundedCacheIndexer = new Mock<ICacheIndexer<int>>();
            BoundedCacheIndexer.Setup(i => i.GetCacheIndex(2)).Returns(0);


            INWaySetAssociativeCache<int, int, LRUEvictionPolicy<int, int>> cache = new NWaySetAssociativeCache<int, int, LRUEvictionPolicy<int, int>>(2, BoundedCacheIndexer.Object);
            cache.Add(3, 30);
            Assert.That(cache.Count == 1);
            Assert.Throws<ArgumentException>(() => cache.Add(3, 40));
        }

        [Test]
        public void AddShouldAddItemsIn2WaySetAssociativeCacheWith2CacheLines()
        {
            Mock<ICacheIndexer<int>> BoundedCacheIndexer = new Mock<ICacheIndexer<int>>();
            BoundedCacheIndexer.Setup(i => i.GetCacheIndex(2)).Returns(0);
            BoundedCacheIndexer.Setup(i => i.GetCacheIndex(3)).Returns(1);
            BoundedCacheIndexer.Setup(i => i.GetCacheIndex(4)).Returns(0);
            BoundedCacheIndexer.Setup(i => i.GetCacheIndex(5)).Returns(1);

            INWaySetAssociativeCache<int, int, LRUEvictionPolicy<int, int>> cache = new NWaySetAssociativeCache<int, int, LRUEvictionPolicy<int, int>>(2, BoundedCacheIndexer.Object);
            cache.Add(3, 30);
            cache.Add(4, 40);
            cache.Add(5, 50);
            cache.Add(2, 20);
            
            Assert.That(cache.Get(2) == 20);
            Assert.That(cache.Get(3) == 30);
            Assert.That(cache.Get(4) == 40);
            Assert.That(cache.Get(5) == 50);
            Assert.That(cache.Count == 4);
        }

        [Test]
        public void AddOrUpdateShouldUpdateOnDuplicateKey()
        {
            Mock<ICacheIndexer<int>> BoundedCacheIndexer = new Mock<ICacheIndexer<int>>();
            BoundedCacheIndexer.Setup(i => i.GetCacheIndex(2)).Returns(0);

            INWaySetAssociativeCache<int, int, LRUEvictionPolicy<int, int>> cache = new NWaySetAssociativeCache<int, int, LRUEvictionPolicy<int, int>>(2, BoundedCacheIndexer.Object);
            cache.AddOrUpdate(2, 20);
            Assert.That(cache.Get(2) == 20);
            cache.AddOrUpdate(2, 40);
            Assert.That(cache.Get(2) == 40);

            Assert.That(cache.Count == 1);

        }

        [Test]
        public void CountShouldReportCorrectNumberOfItemsAdded()
        {
            Mock<ICacheIndexer<int>> BoundedCacheIndexer = new Mock<ICacheIndexer<int>>();
            BoundedCacheIndexer.Setup(i => i.GetCacheIndex(2)).Returns(0);

            INWaySetAssociativeCache<int, int, LRUEvictionPolicy<int, int>> cache = new NWaySetAssociativeCache<int, int, LRUEvictionPolicy<int, int>>(2, BoundedCacheIndexer.Object);
            cache.AddOrUpdate(2, 20);
            Assert.That(cache.Get(2) == 20);
            cache.AddOrUpdate(4, 40);
            Assert.That(cache.Get(4) == 40);

            Assert.That(cache.Count == 2);

        }

        [Test]
        public void AddShouldEvictLeastRecentlyUsedItemIfMoreThanNItemsAddedToCacheLineWithLRUEvictionPolicy()
        {
            Mock<ICacheIndexer<int>> BoundedCacheIndexer = new Mock<ICacheIndexer<int>>();
            BoundedCacheIndexer.Setup(i => i.GetCacheIndex(It.IsAny<int>())).Returns(0);
            var N = 3;
            INWaySetAssociativeCache<int, int, LRUEvictionPolicy<int, int>> cache = new NWaySetAssociativeCache<int, int, LRUEvictionPolicy<int, int>>(N, BoundedCacheIndexer.Object);
            cache.Add(1, 10);
            cache.Add(2, 20);
            cache.Add(3, 30);  // lru item
            var data = cache.Get(1); 
            data = cache.Get(2); 
            cache.Add(4, 40);

            Assert.That(cache.Count == 3);
            

            Assert.Throws<KeyNotFoundException>(() => cache.Get(3));
            Assert.That(cache.Get(1) == 10);
            Assert.That(cache.Get(2) == 20);
            Assert.That(cache.Get(4) == 40);
        }

        [Test]
        public void AddShouldEvictMostRecentlyUsedItemIfMoreThanNItemsAddedToCacheLineWithMRUEvictionPolicy()
        {
            Mock<ICacheIndexer<int>> BoundedCacheIndexer = new Mock<ICacheIndexer<int>>();
            BoundedCacheIndexer.Setup(i => i.GetCacheIndex(It.IsAny<int>())).Returns(0);
            var N = 3;
            INWaySetAssociativeCache<int, int, MRUEvictionPolicy<int, int>> cache = new NWaySetAssociativeCache<int, int, MRUEvictionPolicy<int, int>>(N, BoundedCacheIndexer.Object);
            cache.Add(1, 10);
            cache.Add(3, 30); 
            cache.Add(2, 20); // mru item


            cache.Add(4, 40);

            Assert.That(cache.Count == 3);


            Assert.Throws<KeyNotFoundException>(() => cache.Get(2));
            Assert.That(cache.Get(1) == 10);
            Assert.That(cache.Get(3) == 30);
            Assert.That(cache.Get(4) == 40);
        }

        [Test]
        public void AddShouldEvictBasedOnClientSpecifiedCustomEvictionPolicy()
        {
            Mock<ICacheIndexer<int>> BoundedCacheIndexer = new Mock<ICacheIndexer<int>>();
            BoundedCacheIndexer.Setup(i => i.GetCacheIndex(It.IsAny<int>())).Returns(0);
            var N = 3;
            INWaySetAssociativeCache<int, int, CentralRecentlyUsedEvictionPolicy<int, int>> cache = 
                new NWaySetAssociativeCache<int, int, CentralRecentlyUsedEvictionPolicy<int, int>>(N, BoundedCacheIndexer.Object);
            cache.Add(1, 10);
            cache.Add(2, 20);
            cache.Add(3, 30);
            var data = cache.Get(2);
            // order of recently access from least to most recently = 1, 3, 2
            cache.Add(4, 40);

            Assert.That(cache.Count == 3);


            Assert.Throws<KeyNotFoundException>(() => cache.Get(3));
            Assert.That(cache.Get(1) == 10);
            Assert.That(cache.Get(2) == 20);
            Assert.That(cache.Get(4) == 40);
        }

        [Test]
        public void GetShouldReturnTheValidDataIfKeyExistsWithStringKeysAndValues()
        {
            INWaySetAssociativeCache<string, string, LRUEvictionPolicy<string, string>> cache = 
                new NWaySetAssociativeCache<string, string, LRUEvictionPolicy<string, string>>(1, new BoundedCacheIndexer<string>(1));
            var key = "2";
            var expectedVal = "20";
            cache.Add(key, expectedVal);
            var val = cache.Get("2");
            Assert.That(val == expectedVal);
        }

        [Test]
        public void GetShouldReturnTheValidDataIfKeyExistsWithDecimalKeysAndValues()
        {
            INWaySetAssociativeCache<decimal, decimal, LRUEvictionPolicy<decimal, decimal>> cache =
                new NWaySetAssociativeCache<decimal, decimal, LRUEvictionPolicy<decimal, decimal>>(1, new BoundedCacheIndexer<decimal>(1));
            var key = 2;
            var expectedVal = 20;
            cache.Add(key, expectedVal);
            var val = cache.Get(2);
            Assert.That(val == expectedVal);
        }

        [Test]
        public void GetShouldReturnTheValidDataIfKeyExistsWithDecimalKeysAndReferenceValues()
        {
            INWaySetAssociativeCache<decimal, DataReferenceType, LRUEvictionPolicy<decimal, DataReferenceType>> cache =
                new NWaySetAssociativeCache<decimal, DataReferenceType, LRUEvictionPolicy<decimal, DataReferenceType>>(1, new BoundedCacheIndexer<decimal>(1));
            var key = 2;
            var expectedVal = new DataReferenceType(20);
            cache.Add(key, expectedVal);
            var val = cache.Get(2);
            Assert.That(val == expectedVal);
            Assert.That(val.Equals(expectedVal));
            Assert.That(val.TestVal == 20);
        }

        [Test]
        public void GetShouldReturnTheValidDataIfKeyExistsWithReferenceTypeKeysAndValues()
        {
            INWaySetAssociativeCache<ReferenceTypeKey, DataReferenceType, LRUEvictionPolicy<ReferenceTypeKey, DataReferenceType>> cache =
                new NWaySetAssociativeCache<ReferenceTypeKey, DataReferenceType, LRUEvictionPolicy<ReferenceTypeKey, DataReferenceType>>(1, new BoundedCacheIndexer<ReferenceTypeKey>(1));
            var key = new ReferenceTypeKey(2);
            var expectedVal = new DataReferenceType(20);
            cache.Add(key, expectedVal);
            var val = cache.Get(key);
            Assert.That(val == expectedVal);
            Assert.That(val.Equals(expectedVal));
            Assert.That(val.TestVal == 20);
        }

        [Test]
        public void GetShouldReturnTheValidDataIfKeyExistsWithStructTypeKeysAndValues()
        {
            INWaySetAssociativeCache<StructTypeKey, DataStructType, LRUEvictionPolicy<StructTypeKey, DataStructType>> cache =
                new NWaySetAssociativeCache<StructTypeKey, DataStructType, LRUEvictionPolicy<StructTypeKey, DataStructType>>(1, new BoundedCacheIndexer<StructTypeKey>(1));
            var key = new StructTypeKey(2);
            var expectedVal = new DataStructType(20);
            cache.Add(key, expectedVal);
            var val = cache.Get(key);
            Assert.That(val.Equals(expectedVal));
            Assert.That(val.TestVal == 20);
        }
    }
}