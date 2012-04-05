using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Scalesque {

    /// <summary>
    /// Represents an optional value.  Is either a Some<T> or a None<T> representing value present or missing respectively.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract partial class Option<T> : IEnumerable<T> {
        
        public abstract bool isEmpty { get; }
        
        public bool hasValue { get { return !isEmpty; } }

        public abstract T get();
        
        public Option<U> map<U>(Func<T, U> f) {
            if (isEmpty)
                return None<U>.apply();
            return new Some<U>(f(get()));
        }

        public T getOrElse(Func<T> f) {
            if (isEmpty)
                return f();
            return get();
        }

        public Option<U> flatMap<U>(Func<T, Option<U>> f) {
            if (isEmpty)
                return Option.None();
            return f(get());
        }

        /// <summary>
        /// Opposite of flatMap.  Keeps the value if this is Some{T}, else returns the Option{T} of the function.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public Option<T> or(Func<Option<T>> f) {
            if (hasValue)
                return this;
            return f();
        }

        public IEnumerator<T> GetEnumerator() {
            if (isEmpty)
                return new List<T>().GetEnumerator();
            return new List<T> {get()}.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }

    public sealed class None<T> : Option<T> {
        public override bool isEmpty {
            get { return true; }
        }

        public override T get() {
            throw new ArgumentNullException("Get called on None");
        }

        public static Option<T> apply() {
            return new None<T>();
        }

        private None() {}

        //todo: None equality

    }

    public sealed class Some<T> : Option<T> {
        private readonly T value;

        public Some(T value) {
            this.value = value;
        }

        public override bool isEmpty {
            get { return false; }
        }

        public override T get() {
            return value;
        }
    }

    /// <summary>
    /// Companion class for Option
    /// </summary>
    public static class Option {

        /// <summary>
        /// Creates an  <see cref="Option{T}"/>.  Result be <see cref="Some{T}"/> if the reference is not null else will be <see cref="None{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Option<T> apply<T>(T value) {
            if (value == null)
                return None();
            return new Some<T>(value);
        }

        

        /// <summary>
        /// Converts a reference to T an <see cref="Option{T}"/>.  Result be <see cref="Some{T}"/> if the reference is not null else will be <see cref="None{T}"/>.
        /// 
        /// Implicit method for <see cref="Option.apply{T}"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Option<T> opt<T>(this T value) {
            return apply(value);
        }

        public static Option<T> opt<T>(this Nullable<T> value) where T:struct {
            if (value.HasValue)
                return apply(value.Value);
            return None();
        }

        /// <summary>
        /// Flattens an IEnenumerable{Option{T}} to a IEnenumerable{T}
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static IEnumerable<T> flatten<T>(this IEnumerable<Option<T>> enumerable) {
            return from option in enumerable where option.hasValue select option.get();
        }

        /// <summary>
        /// Convenience method for creating None{T}
        /// </summary>
        /// <returns>None.  implicitly converted to None{T}</returns>
        public static None None() {
            return new None();
        }

        public static Option<T> Some<T>(T value) {
            return apply(value);
        }
    }

}
