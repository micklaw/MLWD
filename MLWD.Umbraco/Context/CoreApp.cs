using MLWD.Umbraco.Mvc.Model.Content;
using MLWD.Umbraco.Umbraco.Services.Container;

namespace MLWD.Umbraco.Context
{
    public class CoreApp<TServiceContainer> : Container where TServiceContainer : CoreServiceContainer, new()
    {
        public TServiceContainer Services { get { return Get<TServiceContainer>(); } }

        public WebContext Context { get { return Get<WebContext>(); } }

        private Settings _settings { get; set; }

        public Settings Settings 
        {
            get
            {
                if (_settings == null)
                {
                    _settings = Services.Content.First<Settings>();
                }

                return _settings;
            }
        } 
    }
}
