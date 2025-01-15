using System.Collections;
using System.Collections.Generic;

namespace ChatAppServer.Utils
{
    public class ThreadSafeList<T> : IEnumerable<T>
    {
        private readonly List<T> _list = new List<T>();
        private readonly object _lock = new object();

        // Add an item to the list
        public void Add(T item)
        {
            lock (_lock)
            {
                _list.Add(item);
            }
        }

        // Remove an item from the list
        public bool Remove(T item)
        {
            lock (_lock)
            {
                return _list.Remove(item);
            }
        }

        // Indexer to get or set items by index
        public T this[int index] // we dont need lock here as this is simply read, to consider remove
        {
            get
            {
                lock (_lock)
                {
                    if (index < 0 || index >= _list.Count)
                    {
                        throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
                    }
                    return _list[index];
                }
            }
            set
            {
                lock (_lock)
                {
                    if (index < 0 || index >= _list.Count)
                    {
                        throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
                    }
                    _list[index] = value;
                }
            }
        }

        // Clear the list
        public void Clear()
        {
            lock (_lock)
            {
                _list.Clear();
            }
        }

        // Get the count of items in the list
        public int Count
        {
            get
            {
                lock (_lock)
                {
                    return _list.Count;
                }
            }
        }


        // Implement IEnumerable<T>
        public IEnumerator<T> GetEnumerator()
        {
            // Use a snapshot to ensure thread safety during enumeration
            List<T> snapshot;
            lock (_lock)
            {
                snapshot = new List<T>(_list);
            }

            return snapshot.GetEnumerator();
        }

        // Implement IEnumerable (non-generic version)
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
