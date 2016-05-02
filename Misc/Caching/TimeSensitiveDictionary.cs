using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DLeh.Util
{
    public class TimeSensitiveDictionary<K, V>
    {
        private Dictionary<K, Tuple<DateTime, V>> valueDict;


        private TimeSpan validTime;
        public TimeSpan ValidTime { get { return validTime; } }

        public TimeSensitiveDictionaryResetScheme resetScheme;
        public TimeSensitiveDictionaryResetScheme ResetScheme { get { return resetScheme; } }

        public TimeSensitiveDictionary(TimeSpan validDuration) : this(validDuration, TimeSensitiveDictionaryResetScheme.None) { }
        public TimeSensitiveDictionary(TimeSpan validDuration, TimeSensitiveDictionaryResetScheme resetScheme)
        {
            valueDict = new Dictionary<K, Tuple<DateTime, V>>();

            validTime = validDuration;
            this.resetScheme = resetScheme;
        }

        public V this[K key]
        {
            get
            {
                var tup = valueDict[key];

                var validThroughTime = tup.Item1;
                var val = tup.Item2;

                if (DateTime.Now > validThroughTime)
                    return default(V);

                if (ResetScheme == TimeSensitiveDictionaryResetScheme.ResetValidDurationOnValidAccess)
                    SetValue(key, val);

                return val;
            }
            set
            {
                SetValue(key, value);
            }
        }

        public void SetValue(K key, V value)
        {
            var validDate = DateTime.Now.Add(validTime);
            valueDict[key] = new Tuple<DateTime, V>(validDate, value);
        }

        public void ClearKey(K key)
        {
            valueDict[key] = null;
        }

        public bool ContainsKey(K key)
        {
            return valueDict.ContainsKey(key);
        }
    }

    public enum TimeSensitiveDictionaryResetScheme
    {
        None,
        ResetValidDurationOnValidAccess
    }
}
