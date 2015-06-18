using System.Collections.Generic;
using System.ComponentModel;
using Our.Umbraco.Ditto.Resolvers.Archetype.Attributes;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Website.Domain.Shared.Archetypes;
using Website.Domain.Shared.Ditto.TypeConverters;
using Yomego.Umbraco.Mvc.Model.Media;

namespace Website.Domain.Shared.DocTypes
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

        [ArchetypeResolver]
        public List<UrlPicker> NavItems { get; set; }
    }
}