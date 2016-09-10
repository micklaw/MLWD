using System;
using System.Configuration;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web.Routing;
using Yomego.Umbraco.Constants;
using Yomego.Umbraco.Context;
using Yomego.Umbraco.Umbraco.Helpers;
using Yomego.Umbraco.Umbraco.Services.Container;
using Content = Yomego.Umbraco.Umbraco.Model.Content;

namespace Yomego.Umbraco.Umbraco.Routing
{
    /// <summary>
    /// Override <see cref="ContentFinderByNiceUrl"/> to get our Ditto model and setup our .Net Mvc route template
    /// </summary>
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
                        var app = new App();

                        // [ML] - Get our content node using our lucene based Ditto cache

                        var poco = app.Services.Content.Get(contentRequest.PublishedContent) as Content;

                        if (poco != null)
                        {
                            var templateName = ConfigurationManager.AppSettings["route:templateName"] ?? "Default";

                            // [ML] - Store POCO in the request items

                            httpContext.Items[Requests.Node] = poco;

                            ITemplate template;

                            if (poco.TemplateId > 0)
                            {
                                // [ML] - If the poco has an Umbraco template selected then use it as per usual Umbraco implementation

                                template = ApplicationContext.Current.Services.FileService.GetTemplate(poco.TemplateId);
                            }
                            else
                            {
                                // [ML] - Or fallback to the Default RoutToMvc Template and use standard Mvc no render controllers required

                                template = ApplicationContext.Current.Services.FileService.GetTemplate(templateName);
                            }

                            if (template == null)
                            {
                                return false;
                            }

                            // [ML] - Set the template to use in rendering the page

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
