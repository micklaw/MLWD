using System;
using System.Collections;
using System.Linq;
using Yomego.Umbraco.Context;
using Yomego.Umbraco.Umbraco.Services.Container;

namespace Yomego.Umbraco.Umbraco.Services
{
    public class CoreService<T> : Service<T> where T : CoreApp<CoreServiceContainer>
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
