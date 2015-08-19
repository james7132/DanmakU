using System;
using System.Collections;
using System.Collections.Generic;

namespace Hourai {

    /// <summary>
    /// An generic iterable priority queue.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PriorityList<T> : ICollection<T> {

        // The actual collection. Maps a priority to a bucket of items at that priority.
        // This is a solution to the fact that SortedList does not support duplicate keys
        private readonly SortedList<int, List<T>> _items;

        // The priority cache. In exchange for using a bit more memory. This allows for faster checks
        // like in Contains, which would otherwise involve iterating through every priority level and running
        // Contains on every bucket
        private readonly Dictionary<T, int> _priorities;

        /// <summary>
        /// Creates an empty PriorityList instance
        /// </summary>
        public PriorityList() {
            _items = new SortedList<int, List<T>>();
            _priorities = new Dictionary<T, int>();
        }

        /// <summary>
        /// Creates a PriorityList instance using the elements from the collection provided.
        /// All of the elements are assigned to the default priority of 0.
        /// </summary>
        /// <param name="collection"></param>
        /// <exception cref="ArgumentNullException">thrown if collection is null</exception>
        public PriorityList(IEnumerable<T> collection) : this() {
            if (collection == null)
                throw new ArgumentNullException("collection");
            _items.Add(0, new List<T>(collection));
            foreach (var element in collection)
                _priorities[element] = 0;
        }

        /// <summary>
        /// Creates a PriorityList instance using the elements and priorities provided by the dictionary.
        /// Each element is properly mapped to their priority.
        /// </summary>
        /// <param name="priorities"></param>
        public PriorityList(IDictionary<T, int> priorities) {
            if(priorities == null)
                throw new ArgumentNullException("priorities");
            _priorities = new Dictionary<T, int>(priorities);
            foreach (var priority in priorities)
                GetOrCreateBucket(priority.Value).Add(priority.Key);
        }

        #region ICollection Implementation
        public IEnumerator<T> GetEnumerator() {
            foreach(KeyValuePair<int, List<T>> pair in _items) {
                List<T> bucket = pair.Value;
                for (var j = 0; j < bucket.Count; j++)
                    yield return bucket[j];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public void Add(T item) {
            Add(item, 0);
        }

        public void Add(T item, int priority) {
            List<T> bucket = GetOrCreateBucket(priority);
            bucket.Add(item);
            _priorities[item] = priority;
        }

        public void Clear() {
            _items.Clear();
            _priorities.Clear();
        }

        public bool Contains(T item) {
            return _priorities.ContainsKey(item);
        }

        public void CopyTo(T[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        public bool Remove(T item) {
            if (Contains(item))
                return false;
            int priority = _priorities[item];
            List<T> bucket = _items[priority];
            bool success = bucket.Remove(item);
            if (success)
                _priorities.Remove(item);
            if (bucket.Count <= 0)
                _items.Remove(priority);
            return true;
        }

        public int Count {
            get { return _priorities.Count; }
        }

        public bool IsReadOnly { get { return false; } }
        #endregion

        /// <summary>
        /// Gets the priority of an item stored within an instance of PriorityList.
        /// </summary>
        /// <param name="item">the item to get the priority of</param>
        /// <exception cref="ArgumentException">thrown if the PriorityList does not contain this item</exception>
        /// <returns>the priority of the item within the list</returns>
        public int GetPriority(T item) {
            if(!Contains(item))
                throw new ArgumentException("item");
            return _priorities[item];
        }

        /// <summary>
        /// Gets the priority of an item stored within an instance of PriorityList.
        /// Note this is for items already in
        /// </summary>
        /// <param name="item">the item to edit the</param>
        /// <param name="priority"></param>
        public void SetPriority(T item, int priority) {
            if (!Contains(item))
                return;

            int current = _priorities[item];

            if (priority == current)
                return;

            List<T> currentBucket = _items[current];
            List<T> newBucket = GetOrCreateBucket(priority);
            
            // Remove from the old bucket
           currentBucket.Remove(item);
            // Add to the new bucket
            newBucket.Add(item);

            //Remove old bucket if it's empty
            if (currentBucket.Count <= 0)
                _items.Remove(current);

            // Store the new priority
            _priorities[item] = priority;
        }

        public void RemoveAllByPriority(int priority) {
            if (!_items.ContainsKey(priority))
                return;
            List<T> bucket = _items[priority];
            
            // Remove the bucket
            _items.Remove(priority);

            // Remove all elements in the bucket from the priority cache
            for (var i = 0; i < bucket.Count; i++)
                _priorities.Remove(bucket[i]);
        }

        List<T> GetOrCreateBucket(int priority) {
            List<T> bucket;
            if (_items.ContainsKey(priority))
                bucket = _items[priority];
            else {
                bucket = new List<T>();
                _items.Add(priority, bucket);
            }
            return bucket;
        }
    }

}