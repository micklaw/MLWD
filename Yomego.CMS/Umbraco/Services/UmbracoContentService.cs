using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;
using Vega.USiteBuilder;
using Vega.USiteBuilder.DocumentTypeBuilder;
using Yomego.CMS.Core.Attributes;
using Yomego.CMS.Core.Enums;
using Yomego.CMS.Core.Mvc.Models;
using Yomego.CMS.Core.Mvc.Models.Interfaces;
using Yomego.CMS.Core.Umbraco.Model;
using umbraco;
using umbraco.MacroEngines;
using umbraco.cms.businesslogic.web;
using Umbraco.Core;
using Umbraco.Core.Models;
using umbraco.interfaces;
using umbraco.NodeFactory;
using Umbraco.Web;
using Content = Yomego.CMS.Core.Umbraco.Model.Content;

namespace Yomego.CMS.Umbraco.Services
{
    public delegate object BindPropertyDelegate(string data);

    public class UmbracoContentService : ContentService
    {
        public static IDictionary<Type, string> ListDelimiters = new Dictionary<Type, string>();

        private static readonly IDictionary<Type, BinderInfo> _modelBinders = new Dictionary<Type, BinderInfo>();

        private const string _defaultListDelimiter = ",";

        static UmbracoContentService()
        {
            ListDelimiters.Add(typeof(string), Environment.NewLine);
        }

        public UmbracoContentService() { }

        public override Content Get(string url)
        {
            var nodeId = uQuery.GetNodeIdByUrl(url);

            Content content = null;

            if (nodeId > 0)
            {
                content = Get(nodeId);
            }

            return content;
        }

        public override Content Get(int id)
        {
            return Get<Content>(id);
        }

        public override T Get<T>(int id)
        {
            return BuildModel<T>(new DynamicNode(id));
        }

        public override T GetRoot<T>()
        {
            var current = uQuery.GetCurrentNode();

            if (current == null)
                return null;

            var node = current.GetAncestorOrSelfNodes()
                        .Where(n => n.Level == 1 && n.NodeTypeAlias == "Homepage")
                        .FirstOrDefault();

            return BuildModel<T>(node);

        }

        public override string GetCurrentCulture()
        {
            Node node = null;

            try
            {
                node = Node.GetCurrent();
            }
            catch (NullReferenceException)
            {
                
            }

            string culture = null;

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

                if (children.Any())
                {
                    foreach (var child in children)
                    {
                        ListChildNodes(child, list);
                    }
                }
            }
        }

        public override IList<ISitemapItem> GetSitemapPages()
        {
            var pages = new List<ISitemapItem>();

            var helper = new UmbracoHelper(UmbracoContext.Current);

            var rootNodes = helper.TypedContentAtRoot();

            if (rootNodes.Any())
            {
                foreach (var rootNode in rootNodes)
                {
                    ListChildNodes(rootNode, pages);
                }
            }

            return pages;
        }

        #region Save Implementation

        public override int Save(Content content, bool publish)
        {
            // [ML] - I removed the publish children also as this seemed excessive

            return SaveContent(content, publish);
        }

        private int SaveContent(Content content, bool publish)
        {
            int parentId = 0;
            if (content.Id == 0 && content.ParentId == 0)
            {
                //throw new ArgumentException("Parent property cannot be null");

                parentId = -1;
            }
            else if (content.ParentId != 0)
            {
                parentId = content.ParentId;
            }

            if (String.IsNullOrWhiteSpace(content.Name))
                content.Name = content.GetType().Name;

            var admin = Util.GetAdminUser();

            DocumentType docType = DocumentTypeManager.GetDocumentType(content.GetType());

            IContent doc = null;

            if (content.Id == 0) // content item is new so create Document
            {
                doc = ApplicationContext.Current.Services.ContentService.CreateContent(content.Name, parentId, docType.Alias);
            }
            else // content item already exists, so load it
            {
                doc = ApplicationContext.Current.Services.ContentService.GetById(content.Id);
            }

            content.Id = doc.Id;

            var entityType = content.GetType();

            foreach (var p in doc.Properties)
            {
                var propertyInfo = entityType.GetProperty(p.Alias, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                
                if (propertyInfo != null)
                {
                    if (propertyInfo.PropertyType.UnderlyingSystemType != typeof(string) && IsIEnumerable(propertyInfo.PropertyType.UnderlyingSystemType))
                    {
                        string delimiter = ",";

                        // first get the array
                        var array = (IEnumerable)propertyInfo.GetValue(content, null);

                        // then find the length

                        if (array != null)
                        {
                            string value = String.Empty;
                            foreach (var item in array)
                            {
                                if (item != null)
                                {
                                    if (item.GetType() == typeof(string))
                                        delimiter = Environment.NewLine;

                                    value += item.ToString() + delimiter;
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
                            p.Value = String.Empty;
                        }
                    }
                    else if (propertyInfo.PropertyType == typeof (Image))
                    {
                        var s = propertyInfo.GetValue(content, null) as Image;

                        if (s != null)
                            p.Value = s.Id.ToString();
                    }
                    else if (propertyInfo.PropertyType == typeof(bool))
                    {
                        var s = propertyInfo.GetValue(content, null) as bool?;

                        if (s != null)
                            p.Value = s.Value ? "1" : "0";
                    }
                    else
                    {
                        var s = propertyInfo.GetValue(content, null);

                        if (s != null)
                            p.Value = s.ToString();
                        else
                            p.Value = null;
                    }

                }
            }

            var result = ApplicationContext.Current.Services.ContentService.SaveAndPublishWithStatus(doc);

            if (result.Success && publish)
            {
                ApplicationContext.Current.Services.ContentService.Publish(doc);
            }

            return result.Result.ContentItem != null ? result.Result.ContentItem.Id : 0;
        }

        #endregion

        #region Bind Implementation

        public static void Register<TService, TEntity>(Expression<Func<TService, BindPropertyDelegate>> builder)
        {
            var unaryExpr = (UnaryExpression)builder.Body;
            var methodCallExpr = (MethodCallExpression)unaryExpr.Operand;

            ConstantExpression constantExpr;

            if (methodCallExpr.Object != null)
            {
                // It appears .Net 4.5 sets Object
                constantExpr = (ConstantExpression)methodCallExpr.Object;
            }
            else
            {
                // It appears .Net 4.0 adds it as the last arg
                constantExpr = (ConstantExpression)methodCallExpr.Arguments.Last();
            }

            var methodInfo = (MethodInfo)constantExpr.Value;

            _modelBinders[typeof(TEntity)] = new BinderInfo(typeof(TService), methodInfo);
        }

        private static bool IsGenericList(Type entityType)
        {
            bool isGenericList = entityType.IsGenericType && (entityType.GetGenericTypeDefinition() == typeof(IList<>) || entityType.GetGenericTypeDefinition() == typeof(List<>));

            return isGenericList;
        }

        private BindPropertyDelegate GetModelBuilder(Type type)
        {
            BindPropertyDelegate builder = null;

            if (_modelBinders.ContainsKey(type))
            {
                var service = System.Web.Mvc.DependencyResolver.Current.GetService(_modelBinders[type].ServiceType);

                builder = Delegate.CreateDelegate(typeof(BindPropertyDelegate), service, _modelBinders[type].Method) as BindPropertyDelegate;
            }

            return builder;
        }

        internal class BinderInfo
        {
            public BinderInfo(Type serviceType, MethodInfo method)
            {
                ServiceType = serviceType;
                Method = method;
            }

            public Type ServiceType { get; set; }

            public MethodInfo Method { get; set; }
        }

        private string GetListDelimiter(Type listItemType)
        {
            if (ListDelimiters.ContainsKey(listItemType))
                return ListDelimiters[listItemType];

            return _defaultListDelimiter;
        }

        private void SetValue(Type entityType, object entity, string nodePropertyAlias, string nodePropertyValue)
        {
            var entityProperty = entityType.GetProperty(nodePropertyAlias, BindingFlags.IgnoreCase |
                                                                           BindingFlags.Public |
                                                                           BindingFlags.Instance);
            if (entityProperty != null)
            {
                var isArchetype = Attribute.IsDefined(entityProperty, typeof (ArchetypeAttribute));

                var entityPropertyType = entityProperty.PropertyType;

                object value = null;

                var binder = GetModelBuilder(entityPropertyType);

                if (binder != null)
                {
                    value = binder(nodePropertyValue);
                }
                else if (isArchetype)
                {
                    value = BuildArchetypeModel<ArchetypeContent>(nodePropertyValue, entityPropertyType);
                }
                else if (IsIEnumerable(entityPropertyType) && !IsType<string>(entityPropertyType))
                {
                    // Get the type of the list
                    var args = entityPropertyType.GetGenericArguments();
                    if (args.Length == 1)
                    {
                        // NOTE: we only support Generic types with one argument type. i.e. MyGeneric<Type1> and not MyGeneric<Type1, Type2>
                        var itemType = args[0];

                        // Get the delimter
                        var delimiter = GetListDelimiter(itemType);

                        var splitItems = nodePropertyValue.Split(new string[1] {delimiter},
                                                                 StringSplitOptions.RemoveEmptyEntries);

                        // Create an instance of the strongly typed list
                        var constructedListType = typeof (List<>).MakeGenericType(itemType);
                        var list = (IList) Activator.CreateInstance(constructedListType);

                        // Get the binder for the specified type
                        binder = GetModelBuilder(itemType);

                        // Populate the list
                        foreach (var splitItem in splitItems)
                        {
                            int id;
                            object item;
                            if (binder == null && int.TryParse(splitItem, out id))
                            {
                                item = BuildModel<Content>(new DynamicNode(id));
                            }
                            else
                            {
                                item = binder(splitItem);
                            }

                            if (item != null)
                            {
                                list.Add(item);
                            }
                        }

                        value = list;
                    }
                }
                else if (typeof (Content).IsAssignableFrom(entityPropertyType))
                {
                    int id;
                    if (int.TryParse(nodePropertyValue, out id))
                    {
                        value = BuildModel<Content>(new DynamicNode(id));
                    }
                }
                

                // We're done, set the property
                entityProperty.SetValue(entity, value, null);
            }
        }


        private object BuildArchetypeModel<T>(string json, Type entityType) where T : ArchetypeContent
        {
            // ML - Get the generic type from the (expected) IList<T> in the model

            var isGenericList = IsGenericList(entityType);

            Type archetypeListType = null;

            if (isGenericList)
            {
                archetypeListType = entityType.GetGenericArguments().FirstOrDefault();
            }
            else
            {
                archetypeListType = entityType;
            }

            if (archetypeListType == null)
            {
                throw new NullReferenceException(string.Format("The type ({0}) can not be inferred?", entityType.Name));
            }

            // ML - Build a generic list from the type fuond above

            var constructedListType = typeof(List<>).MakeGenericType(archetypeListType);
            var list = (IList)Activator.CreateInstance(constructedListType);

            if (!string.IsNullOrWhiteSpace(json))
            {
                // ML - Archetype all the things

                var entity = JsonConvert.DeserializeObject<BaseArchetype>(json);

                if (entity.fieldsets != null && entity.fieldsets.Any())
                {
                    foreach (var fieldset in entity.fieldsets)
                    {
                        if (fieldset.properties != null && fieldset.properties.Any())
                        {
                            var instanceType = DocumentTypeManager.GetArchetype(fieldset.alias);

                            if (instanceType != null)
                            {
                                // ML - Create an instance for each archetype object

                                var instance = Activator.CreateInstance(instanceType) as T;

                                if (instance != null)
                                {
                                    instance.ArchetypeAlias = fieldset.alias;

                                    foreach (var property in fieldset.properties)
                                    {
                                        SetValue(instanceType, instance, property.alias, property.value);
                                    }

                                    // [ML] - If this is not a generic type, then return the first item

                                    if (!isGenericList)
                                    {
                                        return instance;
                                    }

                                    list.Add(instance);
                                }
                            }
                        }
                    }
                }
            }


            return isGenericList ? list : null;
        }

        public T BuildModel<T>(INode node) where T : Content
        {
            // Create instance of the entity

            if (node.Id == 0)
            {
                return default(T); // [ML] - The below seems severe as nodes may be deleted etc by admins, switch comments if required

                // throw new Exception(String.Format("Node Id cannot be zero when building a node"));
            }

            Type entityType;

            try
            {
                entityType = DocumentTypeManager.GetDocumentTypeType(node.NodeTypeAlias);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Cannot find DocType for: {0}", node.NodeTypeAlias), ex);
            }

            var entity = Activator.CreateInstance(entityType) as T;

            if (entity == null)
                throw new ArgumentException(String.Format("{0} cannot be cast to {1}", entityType.Name, typeof(T).Name));

            // The node properties to loop over
            var nodeProperties = node.PropertiesAsList;

            // Set the base properties of Content
            entity.Id = node.Id;
            entity.Name = node.Name;
            entity.Url = node.NiceUrl;
            entity.Path = node.Path;
            entity.ContentTypeAlias = node.NodeTypeAlias;
            entity.ParentId = node.Parent != null ? node.Parent.Id : 0;
            entity.SortOrder = node.SortOrder;

            if (entity.ParentId > 0)
            {
                entity.ParentUrl = node.Parent != null ? node.Parent.NiceUrl : null;
                entity.ParentName = node.Parent != null ? node.Parent.Name : null;
            }

            entity.DateCreated = node.CreateDate;
            entity.DateUpdated = node.UpdateDate;

            // Loop over all the properties in the Umbraco node
            foreach (var nodeProperty in nodeProperties)
            {
                SetValue(entityType, entity, nodeProperty.Alias, nodeProperty.Value);
            }

            // Check for entity properties that are child nodes
            var childProperties = from p in entityType.GetProperties()
                                  let attr = p.GetCustomAttributes(typeof(ChildContentAttribute), true)
                                  where attr.Length == 1
                                  select new { Property = p, Attribute = attr.First() as ChildContentAttribute };

            foreach (var property in childProperties)
            {
                if (IsIEnumerable(property.Property.PropertyType) && !IsType<string>(property.Property.PropertyType))
                {
                    // Get the type of the list
                    var args = property.Property.PropertyType.GetGenericArguments();
                    if (args != null && args.Length == 1)
                    {
                        // NOTE: we only support Generic types with one argument type. i.e. MyGeneric<Type1> and not MyGeneric<Type1, Type2>
                        var itemType = args[0];

                        // Create an instance of the strongly typed list
                        var constructedListType = typeof(List<>).MakeGenericType(itemType);
                        var list = (IList)Activator.CreateInstance(constructedListType);

                        var attribute = property.Property.GetCustomAttribute(typeof(ChildContentAttribute)) as ChildContentAttribute;

                        IEnumerable<INode> childNodes = new List<INode>();

                        if (attribute != null && attribute.SearchType != null)
                        {
                            childNodes = node.GetDescendantNodesByType(attribute.SearchType.Name);
                        }
                        else
                        {
                            childNodes = node.GetChildNodes();
                        }

                        foreach (var child in childNodes)
                        {
                            var item = BuildModel<Content>(new DynamicNode(child.Id));
                            list.Add(item);
                        }

                        property.Property.SetValue(entity, list, null);
                    }
                }
            }

            // Check for parent properties
            var parentProperties = from p in entityType.GetProperties()
                                   let attr = p.GetCustomAttributes(typeof(ParentContentAttribute), true)
                                   where attr.Length == 1
                                   select new { Property = p, Attribute = attr.First() as ParentContentAttribute };

            foreach (var property in parentProperties)
            {
                var item = BuildModel<Content>(new DynamicNode(node.Parent.Id));

                property.Property.SetValue(entity, item);
            }

            OnAfterModelBound(entity);

            return entity;
        }

        #endregion
    }
}