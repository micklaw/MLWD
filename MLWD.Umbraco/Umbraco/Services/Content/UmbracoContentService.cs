using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MLWD.Umbraco.Mvc.Model;
using MLWD.Umbraco.Mvc.Model.Enums;
using MLWD.Umbraco.Mvc.Model.Interfaces;
using MLWD.Umbraco.Umbraco.ModelBuilder.ComponentModel;
using MLWD.Umbraco.Umbraco.ModelBuilder.Extensions;
using Umbraco.Core.Models.PublishedContent;
using umbraco;
using umbraco.cms.businesslogic.web;
using Umbraco.Core.Models;
using umbraco.NodeFactory;
using Umbraco.Web;

namespace MLWD.Umbraco.Umbraco.Services.Content
{
    public delegate object BindPropertyDelegate(string data);

    public class UmbracoContentService : ContentService
    {
        private UmbracoHelper _umbraco { get; set; }
        
        public UmbracoHelper Umbraco
        {
            get
            {
                if (_umbraco == null)
                {
                    _umbraco = ConverterHelper.UmbracoHelper;
                }

                return _umbraco;
            }
        }

        public override PublishedContentModel Get(string url)
        {
            var nodeId = uQuery.GetNodeIdByUrl(url);

            PublishedContentModel content = null;

            if (nodeId > 0)
            {
                content = Get(nodeId);
            }

            return content;
        }

        public override PublishedContentModel Get(int id)
        {
            var content = Umbraco.TypedContent(id);

            if (content != null)
            {
                var type = ConverterHelper.FirstFromBaseType<PublishedContentModel>(content.DocumentTypeAlias);

                if (type != null)
                {
                    return content.As(type) as PublishedContentModel;
                }
            }

            return content as PublishedContentModel;
        }

        public override T Get<T>(int id)
        {
            return Umbraco.TypedContent(id).As<T>();
        }

        public override T GetRoot<T>()
        {
            var current = uQuery.GetCurrentNode();

            if (current == null)
                return null;

            var node = current.GetAncestorOrSelfNodes().FirstOrDefault(n => n.Level == 1);

            if (node != null)
            {
                return Umbraco.TypedContent(node.Id).As<T>();
            }

            return default(T);
        }

        public override string GetCurrentCulture()
        {
            Node node = null;

            try
            {
                node = Node.GetCurrent();
            }
            catch
            {
                // [ML] - Suppress if no umbraco culture
            }

            string culture;

            if (node != null && node.Id > 0)
            {
                culture = GetCulture(node.Id);
            }
            else
            {
                return CultureInfo.CurrentCulture.Name;
            }

            return culture;
        }

        public override string GetCulture(int id)
        {
            // NOTE: Ripped this code out of the umbraco.dll as the GetCurrentDomains(int NodeId) method blows
            // up without an httpcontext

            string culture = null;

            var node = new Node(id);

            Domain[] domains = null;

            string[] strArrays = node.Path.Split(new[] { ',' });

            for (int i = strArrays.Length - 1; i > 0; i--)
            {
                domains = Domain.GetDomainsById(int.Parse(strArrays[i]));

                if (domains.Length > 0)
                    break;
            }

            if (domains != null && domains.Length > 0)
            {
                culture = domains[0].Language.CultureAlias;
            }

            return culture;
        }

        private DateTime GetEntryLastModified(IPublishedContent entry)
        {
            if (entry.UpdateDate > entry.CreateDate)
                return entry.UpdateDate;

            return entry.CreateDate;
        }

        private void ListChildNodes(IPublishedContent parent, IList<ISitemapItem> list)
        {
            if (parent != null && parent.Id > 0)
            {
                if (!parent.GetPropertyValue<bool>("umbracoNaviHide"))
                {
                    list.Add(new SitemapItem(parent.Url)
                    {
                        LastModified = GetEntryLastModified(parent),
                        Priority = 1,
                        ChangeFrequency = ChangeFrequency.Daily
                    });
                }

                var children = parent.Children;

                var publishedContents = children as IPublishedContent[] ?? children.ToArray();

                if (publishedContents.Any())
                {
                    foreach (var child in publishedContents)
                    {
                        ListChildNodes(child, list);
                    }
                }
            }
        }

        public override IList<ISitemapItem> GetSitemapPages()
        {
            var pages = new List<ISitemapItem>();

            var rootNodes = Umbraco.TypedContentAtRoot();

            var publishedContents = rootNodes as IPublishedContent[] ?? rootNodes.ToArray();

            if (publishedContents.Any())
            {
                foreach (var rootNode in publishedContents)
                {
                    ListChildNodes(rootNode, pages);
                }
            }

            return pages;
        }
    }
}