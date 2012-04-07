using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scalesque {
    public sealed class LeftProjection<T, U> {
        private readonly Either<T, U> either;
        private readonly Func<T> getLeft;

        public LeftProjection(Either<T, U> either, Func<T> getLeft) {
            this.either = either;
            this.getLeft = getLeft;
        }

        public T Get() {
            return getLeft();
        }

        public Option<T> ToOption() {
            return either.Fold(left => Option.Some(left), right => Option.None());
        }
    }
}
