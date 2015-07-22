﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Scalesque {

    /// <summary>
    /// Represents an optional value.  Is either a <see cref="Some&lt;T&gt;"/> or a <see cref="None&lt;T&gt;"/> representing value present or missing respectively.
    /// </summary>
    /// <typeparam name="T">&lt;T&gt; The type of the optional value</typeparam>
    public abstract partial class Option<T> : IEnumerable<T> {
        
        /// <summary>
        /// Gets if the optional value is missing
        /// </summary>
        public abstract bool IsEmpty { get; }
        
        /// <summary>
        /// Gets if the optional value is present
        /// </summary>
        public bool HasValue { get { return !IsEmpty; } }

        /// <summary>
        /// Gets the optional value if it is there or throws exception
        /// </summary>
        /// <returns>T</returns>
        public abstract T Get();
        
        /// <summary>
        /// Maps the type of an optional value from &lt;T&gt; to a &lt;U&gt;
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="f"></param>
        /// <returns></returns>
        public Option<U> Map<U>(Func<T, U> f) {
            if (IsEmpty)
                return None<U>.apply();
            return new Some<U>(f(Get()));
        }

        /// <summary>
        /// Gets the value if it exists, else a default value.  Lazy evaulation of default value. 
        /// </summary>
        /// <param name="f">Func&ltT&gt; Allows lazy evaluation. f will only be evaluated if the Option is None</param>
        /// <returns>T</returns>
        public T GetOrElse(Func<T> f) {
            if (IsEmpty)
                return f();
            return Get();
        }

        /// <summary>
        /// Gets the value if it exists, else a default value.  No lazy evaluation of default value.
        /// </summary>
        /// <param name="default">T the default value.  This is evaluated regardless of whether it is used or not</param>
        /// <returns>T</returns>
        public T GetOrElse(T @default) {
            return IsEmpty ? @default : Get();
        }

        /// <summary>
        /// Flattens the value and maps it to an Option&lt;U&gt;
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="f"></param>
        /// <returns></returns>
        public Option<U> FlatMap<U>(Func<T, Option<U>> f) {
            if (IsEmpty)
                return Option.None();
            return f(Get());
        }

        /// <summary>
        /// Opposite of flatMap.  Keeps the value if this is Some&lt;T&gt;, else returns the Option&lt;T&gt; of the function.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public Option<T> Or(Func<Option<T>> f) {
            if (HasValue)
                return this;
            return f();
        }

        /// <summary>
        /// Returns result of ifEmpty if is <see cref="None&lt;T&gt;"/> or passes value to f and returns result
        /// </summary>
        /// <typeparam name="Y"></typeparam>
        /// <param name="ifEmpty"></param>
        /// <param name="f"></param>
        /// <returns>Y</returns>
        public Y Fold<Y>(Func<Y> ifEmpty, Func<T,Y> f) {
            if (IsEmpty)
                return ifEmpty();
            return f(Get());
        }

        /// <summary>
        /// Implements applicative functor for Option&lt;T&gt;
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="tf">Func&ltT,U&gt; tf a function inside the 'context' of an Option which to be applied to this Option</param>
        /// <returns>Option&lt;U&gt; Some&lt;U&gt; if both original Options are Some, else None&lt;U&gt;</returns>
        public Option<U> Applicative<U>(Option<Func<T,U>> tf) {
            return tf.FlatMap<U>(Map);
        }

        public IEnumerator<T> GetEnumerator() {
            if (IsEmpty)
                return new List<T>().GetEnumerator();
            return new List<T> {Get()}.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public override bool Equals(object obj) {
            Option<T> other = (obj as Option<T>).Opt().Flatten();
            Option<Func<T,bool>> applicative = Map<Func<T, bool>>(value => (otherValue) => value.Equals(otherValue));
            return other.Applicative(applicative).GetOrElse(false);
        }

        public override int GetHashCode() {
            return Map(_ => _.GetHashCode()).GetOrElse(base.GetHashCode());
        }
    }

    /// <summary>
    /// Represents a missing optional value
    /// </summary>
    /// <typeparam name="T"></typeparam>
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
    }

    /// <summary>
    /// Represents a present optional value
    /// </summary>
    /// <typeparam name="T"></typeparam>
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
        /// Flattens an IEnumerable&lt;Option&lt;T&gt;&gt; to a IEnenumerable&lt;T&gt;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static IEnumerable<T> Flatten<T>(this IEnumerable<Option<T>> enumerable) {
            return from option in enumerable where option.HasValue select option.Get();
        }

        /// <summary>
        /// Flattens an Option&lt;Option&lt;T&gt;&gt; to a Option&lt;T&gt;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Option<T> Flatten<T>(this Option<Option<T>> option) {
            return option.FlatMap(_ => _);
        }

        /// <summary>
        /// Convenience method for creating None&lt;T&gt;
        /// </summary>
        /// <returns>None.  implicitly converted to None&lt;T&gt;</returns>
        public static None None() {
            return new None();
        }

        /// <summary>
        /// Constructs a Some&lt;T&gt; from a variable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Option<T> Some<T>(T value) {
            return apply(value);
        }

        /// <summary>
        /// Constructs a Some&ltT&gt; from a T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Some<T> ToSome<T>(this T value) {
            return new Some<T>(value);
        }

        /// <summary>
        /// Constructs a None&ltT&gt; from a T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Option<T> ToNone<T>(this T value) {
            return None();
        }
    }
}
