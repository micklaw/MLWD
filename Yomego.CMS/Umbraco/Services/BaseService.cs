using System;
using System.Collections;
using System.Linq;
using Yomego.CMS.Context;

namespace Yomego.CMS.Umbraco.Services
{
    public class BaseService :  Service<CoreApp<CoreServiceContainer>>
    {
        protected static bool IsType<T1>(Type type)
        {
            return typeof(T1) == type;
        }

        protected static bool IsType<T1, T2>(Type type)
        {
            return typeof(T1) == type || typeof(T2) == type;
        }

        protected static bool IsIList(Type type)
        {
            return type.GetInterfaces().Any(t => t == typeof(IList));
        }

        protected static bool IsIEnumerable(Type type)
        {
            return type.GetInterfaces().Any(t => t == typeof(IEnumerable));
        }
    }
}
