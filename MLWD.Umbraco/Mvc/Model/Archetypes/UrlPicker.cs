using System.ComponentModel;
using Archetype.Models;
using MLWD.Umbraco.Mvc.Model.Content;
using MLWD.Umbraco.Umbraco.ModelBuilder.ComponentModel.TypeConverters;

namespace MLWD.Umbraco.Mvc.Model.Archetypes
{
    public class UrlPicker : ArchetypeFieldsetModel
    {
        public string Title { get; set; }

        public string ExternalUrl { get; set; }

        [TypeConverter(typeof(ContentPickerConverter<Page>))]
        public Page InternalUrl { get; set; }

        private string _displayUrl { get; set; }

        public string DisplayUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_displayUrl))
                {
                    _displayUrl = InternalUrl != null ? InternalUrl.Url : ExternalUrl;
                }

                return (string.IsNullOrWhiteSpace(_displayUrl) ? "javascript:void(0);" : _displayUrl).ToLower();
            }
        }

        public bool NewWindow { get; set; }
    }
}
