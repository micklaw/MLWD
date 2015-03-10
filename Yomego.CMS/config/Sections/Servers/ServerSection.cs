using System.Configuration;

namespace Yomego.CMS.Config.Sections.Servers
{
    public class ServerSection : ConfigurationSection
    {
        [ConfigurationProperty("servers", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ServerElementCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public ServerElementCollection Servers
        {
            get
            {
                return (ServerElementCollection)base["servers"];
            }
        }
    }
}
