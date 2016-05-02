using System;
using System.Collections.Concurrent;

namespace DLeh.Util
{

    /// <summary>
    /// Stores a value based on the key for a set amoutn of time. After that time has passed, accessing the same key will remove the value
    /// </summary>
    /// <typeparam name="TKey">Type to use as a key</typeparam>
    /// <typeparam name="TValue">Type to use as a value</typeparam>
    public class TimeSensitiveConcurrentDictionary
    {
        protected static ICurrentDateTimeProvider DateTimeProvider { get; set; }

        static TimeSensitiveConcurrentDictionary()
        {
            SetTimeProvider(new CurrentDateTimeProvider());
        }

        public static void SetTimeProvider(ICurrentDateTimeProvider provider)
        {
            DateTimeProvider = provider;
        }
        public static void ResetTimeProvider()
        {
            SetTimeProvider(new CurrentDateTimeProvider());
        }
    }

    public class TimeSensitiveConcurrentDictionary<TKey, TValue> : TimeSensitiveConcurrentDictionary
    {
        private readonly ConcurrentDictionary<TKey, TimeSensitiveConcurrentDictionaryValue<TValue>> cache
            = new ConcurrentDictionary<TKey, TimeSensitiveConcurrentDictionaryValue<TValue>>();

        private readonly TimeSpan expirationTime;



        public TimeSensitiveConcurrentDictionary(TimeSpan validTime)
        {
            expirationTime = validTime;
        }

        public TValue GetOrAdd(TKey key, TValue newValue)
        {
            var now = TimeSensitiveConcurrentDictionary.DateTimeProvider.Now;
            bool needsToUpdate = false;
            var currentExpirationDate = now;
            if (cache.ContainsKey(key))
            {
                TimeSensitiveConcurrentDictionaryValue<TValue> tempVal;
                if (cache.TryGetValue(key, out tempVal))
                {
                    currentExpirationDate = tempVal.ExpirationDate;
                }
                if (now >= currentExpirationDate)
                {
                    TryRemove(key);
                    needsToUpdate = true;
                }
            }
            else
            {
                needsToUpdate = true;
            }
            if (needsToUpdate)
            {
                currentExpirationDate = now.Add(expirationTime);
            }

            return cache.GetOrAdd(key, new TimeSensitiveConcurrentDictionaryValue<TValue>(currentExpirationDate, newValue)).Value;
        }
        public bool TryRemove(TKey a)
        {
            TimeSensitiveConcurrentDictionaryValue<TValue> result;
            return cache.TryRemove(a, out result);
        }

        private struct TimeSensitiveConcurrentDictionaryValue<TTSValue>
        {
            private DateTime expirationDate;
            private TTSValue value;

            public DateTime ExpirationDate { get { return expirationDate; } }
            public TTSValue Value { get { return value; } }

            public TimeSensitiveConcurrentDictionaryValue(DateTime expirationDate, TTSValue value)
            {
                this.expirationDate = expirationDate;
                this.value = value;
            }
        }
    }

    public interface ICurrentDateTimeProvider
    {
        DateTime Now { get; }
    }

    public class CurrentDateTimeProvider : ICurrentDateTimeProvider
    {
        public DateTime Now { get { return DateTime.Now; } }
    }
}
