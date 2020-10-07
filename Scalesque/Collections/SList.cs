﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Scalesque.Collections {

    /// <summary>
    /// An immutable linked list terminating in <see cref="Nil{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SList<T> : IEnumerable<T> {
        public abstract T Head { get; }
        public abstract SList<T> Tail { get; }
        public abstract bool IsEmpty { get; }

        /// <summary>
        /// Gets the length of the list.  O(1).
        /// </summary>
        public abstract int Length { get; }

        public IEnumerator<T> GetEnumerator() {
            return new SListEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public static NonEmptySList<T> operator +(SList<T> tail, T head) {
            return new NonEmptySList<T>(head, tail);
        }
        /// <summary>
        /// Prepends a new head to the list. O(1).
        /// </summary>
        /// <param name="newHead"></param>
        /// <returns>A new instance of SList&lt;T&gt;</returns>
        public NonEmptySList<T> Prepend(T newHead) {
            return this + newHead;
        }

        public Option<NonEmptySList<T>> ToNonEmptyList() {
            return HeadAndTail.unapply(this);
        }

        public static implicit operator SList<T>(Nil nil) {
            return Nil<T>.Instance;
        } 

       
    }

    /// <summary>
    /// Extractor for a list with a head and a tail
    /// </summary>
    public static class HeadAndTail
    {
        public static Option<NonEmptySList<T>> unapply<T>(SList<T> list) {
            return list.IsEmpty ? Option.None() : Option.Some((NonEmptySList<T>)list);
        }
    }

    /// <summary>
    /// An immutable linked list guaranteed to not be empty.  Terminates in <see cref="Nil{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NonEmptySList<T> : SList<T>, ISemiJoin<NonEmptySList<T>> {
        private readonly T value;
        private readonly SList<T> next;
        private readonly int length;

        public NonEmptySList(T value):this(value, Nil<T>.Instance) {}

        public NonEmptySList(T value, SList<T> next) {
            this.value = value;
            this.next = next;
            length = next.Length + 1;
        }

        public override T Head {
            get { return value; }
        }

        public override SList<T> Tail {
            get { return next; }
        }

        public override bool IsEmpty {
            get { return false;}
        }

        public override int Length { get { return length; } }

        /// <summary>
        /// Prepends the other to this until the other SList is Nil
        /// </summary>
        /// <param name="other"></param>
        /// <returns>NonEmptySList&lt;T&gt;</returns>
        private NonEmptySList<T> prependWith(SList<T> other) {
            //i think this construct will allow tail call optimisation in .net 4.0
            //http://stackoverflow.com/questions/491376/why-doesnt-net-c-optimize-for-tail-call-recursion
            return other.Length == 0 ? this : Prepend(other.Head).prependWith(other.Tail);
        }

        /// <summary>
        /// Used for semi joins
        /// </summary>
        NonEmptySList<T> ISemiJoin<NonEmptySList<T>>.Value {
            get { return this; }
        }

        /// <summary>
        /// Joins this SList&lt;T&gt; to another SList&lt;T&gt;
        /// </summary>
        /// <param name="other"></param>
        /// <returns>NonEmptySList&lt;T&gt; a unified list contain T from both lists</returns>
        public NonEmptySList<T> Join(ISemiJoin<NonEmptySList<T>> other) {
            return Prepend(other.Value.Head).prependWith(other.Value.Tail);
        }

        public ISemiJoin<NonEmptySList<T>> SemiJoin(ISemiJoin<NonEmptySList<T>> other) {
            return Join(other);
        }
    }


    /// <summary>
    /// Allows Nils of different types to detect and equal eachother
    /// </summary>
    internal interface INil{}

    /// <summary>
    /// The node at the end of a linked list.  All NiL&lt;T&gt; instances are equal to each other.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class Nil<T> : SList<T>, INil {
        private Nil() {}

        static Nil() {
            Instance = new Nil<T>();
        }

        public static Nil<T> Instance { get; private set; }

        public override bool Equals(object obj) {
            var other = obj as INil;
            return other != null;
        }

        public override T Head {
            get { throw new IndexOutOfRangeException("Head called on Nil"); }
        }

        public override SList<T> Tail {
            get { throw new IndexOutOfRangeException("Tail called on Nil"); }
        }

        public override bool IsEmpty {
            get { return true; }
        }

        public override int Length {
            get {return 0; }
        }

        public static SList<T> operator +(T head, Nil<T> tail) {
            return new NonEmptySList<T>(head, tail);
        }

        public static implicit operator Nil<T>(Nil nil) {
            return Instance;
        } 
    }

    internal class SListEnumerator<T> : IEnumerator<T> {
        private readonly SList<T> first;
        private SList<T> current;

        public SListEnumerator(SList<T> first) {
            this.first = first;
        }

        public bool MoveNext() {
            if (current == null)
                current = first;
            else {
                current = current.Tail;
            }

            return !current.IsEmpty;
        }

        public void Reset() {
            current = null;
        }

        public T Current {
            get { return current.Head; }
        }

        #region Blah
        public void Dispose() { }
        object IEnumerator.Current {
            get { return Current; }
        }
        #endregion Blah
    }

    /// <summary>
    /// Companion object for <see cref="SList{T}"/>
    /// </summary>
    public static class SList {
        public static SList<T> apply<T>(T head) {
            return new NonEmptySList<T>(head, Nil<T>.Instance);
        }

        public static SList<T> apply<T>(params T[] instances) {
            SList<T> head = Nil<T>.Instance;
            for (int i = instances.Length - 1; i >= 0; i--) {
                head += instances[i];
            }
            return head;
        }

        public static SList<T> apply<T>(IEnumerable<T> enumerable) {
            return apply(enumerable.ToArray());
        }

        /// <summary>
        /// Prepens the value to an SList
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static NonEmptySList<T> cons<T>(this T value, SList<T> list) {
            return list.Prepend(value);
        }
    }

    public class Nil
    {
        public static Nil apply() {
            return new Nil();
        }
     
        /// <summary>
        /// An extractor for null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool unapply<T>(SList<T> list) {
            return list.ToNonEmptyList().IsEmpty;
        }
    }
}

