using System;
using System.Collections.Generic;
using System.Linq;

namespace Greenleaf
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> Distinct<T, TIdentity>(this IEnumerable<T> source, Func<T, TIdentity> identitySelector)
        {
            return source.Distinct(new DelegateComparer<T, TIdentity>(identitySelector));
        }

        private class DelegateComparer<T, TIdentity> : IEqualityComparer<T>
        {
            private readonly Func<T, TIdentity> _identitySelector;

            public DelegateComparer(Func<T, TIdentity> identitySelector)
            {
                this._identitySelector = identitySelector;
            }

            public bool Equals(T x, T y)
            {
                return Equals(_identitySelector(x), _identitySelector(y));
            }

            public int GetHashCode(T obj)
            {
                return _identitySelector(obj).GetHashCode();
            }
        }
    }
}
