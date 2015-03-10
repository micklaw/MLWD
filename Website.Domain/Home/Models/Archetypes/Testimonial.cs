using Yomego.CMS.Core.Attributes;
using Yomego.CMS.Core.Umbraco.Model;
using BaseArchetype = Website.Domain.Shared.Models.BaseArchetype;

namespace Website.Domain.Home.Models.Archetypes
{
    [ArchetypeContent]
    public class Testimonial : BaseArchetype
    {
        public string Person { get; set; }
        
        public Image Image { get; set; }

        public string Quote { get; set; }
    }
}