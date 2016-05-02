using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DLeh.Util
{
    public static class FuncUtil
    {

        public static Func<A, R> Memoize<A, R>(Func<A, R> f)
        {
            var cache = new ConcurrentDictionary<A, Lazy<R>>();
            return a => cache.GetOrAdd(a, new Lazy<R>(() => f(a))).Value;
        }

        public static Func<A, B, R> Memoize<A, B, R>(this Func<A, B, R> f)
        {
            var map = DictionaryHelper<Lazy<R>>.CreateConcurrent(new { a = default(A), b = default(B) });
            return (a, b) => map.GetOrAdd(new { a, b }, new Lazy<R>(() => f(a, b))).Value;
        }

        public static Func<A, B, C, R> Memoize<A, B, C, R>(this Func<A, B, C, R> f)
        {
            var map = DictionaryHelper<Lazy<R>>.CreateConcurrent(new { a = default(A), b = default(B), c = default(C) });
            return (a, b, c) => map.GetOrAdd(new { a, b, c }, new Lazy<R>(() => f(a, b, c))).Value;
        }

        public static Func<A, B, C, D, R> Memoize<A, B, C, D, R>(this Func<A, B, C, D, R> f)
        {
            var map = DictionaryHelper<Lazy<R>>.CreateConcurrent(new { a = default(A), b = default(B), c = default(C), d = default(D) });
            return (a, b, c, d) => map.GetOrAdd(new { a, b, c, d }, new Lazy<R>(() => f(a, b, c, d))).Value;
        }



        #region Time Sensitive

        static ICacheTimeoutProvider cacheTimeoutProvider = new SixHourCacheTimeoutProvider();
        public static readonly TimeSpan DefaultValidTime = cacheTimeoutProvider.InvalidAfter;

        public static Func<R> MemoizeTimeSensitive<R>(Func<R> f, TimeSpan validTime = default(TimeSpan))
        {
            var key = 1; //use 1 as a dummy key
            var map = DictionaryHelper<Lazy<R>>.CreateTimeSensitive(key, validTime);
            return () => map.GetOrAdd(key, new Lazy<R>(() => f())).Value;
        }

        public static Func<A, R> MemoizeTimeSensitive<A, R>(Func<A, R> f, TimeSpan validTime = default(TimeSpan))
        {
            var map = DictionaryHelper<Lazy<R>>.CreateTimeSensitive(new { a = default(A) }, validTime);
            return a => map.GetOrAdd(new { a }, new Lazy<R>(() => f(a))).Value;
        }

        public static Func<A, B, R> MemoizeTimeSensitive<A, B, R>(this Func<A, B, R> f, TimeSpan validTime = default(TimeSpan))
        {
            var map = DictionaryHelper<Lazy<R>>.CreateTimeSensitive(new { a = default(A), b = default(B) }, validTime);
            return (a, b) => map.GetOrAdd(new { a, b }, new Lazy<R>(() => f(a, b))).Value;
        }

        public static Func<A, B, C, R> MemoizeTimeSensitive<A, B, C, R>(this Func<A, B, C, R> f, TimeSpan validTime = default(TimeSpan))
        {
            var map = DictionaryHelper<Lazy<R>>.CreateTimeSensitive(new { a = default(A), b = default(B), c = default(C) }, validTime);
            return (a, b, c) => map.GetOrAdd(new { a, b, c }, new Lazy<R>(() => f(a, b, c))).Value;
        }

        public static Func<A, B, C, D, R> MemoizeTimeSensitive<A, B, C, D, R>(this Func<A, B, C, D, R> f, TimeSpan validTime = default(TimeSpan))
        {
            var map = DictionaryHelper<Lazy<R>>.CreateTimeSensitive(new { a = default(A), b = default(B), c = default(C), d = default(D) }, validTime);
            return (a, b, c, d) => map.GetOrAdd(new { a, b, c, d }, new Lazy<R>(() => f(a, b, c, d))).Value;
        }

        #endregion Time Sensitive

        static class DictionaryHelper<Value>
        {
            public static Dictionary<Key, Value> Create<Key>(Key prototype)
            {
                return new Dictionary<Key, Value>();
            }

            public static ConcurrentDictionary<Key, Value> CreateConcurrent<Key>(Key prototype)
            {
                return new ConcurrentDictionary<Key, Value>();
            }

            public static TimeSensitiveConcurrentDictionary<Key, Value> CreateTimeSensitive<Key>(Key prototype, TimeSpan timeSpan = default(TimeSpan))
            {
                timeSpan = timeSpan == default(TimeSpan) ? DefaultValidTime : timeSpan;
                return new TimeSensitiveConcurrentDictionary<Key, Value>(timeSpan);
            }
        }




        //a loosely-typed cache of memoized funcs so that we don't need a different one for each func type.
        static ConcurrentDictionary<string, object> funcCache = new ConcurrentDictionary<string, object>();

        /// <summary>
        /// Returns the value of the function with the provided arguments. Returns a cached result if available.
        /// </summary>
        public static R RunMemoized<A, R>(string key, A a, Func<A, R> func, TimeSpan validTime = default(TimeSpan), bool cache = true)
        {
            if (!cache)
                return func(a);
            var cached = (Func<A, R>)funcCache.GetOrAdd(key, MemoizeTimeSensitive(func, validTime));
            return cached(a);
        }
        public static R RunMemoized<A, B, R>(string key, A a, B b, Func<A, B, R> func, TimeSpan validTime = default(TimeSpan))
        {
            var cached = (Func<A, B, R>)funcCache.GetOrAdd(key, MemoizeTimeSensitive(func, validTime));
            return cached(a, b);
        }
        public static R RunMemoized<A, B, C, R>(string key, A a, B b, C c, Func<A, B, C, R> func, TimeSpan validTime = default(TimeSpan))
        {
            var cached = (Func<A, B, C, R>)funcCache.GetOrAdd(key, MemoizeTimeSensitive(func, validTime));
            return cached(a, b, c);
        }

        public static void ClearCache()
        {
            funcCache = new ConcurrentDictionary<string, object>();
        }
        public static bool ClearCachedFunction(string key)
        {
            object temp;
            return funcCache.TryRemove(key, out temp);
        }
        public static List<string> CurrentCachedFunctions => funcCache.Keys.ToList();
    }

    //TODO: allow injecting this value. Currently 
    public interface ICacheTimeoutProvider
    {
        TimeSpan InvalidAfter { get; }
    }

    public class ConfigurableCacheTimeoutProvider : ICacheTimeoutProvider
    {
        public ConfigurableCacheTimeoutProvider(TimeSpan invalidAfter)
        {
            _invalidAfter = invalidAfter;
        }
        private TimeSpan _invalidAfter;
        public TimeSpan InvalidAfter { get { return _invalidAfter; } }
    }

    public class SixHourCacheTimeoutProvider : ConfigurableCacheTimeoutProvider
    {
        public SixHourCacheTimeoutProvider()
            : base(TimeSpan.FromHours(6))
        { }
    }

    //public static class FuncCacher<A, TOut>
    //{
    //    static Dictionary<string, Func<A, TOut>> cache = new Dictionary<string, Func<A, TOut>>();

    //    public static TOut GetCachedOrRun(string key, A input, Func<A, TOut> func)
    //    {
    //        if (!cache.ContainsKey(key))
    //        {
    //            cache.Add(key, FuncUtil.MemoizeTimeSensitive(func));
    //        }
    //        return cache[key](input);
    //    }
    //}
}
