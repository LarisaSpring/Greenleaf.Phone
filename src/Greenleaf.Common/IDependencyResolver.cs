using System;

namespace Greenleaf
{
    public interface IDependencyResolver
    {
        T Resolve<T>();

        T Resolve<T>(object parameters);

        object Resolve(Type type);

        object Resolve(Type type, object parameters);

        void SetValue<T>(T value);
    }
}