using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Greenleaf
{
    public static class ObjectExtensions
    {
        public static IDictionary<string, object> ToDictionary(this object @object)
        {
            // in case of "Attempt to access the method failed" exception
            // because of anonymous types compiles as internal classes
            // use InternalsVisibleToAttribute in your asseblies
            return @object.GetType().GetRuntimeProperties().ToDictionary(x => x.Name, x => x.GetValue(@object, null));
        }

        public static void LoadTo(this object from, object to)
        {
            var query = from p1 in @from.GetType().GetRuntimeProperties()
                       from p2 in @to.GetType().GetRuntimeProperties()
                       where p1.Name.IsEquals(p2.Name) && p2.CanWrite && !p2.GetIndexParameters().Any()
                       select new { p1, p2 };

            query.ForEach(x=>x.p2.SetValue(to, x.p1.GetValue(@from)));
        }
    }
}