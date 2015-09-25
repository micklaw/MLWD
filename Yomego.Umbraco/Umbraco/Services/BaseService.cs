using System;
using Yomego.Umbraco.Context;
using Yomego.Umbraco.Umbraco.Services.Container;

namespace Yomego.Umbraco.Umbraco.Services
{
    public class BaseService<T> : CoreService<CoreApp<CoreServiceContainer>> where T : App
    {
        private readonly Lazy<T> _app = new Lazy<T>(Activator.CreateInstance<T>);

        public  new T App => _app.Value;
    }
}
