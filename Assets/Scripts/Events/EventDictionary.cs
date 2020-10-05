using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace UnityTools
{
    public class EventDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private readonly Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
        public readonly ItemEvent ItemAdded = new ItemEvent();
        public readonly ItemEvent ItemRemoved = new ItemEvent();

        public void AddItem(TKey id, TValue item)
        {
            if (dict.ContainsKey(id))
            {
                dict.Remove(id);
            }
            dict.Add(id, item);
            ItemAdded.Invoke(id, item);
        }

        /**
         * If the item exists in the dictionary, perform an action. Otherwise, listen for it and perform it next time it gets added
         */
        public void DoOrListen(TKey key, Action<TValue> action)
        {
            if (ContainsKey(key))
            {
                action(dict[key]);
            }
            else
            {
                ItemAdded.AddListener((id, item) =>
                {
                    if (id.Equals(key))
                    {
                        action(item);
                    }
                });
            }
        }

        public void Remove(TKey id)
        {
            if (dict.ContainsKey(id))
            {
                var item = dict[id];
                dict.Remove(id);
                ItemRemoved.Invoke(id, item);
            }
        }

        public bool ContainsKey(TKey id)
        {
            return dict.ContainsKey(id);
        }

        public TValue this[TKey i]
        {
            get { return dict[i]; }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TKey, TValue>>)dict).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TKey, TValue>>)dict).GetEnumerator();
        }

        [Serializable]
        public class ItemEvent : UnityEvent<TKey, TValue> { }
    }
}