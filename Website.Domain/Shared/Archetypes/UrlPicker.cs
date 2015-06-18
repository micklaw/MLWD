using System.ComponentModel;
using Archetype.Models;
using Our.Umbraco.Ditto;
using Website.Domain.Shared.DocTypes;

namespace Website.Domain.Shared.Archetypes
{
    public class UrlPicker : ArchetypeFieldsetModel
    {
        public string Title { get; set; }

        public string ExternalUrl { get; set; }

        [TypeConverter(typeof(DittoContentPickerConverter))]
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