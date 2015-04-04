using System.Collections.Generic;
using System.ComponentModel;
using MLWD.Umbraco.Mvc.Model.Archetypes;
using MLWD.Umbraco.Mvc.Model.Media;
using MLWD.Umbraco.Umbraco.ModelBuilder.ComponentModel.TypeConverters;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;

namespace MLWD.Umbraco.Mvc.Model.Content
{
    public class Settings : PublishedContentModel
    {
        public Settings(IPublishedContent content) : base(content) { }

        public string FacebookAppId { get; set; }

        public string FacebookSecret { get; set; }

        public string GoogleAnalytics { get; set; }

        public bool Hide { get; set; }

        public string Robots { get; set; }

        public int ApiPageCount { get; set; }

        public string CompanyName { get; set; }

        [TypeConverter(typeof(ImageConverter))]
        public Image CompanyLogo { get; set; }

        public string CompanyEmail { get; set; }

        public string CompanyTelephone { get; set; }

        public string CompanyFacebook { get; set; }

        public string CompanyTwitter { get; set; }

        public string CompanyLinkedIn { get; set; }

        public string CompanySkype { get; set; }

        [TypeConverter(typeof(ArchetypeConverter<List<UrlPicker>>))]
        public List<UrlPicker> NavItems { get; set; }
    }
}
