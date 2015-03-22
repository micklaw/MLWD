using System.Collections.Generic;
using System.ComponentModel;
using MLWD.Umbraco.Mvc.Attributes;
using MLWD.Umbraco.Mvc.Model.Content;
using MLWD.Umbraco.Mvc.Model.Media;
using MLWD.Umbraco.Umbraco.ModelBuilder.ComponentModel.TypeConverters;
using Umbraco.Core.Models;
using Website.Domain.Home.Models.Archetypes;

namespace Website.Domain.Home.DocTypes
{
    [UmbracoRoute("Home")]
    public class Homepage : Page
    {
        public Homepage(IPublishedContent content) : base(content) { }

        [TypeConverter(typeof(ImageConverter))]
        public Image HeaderBackground { get; set; }

        [TypeConverter(typeof(ArchetypeConverter<IList<Testimonial>>))]
        public IList<Testimonial> Testimonials { get; set; }

        [TypeConverter(typeof(ArchetypeConverter<IList<Models.Archetypes.Service>>))]
        public IList<Models.Archetypes.Service> Services { get; set; }
    }
}
