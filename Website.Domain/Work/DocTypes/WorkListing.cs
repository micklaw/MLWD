using Yomego.CMS.Core.Attributes;
using Yomego.CMS.Core.Enums;
using Yomego.CMS.Core.Umbraco.Model;

namespace Website.Domain.Work.DocTypes
{
    [ContentType(Name = "Work Listing", IsContainer = true, Description = "Work listing for the site", Controller = "Work", IconUrl = "icon-newspaper", AllowedChildNodeTypes = new[]
        {
            typeof(WorkDetails)
        })]
    public class WorkListing : Page
    {
        [ContentTypeProperty(ContentTypePropertyUI.Numeric, Name = "Page Item Count", Tab = Tabs.Content, DefaultValue = 5)]
        public int WorkPageCount { get; set; }
    }
}
