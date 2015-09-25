using System.ComponentModel;
using Archetype.Models;
using Yomego.Umbraco.Mvc.Model.Media;
using Yomego.Umbraco.Umbraco.Ditto.TypeConverters;

namespace Website.Domain.Home.Models.Archetypes
{
    public class Testimonial : ArchetypeFieldsetModel
    {
        public string Person { get; set; }

        [TypeConverter(typeof(ImageConverter))]
        public Image Image { get; set; }

        public string Quote { get; set; }
    }
}