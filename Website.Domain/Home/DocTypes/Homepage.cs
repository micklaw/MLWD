using System.Collections.Generic;
using System.ComponentModel;
using Our.Umbraco.Ditto.Resolvers.Archetype.Attributes;
using Our.Umbraco.Ditto.Resolvers.Archetype.Resolvers;
using Yomego.Umbraco.Mvc.Attributes;
using Yomego.Umbraco.Mvc.Model.Media;
using Umbraco.Core.Models;
using Website.Domain.Home.Models.Archetypes;
using Website.Domain.Shared.DocTypes;
using Yomego.Umbraco.Umbraco.Ditto.TypeConverters;

namespace Website.Domain.Home.DocTypes
{
    [UmbracoRoute("Home")]
    public class Homepage : Page
    {
        public Homepage(IPublishedContent content) : base(content) { }

        [TypeConverter(typeof(ImageConverter))]
        public Image HeaderBackground { get; set; }

        [ArchetypeValueResolver]
        public IList<Testimonial> Testimonials { get; set; }

        [ArchetypeValueResolver]
        public IList<Models.Archetypes.Service> Services { get; set; }
    }
}
