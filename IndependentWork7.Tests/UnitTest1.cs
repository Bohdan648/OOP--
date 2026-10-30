using System;
using System.Collections.Generic;
using Xunit;
using IndependentWork7;

namespace IndependentWork7.Tests
{
    public class CacheTests
    {
        [Fact]
        public void AddAndGet_ValidElement_ReturnsCorrectValue()
        {
            var cache = new Cache<int, string>(3);

            cache.Add(1, "TestValue");

            Assert.Equal("TestValue", cache.Get(1));
        }

        [Fact]
        public void ContainsKey_CheckPresence_ReturnsExpectedBool()
        {
            var cache = new Cache<string, int>(2);
            cache.Add("A", 100);

            Assert.True(cache.ContainsKey("A"));
            Assert.False(cache.ContainsKey("B"));
        }

        [Fact]
        public void Count_CheckProperty_ReturnsCorrectItemCount()
        {
            var cache = new Cache<int, int>(5);
            Assert.Equal(0, cache.Count);

            cache.Add(1, 10);
            cache.Add(2, 20);

            Assert.Equal(2, cache.Count);
        }

        [Fact]
        public void Get_NonExistingKey_ThrowsKeyNotFoundException()
        {
            var cache = new Cache<string, string>(2);

            Assert.Throws<KeyNotFoundException>(() => cache.Get("NonExistingKey"));
        }

        [Fact]
        public void Add_CapacityExceeded_EvictsOldestElementFifo()
        {
            var cache = new Cache<int, string>(2);

            cache.Add(1, "First");
            cache.Add(2, "Second");
            cache.Add(3, "Third");

            Assert.Equal(2, cache.Count);
            Assert.False(cache.ContainsKey(1));
            Assert.True(cache.ContainsKey(2));
            Assert.True(cache.ContainsKey(3));
        }

        [Fact]
        public void Add_ExistingKey_UpdatesValueWithoutEviction()
        {
            var cache = new Cache<string, string>(2);

            cache.Add("Key1", "Value1");
            cache.Add("Key1", "UpdatedValue1");

            Assert.Equal(1, cache.Count);
            Assert.Equal("UpdatedValue1", cache.Get("Key1"));
        }
    }
}