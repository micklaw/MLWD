using System.Globalization;
using System.Linq;
using Our.Umbraco.Ditto;
using umbraco;
using umbraco.cms.businesslogic.web;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using umbraco.NodeFactory;
using Umbraco.Web;
using Yomego.Umbraco.Umbraco.Helpers;

namespace Yomego.Umbraco.Umbraco.Services.Content
{
    public delegate object BindPropertyDelegate(string data);

    internal class UmbracoContentService : ContentService
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

        public override object Get(string url)
        {
            var nodeId = uQuery.GetNodeIdByUrl(url);

            object content = null;

            if (nodeId > 0)
            {
                content = Get(nodeId);
            }

            return content;
        }

        public override object Get(IPublishedContent model)
        {
            if (model != null)
            {
                var type = ConverterHelper.FirstFromBaseType<PublishedContentModel>(model.DocumentTypeAlias);

                if (type != null)
                {
                    return model.As(type) as PublishedContentModel;
                }
            }

            return model;
        }

        public override object Get(int id)
        {
            var content = Umbraco.TypedContent(id);

            if (content != null)
            {
                var type = ConverterHelper.FirstFromBaseType<PublishedContentModel>(content.DocumentTypeAlias);

                if (type != null)
                {
                    return content.As(type) as Model.Content;
                }
            }

            return content as Model.Content;
        }

        public override T Get<T>(int id)
        {
            var content = Umbraco.TypedContent(id);
            
            return content.As<T>();
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
    }
}