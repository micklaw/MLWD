using System.Collections.Generic;
using System.ComponentModel;
using Our.Umbraco.Ditto.Resolvers.Archetype.Attributes;
using Yomego.Umbraco.Mvc.Attributes;
using Yomego.Umbraco.Mvc.Model.Media;
using Umbraco.Core.Models;
using Website.Domain.Shared.DocTypes;
using Yomego.Umbraco.Umbraco.Ditto.TypeConverters;

namespace Website.Domain.Service.DocTypes
{
    [UmbracoRoute("Services")]
    public class Services : Page
    {
        public Services(IPublishedContent content) : base(content) { }

        [TypeConverter(typeof(ImageConverter))]
        public Image ServiceImage { get; set; }

        [ArchetypeValueResolver]
        public IList<Home.Models.Archetypes.Service> ServicesList { get; set; }
    }
}
