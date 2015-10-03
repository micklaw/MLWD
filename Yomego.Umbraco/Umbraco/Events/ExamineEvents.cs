using System;
using Examine;
using Lucene.Net.Documents;
using umbraco.NodeFactory;
using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Yomego.Umbraco.Context;
using Yomego.Umbraco.Umbraco.Services.Container;

namespace Yomego.Umbraco.Umbraco.Events
{
    public class ExamineEvents : IApplicationEventHandler
    {
        private static object _lockObj = new object();
        private static bool _ran = false;

        private CoreApp<CoreServiceContainer> GetApp(bool flushCache = true)
        {
            var app = new CoreApp<CoreServiceContainer>();

            if (flushCache)
            {
                app.Services.Content.ClearCache();
            }

            return app;
        }
            
        public void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
           
        }

        void ExamineEventsGatheringNodeData(object sender, IndexingNodeDataEventArgs e)
        {
            var app = GetApp();

            var node = new Node(e.NodeId);

            if (node.Id > 0)
            {
                var parentId = (node.Parent != null) ? node.Parent.Id : 0;

                e.Fields.Add("SystemParentId", parentId.ToString());

                // NOTE: Examine prepends __Sort_ to a column name
                e.Fields.Add("SystemSortOrder", node.SortOrder.ToString().PadLeft(12, '0'));

                bool exists;
                var publishDate = node.GetProperty("publishDate", out exists);

                if (exists)
                {
                    e.Fields.Add("SystemPublishDate", publishDate.Value);
                }

                var datePublished = node.GetProperty("datePublished", out exists);

                if (!exists)
                {
                    datePublished = node.GetProperty("published", out exists);
                }

                if (datePublished != null)
                {
                    DateTime date;
                    if (DateTime.TryParse(datePublished.Value, out date))
                    {
                        e.Fields.Add("ContentDatePublished", date.ToString("yyyyMMddHHmmss"));
                    }
                }

                var culture = app.Services.Content.GetCulture(node.Id);

                if (string.IsNullOrWhiteSpace(culture))
                {
                    culture = "en-GB";
                }

                e.Fields.Add("SystemCulture", culture.Replace("-", "").ToLower());
            }
        }

        public void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            // lock - taken from: http://our.umbraco.org/wiki/reference/api-cheatsheet/using-iapplicationeventhandler-to-register-events
            if (!_ran)
            {
                lock (_lockObj)
                {
                    if (!_ran)
                    {
                        ContentService.Trashed += ContentServiceOnTrashed;
                        ExamineManager.Instance.IndexProviderCollection[Constants.Examine.MainExamineIndexProvider].GatheringNodeData += ExamineEventsGatheringNodeData;
                        _ran = true;
                    }
                }
            }
        }

        private void ContentServiceOnTrashed(IContentService sender, MoveEventArgs<IContent> moveEventArgs)
        {
            GetApp();
        }

        public void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            
        }
    }
}
