using System.Collections.Generic;
using System.ComponentModel;
using MLWD.Umbraco.Mvc.Attributes;
using MLWD.Umbraco.Umbraco.ModelBuilder.ComponentModel.TypeConverters;
using Umbraco.Core.Models;
using Website.Domain.Home.Models.Archetypes;
using Website.Domain.Shared.DocTypes;

namespace Website.Domain.Home.DocTypes
{
    [UmbracoRoute("Home")]
    public class Homepage : Page
    {
        public Homepage(IPublishedContent content) : base(content) { }

        [TypeConverter(typeof(ArchetypeConverter<IList<Testimonial>>))]
        public IList<Testimonial> Testimonials { get; set; }
    }
}
