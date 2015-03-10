using Yomego.Loyalty.CMS.Plugins.Umbraco.jumps.umbraco.usync.Interfaces;
using jumps.umbraco.usync;

namespace jumps.umbraco.usync
{
    public class uSyncService : IuSyncService
    {
        public void Sync()
        {
            var usync = new uSync();

            usync.DoSync(RunMode.Manual, true, false, false);
        }

        public void Save()
        {
            var usync = new uSync();

            usync.DoSync(RunMode.Manual, false, true, false);
        }
    }
}
