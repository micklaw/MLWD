using jumps.umbraco.usync.Interfaces;

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
