using Yomego.CMS.Core.Attributes;
using Yomego.CMS.Core.Umbraco.Model;

namespace Yomego.CMS.Core.Umbraco.Archetypes
{
    [ArchetypeContent]
    public class UrlPicker
    {
        public string Title { get; set; }

        public string Url { get; set; }

        public SimpleLink InternalUrl { get; set; }

        public bool NewWindow { get; set; }

        public string DisplayUrl
        {
            get
            {
                var url = Url;

                if (InternalUrl != null && InternalUrl.HasUrl)
                {
                    url = InternalUrl.Url;
                }

                return url;
            }
        }
    }
}
