using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;

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
    }
}
