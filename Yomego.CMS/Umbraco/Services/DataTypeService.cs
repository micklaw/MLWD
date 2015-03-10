using System.Collections.Generic;

namespace Yomego.CMS.Umbraco.Services
{
    public abstract class DataTypeService : BaseService
    {
        #region Abstract Methods

        public abstract Dictionary<string, string> GetPreValue(int id);

        #endregion
    }
}