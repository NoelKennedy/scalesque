using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Scalesque {

    /// <summary>
    /// Represents an optional value.  Is either a Some{T} or a None{T} representing value present or missing respectively.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract partial class Option<T> : IEnumerable<T> {
        
        public abstract bool IsEmpty { get; }
        
        public bool HasValue { get { return !IsEmpty; } }

        public abstract T Get();
        
        public Option<U> Map<U>(Func<T, U> f) {
            if (IsEmpty)
                return None<U>.apply();
            return new Some<U>(f(Get()));
        }

        public T GetOrElse(Func<T> f) {
            if (IsEmpty)
                return f();
            return Get();
        }

        public Option<U> FlatMap<U>(Func<T, Option<U>> f) {
            if (IsEmpty)
                return Option.None();
            return f(Get());
        }

        /// <summary>
        /// Opposite of flatMap.  Keeps the value if this is Some{T}, else returns the Option{T} of the function.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public Option<T> Or(Func<Option<T>> f) {
            if (HasValue)
                return this;
            return f();
        }

        public IEnumerator<T> GetEnumerator() {
            if (IsEmpty)
                return new List<T>().GetEnumerator();
            return new List<T> {Get()}.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }

    public sealed class None<T> : Option<T> {
        public override bool IsEmpty {
            get { return true; }
        }

        public override T Get() {
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

        public override bool IsEmpty {
            get { return false; }
        }

        public override T Get() {
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
        public static Option<T> Opt<T>(this T value) {
            return apply(value);
        }

        public static Option<T> Opt<T>(this Nullable<T> value) where T:struct {
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
        public static IEnumerable<T> Flatten<T>(this IEnumerable<Option<T>> enumerable) {
            return from option in enumerable where option.HasValue select option.Get();
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
