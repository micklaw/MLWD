using System;
using System.ComponentModel;
using System.Globalization;
using Our.Umbraco.Ditto;
using Umbraco.Core.Models;
using Yomego.Umbraco.Umbraco.Helpers;

namespace Yomego.Umbraco.Umbraco.Ditto.Resolvers.TemplateName
{
    public class ViewNameValueResolver : DittoValueResolver
    {
        public override object ResolveValue()
        {
            var content = Context.Instance as IPublishedContent;
            var descriptor = Context.PropertyDescriptor;

            if (content != null && descriptor != null)
            {
                // [ML] - If there is no template selected then grab the ViewName from the route attribute or just Index

                return ConverterHelper.GetTemplateName(content.TemplateId, content.DocumentTypeAlias);
            }

            throw new NullReferenceException($"No template can be found for this content. Should be very hard to get here.");
        }
    }
}
