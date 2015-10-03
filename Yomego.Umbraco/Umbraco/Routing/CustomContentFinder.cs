using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Routing;
using Yomego.Umbraco.Constants;
using Yomego.Umbraco.Context;
using Yomego.Umbraco.Umbraco.Helpers;
using Yomego.Umbraco.Umbraco.Services.Container;
using Content = Yomego.Umbraco.Umbraco.Model.Content;

namespace Yomego.Umbraco.Umbraco.Routing
{
    public class CustomContentFinder : ContentFinderByNiceUrl, IContentFinder
    {
        public new bool TryFindContent(PublishedContentRequest contentRequest)
        {
            var httpContext = HttpContext.Current;

            // [ML] - If we have an HttpContent and its not previously routed

            if (httpContext != null)
            {
                var path = contentRequest.Uri.GetAbsolutePathDecoded();

                if (!string.IsNullOrWhiteSpace(path))
                {
                    contentRequest.PublishedContent = FindContent(contentRequest, path);

                    if (contentRequest.PublishedContent == null)
                    {
                        // [ML] - This might be a Preview, so find the node with the preview url

                        int nodeId;
                        if (int.TryParse(path.Replace("/", string.Empty), out nodeId))
                        {
                            contentRequest.PublishedContent = ConverterHelper.UmbracoHelper.TypedContent(nodeId);
                        }
                    }

                    // [ML] - If we have a pubished content request, then hook in routing to our controller with our populated POCO with Ditto

                    if (contentRequest.PublishedContent != null)
                    {
                        var app = new CoreApp<CoreServiceContainer>();

                        var poco = app.Services.Content.Get(contentRequest.PublishedContent) as Content;

                        if (poco != null)
                        {
                            httpContext.Items[Requests.Node] = poco;

                            ITemplate template;

                            if (poco.TemplateId > 0)
                            {
                                // [ML] - If the poco has an Umbraco template then use it, this could use an optional Action or just be used like umbraco

                                template = ApplicationContext.Current.Services.FileService.GetTemplate(poco.TemplateId);
                            }
                            else
                            {
                                // [ML] - Or fallback to the Default RoutToMvc Template and use our Mvc jazz

                                template = ApplicationContext.Current.Services.FileService.GetTemplate("Default");
                            }

                            if (template == null)
                            {
                                throw new InvalidOperationException("There doesn't appear to be a Default template with an alias of 'Default'");
                            }

                            contentRequest.SetTemplate(template);

                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
