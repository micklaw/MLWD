using System.Collections.Generic;
using System.ComponentModel;
using MLWD.Umbraco.Mvc.Attributes;
using MLWD.Umbraco.Mvc.Model.Content;
using MLWD.Umbraco.Mvc.Model.Media;
using MLWD.Umbraco.Umbraco.ModelBuilder.ComponentModel.TypeConverters;
using Umbraco.Core.Models;

namespace Website.Domain.Service.DocTypes
{
    [UmbracoRoute("Services")]
    public class Services : Page
    {
        public Services(IPublishedContent content) : base(content) { }

        [TypeConverter(typeof(ImageConverter))]
        public Image ServiceImage { get; set; }

        [TypeConverter(typeof(ArchetypeConverter<IList<Home.Models.Archetypes.Service>>))]
        public IList<Home.Models.Archetypes.Service> ServicesList { get; set; }
    }
}
