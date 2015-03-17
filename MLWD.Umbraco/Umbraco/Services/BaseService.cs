using System;
using System.Collections;
using System.Linq;
using MLWD.Umbraco.Context;
using MLWD.Umbraco.Umbraco.Services.Container;

namespace MLWD.Umbraco.Umbraco.Services
{
    public class BaseService :  Service<CoreApp<CoreServiceContainer>>
    {
        protected static bool IsType<T1>(Type type)
        {
            return typeof(T1) == type;
        }

        protected static bool IsIEnumerable(Type type)
        {
            return type.GetInterfaces().Any(t => t == typeof(IEnumerable));
        }
    }
}
