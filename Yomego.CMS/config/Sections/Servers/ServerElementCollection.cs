using System.Configuration;

namespace Yomego.CMS.Config.Sections.Servers
{
    public class ServerElementCollection : ConfigurationElementCollection
    {
        public ServerSectionElement this[int index]
        {
            get { return (ServerSectionElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }

                BaseAdd(index, value);
            }
        }

        public void Add(ServerSectionElement serverConfig)
        {
            BaseAdd(serverConfig);
        }

        public void Clear()
        {
            BaseClear();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ServerSectionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ServerSectionElement)element).Key;
        }

        public string GetElementUrl(string key)
        {
            return ((ServerSectionElement)BaseGet(key)).Url;
        }

        public void Remove(ServerSectionElement serverConfig)
        {
            BaseRemove(serverConfig.Key);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }
    }
}
