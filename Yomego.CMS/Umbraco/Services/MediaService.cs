using Yomego.CMS.Core.Umbraco.Model;

namespace Yomego.CMS.Umbraco.Services
{
    public abstract class MediaService : BaseService
    {
        #region Abstract Methods

        /// <summary>
        /// Saves the content and publishes if true
        /// </summary>
        public abstract int Save(MediaItem mediaItem);

        public abstract MediaItem Get(int id, bool includeChildren = false);

        #endregion
    }
}
