using System;
using System.Collections;
using System.Collections.Generic;

namespace Scalesque {

    /// <summary>
    /// A list that is guaranteed to be non-empty
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NonEmptyList<T> : IList<T> {

        readonly List<T> inner = new List<T>();

        /// <summary>
        /// Allows private creation of a temporarily empty list
        /// </summary>
        private NonEmptyList() {}


        public NonEmptyList(T head) {
            inner.Add(head);
        }

        public NonEmptyList(IEnumerable<T> enumerable) {
            enumerable.ForEach(inner.Add);
            if(inner.Count == 0)
                throw new ArgumentOutOfRangeException("NonEmptyList initialised with empty IEnumerable<T>");
        }

        public IEnumerator<T> GetEnumerator() {
            return inner.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public void Add(T item) {
            inner.Add(item);
        }

        public void Clear() {
            throw new NotImplementedException();
        }

        public bool Contains(T item) {
            return inner.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex) {
            inner.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item) {
            if (inner.Count > 2)
                return inner.Remove(item);
            else throw new ArgumentOutOfRangeException("Can't remove last element from a non empty list");
        }

        public int Count {
            get { return inner.Count; }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public int IndexOf(T item) {
            return inner.IndexOf(item);
        }

        public void Insert(int index, T item) {
            inner.Insert(index, item);
        }

        public void RemoveAt(int index) {
            if(inner.Count == 1 && index == 0)
                throw new ArgumentOutOfRangeException("Can't remove last element from a non empty list");
            else inner.RemoveAt(index);
        }

        public T this[int index] {
            get { return inner[index]; }
            set { inner[index] = value; }
        }

        /// <summary>
        /// Creates a new NEL with all members of both lists
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public NonEmptyList<T> Merge(NonEmptyList<T> other) {
            var newList = new NonEmptyList<T>();
            inner.ForEach(newList.Add);
            other.ForEach(newList.Add);
            return newList;
        }
    }
}
