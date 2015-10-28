using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Xml;
using umbraco.cms.businesslogic.web;
using umbraco.NodeFactory;
using Umbraco.Core;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.Web.Models.ContentEditing;
using Yomego.Umbraco.Mvc.Attributes;
#pragma warning disable 618

namespace Yomego.Umbraco.Umbraco.Helpers
{
    /// <summary>
    /// Provides helper methods to aid with conversion. Not much in here for now but who knows 
    /// what the future has in store?
    /// </summary>
    public static class ConverterHelper
    {
        private static ConcurrentDictionary<string, Type> _baseTypes = new ConcurrentDictionary<string, Type>();

        private static ConcurrentDictionary<string, string> _documentTypeDefaultActions = new ConcurrentDictionary<string, string>();

        private static ConcurrentDictionary<int, string> _templateNames = new ConcurrentDictionary<int, string>();

        /// <summary>
        /// Gets the <see cref="UmbracoHelper"/> for querying published content or media.
        /// </summary>
        public static UmbracoHelper UmbracoHelper
        {
            get
            {
                // Pull the item from the cache if possible to reduce the db access overhead caused by 
                // multiple reflection iterations for the given type taking place in a single request.
                return (UmbracoHelper)ApplicationContext.Current.ApplicationCache.RequestCache.GetCacheItem(
                        "Ditto.UmbracoHelper",
                        () => new UmbracoHelper(UmbracoContext.Current));
            }
        }

        /// <summary>
        /// Gets Ids from known XML fragments (as saved by the MNTP / XPath CheckBoxList)
        /// </summary>
        /// <param name="xml">The Xml</param>
        /// <returns>An array of node ids as integer.</returns>
        public static int[] GetXmlIds(string xml)
        {
            var ids = new List<int>();

            if (!string.IsNullOrEmpty(xml))
            {
                using (var xmlReader = XmlReader.Create(new StringReader(xml)))
                {
                    try
                    {
                        xmlReader.Read();

                        // Check name of first element
                        switch (xmlReader.Name)
                        {
                            case "MultiNodePicker":
                            case "XPathCheckBoxList":
                            case "CheckBoxTree":

                                // Position on first <nodeId>
                                xmlReader.ReadStartElement();

                                while (!xmlReader.EOF)
                                {
                                    if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "nodeId")
                                    {
                                        int id;
                                        if (int.TryParse(xmlReader.ReadElementContentAsString(), out id))
                                        {
                                            ids.Add(id);
                                        }
                                    }
                                    else
                                    {
                                        // Step the reader on
                                        xmlReader.Read();
                                    }
                                }

                                break;
                        }
                    }
                    catch
                    {
                        // Failed to read as Xml
                    }
                }
            }

            return ids.ToArray();
        }

        public static CultureInfo GetCurrentCulture()
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

            string culture = null;

            if (node != null && node.Id > 0)
            {
                culture = GetCulture(node.Id);
            }
            
            if (string.IsNullOrWhiteSpace(culture))
            {
                return CultureInfo.CurrentCulture;
            }

            return new CultureInfo(culture);
        }

        private static string GetCulture(int id)
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

        private static string GetAlias(Type type)
        {
            var attribute = type.GetCustomAttribute<UmbracoRouteAttribute>(false);

            if (attribute != null)
            {
                if (!string.IsNullOrWhiteSpace(attribute.Alias))
                {
                    return attribute.Alias;
                }
            }

            return type.Name;
        }

        public static Type FirstFromBaseType<T>(string name)
        {
            Type type = null;

            PopulateBaseTypes<T>();

            if (!string.IsNullOrWhiteSpace(name))
            {
                _baseTypes.TryGetValue(name, out type);
            }

            return type;
        }

        private static void PopulateBaseTypes<T>()
        {
            if (_baseTypes == null || _baseTypes.Count == 0)
            {
                if (_baseTypes == null)
                {
                    _baseTypes = new ConcurrentDictionary<string, Type>();
                }

                var types = PluginManager.Current.ResolveTypes<T>();

                if (types != null)
                {
                    foreach (var type in types)
                    {
                        _baseTypes.TryAdd(GetAlias(type), type);
                    }
                }
            }
        }

        /// <summary>
        /// Get the view name to render wth from the template if picked or from the document type
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="docTypeAlias"></param>
        /// <returns></returns>
        public static string GetTemplateName(int templateId, string docTypeAlias)
        {
            string action = null;

            if (templateId == 0)
            {
                // [ML] - Try and get it from the template 0, documentAlias cache

                if (!_documentTypeDefaultActions.TryGetValue(docTypeAlias, out action))
                {
                    // [ML] - If we cant find it get it with the route attribute action and update the template 0, document type alias cache

                    var type = FirstFromBaseType<PublishedContentModel>(docTypeAlias);

                    var routeAttribute = type?.GetCustomAttribute<UmbracoRouteAttribute>(false);

                    if (!string.IsNullOrWhiteSpace(routeAttribute?.Action))
                    {
                        action = routeAttribute.Action;
                    }

                    if (string.IsNullOrWhiteSpace(action))
                    {
                        action = "Index";
                    }

                    _documentTypeDefaultActions.TryAdd(docTypeAlias, action);
                }

                return action;
            }

            // [ML] - If not found, grab the template name and update the cache template name cache

            var defaultViewName = ConfigurationManager.AppSettings["route:templateName"] ?? "Default";

            action = ApplicationContext.Current.Services.FileService.GetTemplate(templateId)?.Alias;

            if (string.IsNullOrWhiteSpace(action) || action.ToLower().Equals(defaultViewName.ToLower()))
            {
                action = "Index";
            }

            return action;
        }
    }
}