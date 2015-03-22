using System.ComponentModel;
using System.Web;
using Archetype.Models;
using MLWD.Umbraco.Mvc.Model.Media;
using MLWD.Umbraco.Umbraco.ModelBuilder.ComponentModel.TypeConverters;

namespace Website.Domain.Home.Models.Archetypes
{
    public class Service : ArchetypeFieldsetModel
    {
        [TypeConverter(typeof(ImageConverter))]
        public Image Image { get; set; }

        public string Title { get; set; }

        public IHtmlString Description { get; set; }
    }
}