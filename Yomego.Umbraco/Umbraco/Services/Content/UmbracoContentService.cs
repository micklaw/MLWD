using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Our.Umbraco.Ditto;
using Our.Umbraco.Ditto.Resolvers.Archetype.Attributes;
using umbraco;
using umbraco.cms.businesslogic.web;
using umbraco.MacroEngines;
using umbraco.NodeFactory;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Yomego.Umbraco.Umbraco.Helpers;
#pragma warning disable 618

namespace Yomego.Umbraco.Umbraco.Services.Content
{
    public delegate object BindPropertyDelegate(string data);

    internal class UmbracoContentService : ContentService
    {
        private static ConcurrentDictionary<int, PublishedContentModel> _dittoCache = new ConcurrentDictionary<int, PublishedContentModel>();

        private PublishedContentModel GetFromCache(int nodeId)
        {
            PublishedContentModel content;
            _dittoCache.TryGetValue(nodeId, out content);

            return content;
        }

        private object AddToCache(int nodeId, object content)
        {
            var node = content as PublishedContentModel;

            if (nodeId > 0 && (node != null && node.Id > 0))
            {
                _dittoCache.TryAdd(nodeId, node);
            }

            return node;
        }

        public override void ClearCache()
        {
            _dittoCache = new ConcurrentDictionary<int, PublishedContentModel>();
        }

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

            if (nodeId > 0)
            {
                object content = GetFromCache(nodeId);

                if (content == null)
                {
                    return AddToCache(nodeId, Get(nodeId));
                }
            }

            return null;
        }

        public override object Get(IPublishedContent model)
        {
            if (model != null && model.Id > 0)
            {
                var content = GetFromCache(model.Id);

                if (content == null)
                {
                    var type = ConverterHelper.FirstFromBaseType<PublishedContentModel>(model.DocumentTypeAlias);

                    if (type != null)
                    {
                        using (ApplicationContext.Current.ProfilingLogger.DebugDuration<object>($"Getting node from IPublishedContent '{model.Id}' of type '{type.Name}"))
                        {
                            return AddToCache(model.Id, model.As(type));
                        }
                    }
                }

                return content;
            }

            return null;
        }

        public override object Get(int id)
        {
            var content = GetFromCache(id);

            if (content == null)
            {
                var node = Umbraco.TypedContent(id);

                if (node != null)
                {
                    var type = ConverterHelper.FirstFromBaseType<PublishedContentModel>(node.DocumentTypeAlias);

                    if (type != null)
                    {
                        return AddToCache(id, node.As(type));
                    }
                }

                return null;
            }

            return content;
        }

        public override T Get<T>(int id)
        {
            var content = GetFromCache(id);

            if (content == null)
            {
                var node = Umbraco.TypedContent(id);

                if (node != null)
                {
                    return (T)AddToCache(id, node.As<T>());
                }

                return null;
            }

            return (T)content;
        }

        private DynamicNode _rootNode { get; set; }

        public override DynamicNode RootNode
        {
            get
            {
                if (_rootNode == null)
                {
                    _rootNode = new DynamicNode(-1);
                }

                return _rootNode;
            }
        }

        public override T GetRoot<T>()
        {
            var current = uQuery.GetCurrentNode();

            var node = current?.GetAncestorOrSelfNodes().FirstOrDefault(n => n.Level == 1);

            if (node != null)
            {
                return Get<T>(node.Id);
            }

            return null;
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

        public override void Save(PublishedContentModel content, bool publish = true, int? parentId = null)
        {
            SaveContent(content, publish, parentId);
        }

        #region Save Implementation

        private void SaveContent(PublishedContentModel content, bool publish, int? parentId = null)
        {
            // [ML] - Use the parentId passed or either fallback to the content.Parent.Id of node or default to -1

            parentId = parentId ?? ((content.Parent != null && content.Parent.Id != 0) ? content.Parent.Id : -1);

            IContent doc = null;

            if (content.Id == 0) // content item is new so create Document
            {
                doc = ApplicationContext.Current.Services.ContentService.CreateContent(content.Name, parentId.Value, content.DocumentTypeAlias);
            }
            else // content item already exists, so load it
            {
                doc = ApplicationContext.Current.Services.ContentService.GetById(content.Id);
            }

            var entityType = content.GetType();

            foreach (var p in doc.Properties)
            {
                var propertyInfo = entityType.GetProperty(p.Alias, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (propertyInfo != null)
                {
                    var isArchetype = propertyInfo.GetCustomAttribute<ArchetypeValueResolverAttribute>() != null;

                    if (!isArchetype)
                    {
                        if (propertyInfo.PropertyType.UnderlyingSystemType != typeof (string) &&
                            IsIEnumerable(propertyInfo.PropertyType.UnderlyingSystemType))
                        {
                            string delimiter = ",";

                            // first get the array
                            var array = (IEnumerable) propertyInfo.GetValue(content, null);

                            // then find the length

                            if (array != null)
                            {
                                var value = string.Empty;

                                foreach (var item in array)
                                {
                                    if (item != null)
                                    {
                                        if (item is string)
                                        {
                                            delimiter = Environment.NewLine;
                                        }

                                        var itemAsContent = item as PublishedContentModel;

                                        if (itemAsContent != null)
                                        {
                                            value += itemAsContent.Id.ToString() + delimiter;
                                        }
                                        else
                                        {
                                            value += item.ToString() + delimiter;
                                        }
                                    }
                                }

                                if (value.EndsWith(delimiter))
                                {
                                    value = delimiter != Environment.NewLine
                                        ? value.Substring(0, value.Length - 1)
                                        : value.Substring(0, value.Length - 2);
                                }

                                p.Value = value;
                            }
                            else
                            {
                                p.Value = string.Empty;
                            }
                        }
                        else
                        {
                            var s = propertyInfo.GetValue(content, null);

                            // ML - For some mental reason bools are saved as 1 or 0 strings

                            if (s is bool)
                            {
                                p.Value = (bool) s ? "1" : "0";
                            }
                            else
                            {
                                p.Value = s?.ToString();
                            }
                        }
                    }
                    else
                    {
                        // TODO: [ML] - Find a way to serialise this object to an Archetype fomat for the DB
                    }
                }
            }

            ApplicationContext.Current.Services.ContentService.SaveAndPublish(doc);

            if (publish)
            {
                ApplicationContext.Current.Services.ContentService.Publish(doc);
            }
        }

        #endregion
    }
}