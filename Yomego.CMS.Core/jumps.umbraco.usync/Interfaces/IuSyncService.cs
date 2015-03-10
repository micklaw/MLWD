using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yomego.Loyalty.CMS.Plugins.Umbraco.jumps.umbraco.usync.Interfaces
{
    public interface IuSyncService
    {
        void Sync();

        void Save();
    }
}
