using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atomic.Core.Collections
{
    public class BaseCollection<T> : MonoBehaviour, IEnumerable<T>, IEnumerable, ISerializationCallbackReceiver
    {
        [SerializeField]
        private bool _clearOnPlay;

        [SerializeField]
        private List<T> list = new List<T>();

        public void Add(T obj)
        {
            list.Add(obj);
        }

        public void AddRange(IEnumerable<T> objs)
        {
            list.AddRange(objs);
        }

        public void ClearAndAddRange(IEnumerable<T> items)
        {
            list.Clear();
            list.AddRange(items);
        }

        public void Remove(T obj)
        {
            Debug.Log("hello");
            list.Remove(obj);
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(T value)
        {
            return list.Contains(value);
        }

        public int IndexOf(T value)
        {
            return list.IndexOf(value);
        }

        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
        }

        public void Insert(int index, T value)
        {
            list.Insert(index, value);
        }

        public T[] ToArray()
        {
            return list.ToArray();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public void OnAfterDeserialize()
        {
        }

        public void OnBeforeSerialize()
        {
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
