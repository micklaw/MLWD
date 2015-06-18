using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Archetype.Models;
using Our.Umbraco.Ditto;
using Our.Umbraco.Ditto.Resolvers.Archetype.Attributes;
using Our.Umbraco.Ditto.Resolvers.Archetype.Extensions;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Yomego.Umbraco;

namespace Website.Domain.Shared.Ditto.ValueResolvers.ChildContent
{
    public class ChildContentValueResolver : DittoValueResolver<ChildContentResolverAttribute>
    {
        public override object ResolveValue(ITypeDescriptorContext context, ChildContentResolverAttribute attribute, CultureInfo culture)
        {
            var content = context.Instance as IPublishedContent;
            var descriptor = context.PropertyDescriptor;

            if (content != null && descriptor != null && content.Children != null && content.Children.Any())
            {
                // [ML] - As we actually have stuff to do now, generate a list please if required

                var propertyType = descriptor.PropertyType;
                var isGenericType = propertyType.IsGenericType;
                var targetType = isGenericType
                    ? propertyType.GenericTypeArguments.First()
                    : propertyType;

                if (!isGenericType)
                {
                    return content.FirstChild().As(targetType);
                }

                // [ML] - Create a list

                var listType = typeof(List<>).MakeGenericType(targetType);
                var list = (IList)Activator.CreateInstance(listType);

                var app = new App();

                foreach (var child in content.Children.OfType<IPublishedContent>())
                {
                    var model = app.Services.Content.Get(child);

                    if (model != null)
                    {
                        list.Add(model);
                    }
                }

                return list;
            }

            return null;
        }
    }
}