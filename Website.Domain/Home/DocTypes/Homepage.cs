using System.Collections.Generic;
using System.ComponentModel;
using Our.Umbraco.Ditto.Resolvers.Archetype.Attributes;
using Yomego.Umbraco.Mvc.Attributes;
using Yomego.Umbraco.Mvc.Model.Media;
using Umbraco.Core.Models;
using Website.Domain.Home.Models.Archetypes;
using Website.Domain.Shared.Ditto.TypeConverters;
using Website.Domain.Shared.DocTypes;

namespace Website.Domain.Home.DocTypes
{
    [UmbracoRoute("Home")]
    public class Homepage : Page
    {
        public Homepage(IPublishedContent content) : base(content) { }

        [TypeConverter(typeof(ImageConverter))]
        public Image HeaderBackground { get; set; }

        [ArchetypeResolver]
        public IList<Testimonial> Testimonials { get; set; }

        [ArchetypeResolver]
        public IList<Models.Archetypes.Service> Services { get; set; }
    }
}
