using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace Greenleaf.Phone
{
    public class LocatorBase
    {
        private static readonly Dictionary<Type, object> _cache = new Dictionary<Type, object>();

        public IUnityContainer InjectionModule { get; set; }

        protected T Resolve<T>(bool singleton = false)
        {
            if (!singleton)
            {
                var instance = GetInstance<T>();

                return instance;
            }

            if (!_cache.ContainsKey(typeof (T)))
            {
                var instance = GetInstance<T>();

                _cache.Add(typeof (T), instance);
            }

            return (T) _cache[typeof (T)];
        }

        private T GetInstance<T>()
        {
            try
            {
                return InjectionModule.Resolve<T>();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Can't resolve {0} type.".FormatWith(typeof (T)), ex);
            }
        }
    }
}