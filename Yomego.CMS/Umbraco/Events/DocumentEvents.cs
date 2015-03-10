using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Umbraco.Core;
using Yomego.CMS.Config.Sections.Servers;
using log4net;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.web;

namespace Yomego.CMS.Umbraco.Events
{
    /// <summary>
    /// Class is used to send a request to flush the remote cache if caching the results from using the service as an API
    /// </summary>
    public class DocumentEvents : IApplicationEventHandler
    {
        private ILog _logger { get; set; }

        private IDictionary<string, Uri> _servers { get; set; }
        private IDictionary<string, Uri> Servers
        {
            get
            {
                if (_servers == null)
                {
                    var serverConfig = ConfigurationManager.GetSection("cluster") as ServerSection;

                    if (serverConfig == null)
                    {
                        throw new NullReferenceException("The server clusters section is not found in the app.config");
                    }

                    IDictionary<string, string> servers = new Dictionary<string, string>();

                    if (serverConfig.Servers != null && serverConfig.Servers.Count > 0)
                    {
                        _servers = new Dictionary<string, Uri>();

                        foreach (ServerSectionElement server in serverConfig.Servers)
                        {
                            try
                            {
                                var url = new Uri(server.Url);

                                _servers.Add(server.Key, url);
                            }
                            catch (Exception exception)
                            {
                                _logger.Error("remote Cache Uri bad format: " + exception.Message);
                            }
                        }
                    }
                }

                return _servers;
            }
        }

        private async Task<bool> DoRequest(Uri url)
        {
            _logger.Info(string.Format("Attempting to clear cache @ endpoint - {0}", url.AbsoluteUri));

            var client = new HttpClient();

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var request = new HttpRequestMessage
            {
                RequestUri = url,
                Method = HttpMethod.Post,
                Content = new StringContent(JsonConvert.SerializeObject(string.Empty),
                                            Encoding.UTF8,
                                            "application/json")
            };

            Func<HttpResponseMessage, bool> responseAction = (response) =>
            {
                _logger.Info(string.Format("Completed cache refresh @ endpoint - {0} - status code {1}", url, response.StatusCode));

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }

                return false;
            };

            try
            {
                return await client.SendAsync(request).ContinueWith((method) => responseAction.Invoke(method.Result));
            }
            catch (Exception exception)
            {
                _logger.Error("Error refreshing cache: " + exception.Message);
            }

            return false;
        }

        private async Task<bool> ClearCacheAsync()
        {
            _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            if (Servers.Any())
            {
                var tasks = Servers.Select(server => DoRequest(server.Value)).ToList();
                // [ML] - Loop all the we requests, fire async and wait until theyre done 

                if (tasks.Any())
                {
                    foreach (var isComlpeted in await System.Threading.Tasks.Task.WhenAll(tasks))
                    {
                        if (!isComlpeted)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }


        public void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            umbraco.content.AfterUpdateDocumentCache += DocumentEventsCacheUpdated;
        }

        private void DocumentEventsCacheUpdated(Document sender, DocumentCacheEventArgs e)
        {
            Task.Factory.StartNew(async () =>
            {
                await ClearCacheAsync();
            });
        }

        public void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {

        }

        public void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {

        }
    }
}
