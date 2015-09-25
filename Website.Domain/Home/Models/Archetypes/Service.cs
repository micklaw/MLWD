using System.ComponentModel;
using System.Web;
using Archetype.Models;
using Yomego.Umbraco.Mvc.Model.Media;
using Yomego.Umbraco.Umbraco.Ditto.TypeConverters;

namespace Website.Domain.Home.Models.Archetypes
{
    public class Service : ArchetypeFieldsetModel
    {
        [TypeConverter(typeof(ImageConverter))]
        public Image Image { get; set; }

        public string Title { get; set; }

        [TypeConverter(typeof(HtmlStringConverter))]
        public HtmlString Description { get; set; }
    }
}