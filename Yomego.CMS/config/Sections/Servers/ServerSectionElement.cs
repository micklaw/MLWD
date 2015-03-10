using System.Configuration;

namespace Yomego.CMS.Config.Sections.Servers
{
    public class ServerSectionElement : ConfigurationElement
    {
        [ConfigurationProperty("Key", IsRequired = true, IsKey = true)]
        public string Key
        {
            get
            {
                return this["Key"] as string;
            }
            set
            {
                this["Key"] = value;
            }
        }

        [ConfigurationProperty("Url", IsRequired = true, IsKey = false)]
        public string Url
        {
            get
            {
                return this["Url"] as string;
            }
            set
            {
                this["Url"] = value;
            }
        }
    }
}
