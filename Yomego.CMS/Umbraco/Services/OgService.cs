using Yomego.CMS.Core.Umbraco.Model;

namespace Yomego.CMS.Umbraco.Services
{
    public class OgService : Service<App>
    {
        public OgModel OgTagFromContent(Content content)
        {
            OgModel model = null;

            // ML - Check this is a content page

            var contentPage = content as Page;

            if (contentPage != null)
            {
                if (contentPage.OgUseThis)
                {
                    model = new OgModel("article");

                    var page = content as Page;

                    model.Title = page.OgTitle;
                    model.Description = page.OgDescription;

                    if (page.OgImage != null && page.OgImage.HasUrl)
                    {
                        model.ImageUrl = App.Context.DomainUrl + page.OgImage.Url;
                    }

                    model.ShareUrl = App.Context.DomainUrl + string.Format("/og?id={0}", page.Id);
                    model.RedirectUrl = App.Context.DomainUrl + page.Url;
                }
            }

            return model;
        }
    }
}
