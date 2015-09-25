using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using jumps.umbraco.usync.helpers;
using umbraco.cms.businesslogic;
using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.IO;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using DeleteEventArgs = umbraco.cms.businesslogic.DeleteEventArgs;
using Template = umbraco.cms.businesslogic.template.Template;
#pragma warning disable 618

namespace jumps.umbraco.usync
{
    /// <summary>
    ///  syncornizes the templates with the usync folder
    ///  
    /// templates / partial views are almost compleatly
    /// stored on disk, but the umbraco database stores
    /// and ID, alias and parent, to maintain structure
    /// 
    /// SyncTemplate uses the packaging API to import and
    /// export the templates. 
    /// </summary>
    public class SyncTemplate
    {

        public static void SaveToDisk(ITemplate item)
        {
            if (item != null)
            {
                try
                {
                    var packagingService = ApplicationContext.Current.Services.PackagingService;

                    XElement node = packagingService.Export(item, true);

                    if ( node.Element("Master") == null)
                    {
                        // manually add the master..
                        int masterId = GetMasterId(item);
                        if ( masterId > 0 )
                        {
                            var master = ApplicationContext.Current.Services.FileService.GetTemplate(masterId);
                            if ( master != null )
                            {
                                node.Add(new XElement("Master", master.Alias));
                            }
                        }
                    }
                    node.AddMD5Hash(item.Alias + item.Name);

                    XmlDoc.SaveElement("Template", GetTemplatePath(item), XmlDoc.ScrubFile(item.Alias) , node);
                }
                catch (Exception ex)
                {
                    LogHelper.Info<SyncTemplate>("uSync: Error Saving Template {0} - {1}", 
                        ()=>item.Name, ()=>ex.ToString());
                }
            }

        }

        public static void SaveAllToDisk()
        {
            var fileService = ApplicationContext.Current.Services.FileService;

            try
            {
                foreach(Umbraco.Core.Models.Template item in fileService.GetTemplates() )
                { 
                    SaveToDisk(item);
                }
            }
            catch( Exception ex )
            {
                LogHelper.Info<SyncTemplate>("uSync: Error saving all templates {0}", ()=> ex.ToString()); 
            }
        }

        private static int GetMasterId(ITemplate item)
        {
            // go old school to the get the master id.
            Template t = new Template(item.Id);

            if (t.MasterTemplate > 0)
                return t.MasterTemplate;

            return -1;
        }
        
        
        private static string GetTemplatePath(ITemplate item)
        {
            return GetDocPath(new Template(item.Id));
        }

        private static string GetDocPath(Template item)
        {
            string path = "";
            if (item != null)
            {
                if (item.MasterTemplate > 0)
                {
                    path = GetDocPath(new Template(item.MasterTemplate));
                }

                path = string.Format("{0}//{1}", path, XmlDoc.ScrubFile(item.Alias));
            }
            return path;
        }
        

        public static void ReadAllFromDisk()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            string path = IOHelper.MapPath(string.Format("{0}{1}",
                uSyncIO.RootFolder,
                "Template"));

            ReadFromDisk(path);

            sw.Stop();
            LogHelper.Info<uSync>("Processed Templates ({0}ms)", () => sw.ElapsedMilliseconds);
        }

        public static void ReadFromDisk(string path)
        {
            if (Directory.Exists(path))
            {
                var packagingService = ApplicationContext.Current.Services.PackagingService ; 

                foreach (string file in Directory.GetFiles(path, "*.config"))
                {
                    XElement node = XElement.Load(file) ;                                                    
                    if (node != null ) {

                        if (Tracker.TemplateChanged(node))
                        {
                            LogHelper.Info<SyncTemplate>("Importing template {0}", () => path);

                            var templates = packagingService.ImportTemplates(node);


                            // master setting - doesn't appear to be a thing on the import so we do it here...
                            if ( node.Element("Master") != null && !string.IsNullOrEmpty(node.Element("Master").Value) )
                            {
                                var master = node.Element("Master");
                                var template = templates.FirstOrDefault();

                                if (  template != null)
                                {
                                    var masterTemplate = ApplicationContext.Current.Services.FileService.GetTemplate(master.Value);

                                    if ( masterTemplate != null )
                                    {
                                        template.SetMasterTemplate(masterTemplate);
                                        ApplicationContext.Current.Services.FileService.SaveTemplate(template);
                                        LogHelper.Info<uSync>("uSync has stepped in and set the master to {0}", () => masterTemplate.Alias);
                                    }
                                }

                            }
                        }
                    }
                }

                foreach (string folder in Directory.GetDirectories(path))
                {
                    ReadFromDisk(folder);
                }
            }
        }

        public static void AttachEvents()
        {
            Template.AfterDelete += Template_AfterDelete;
            Template.AfterSave += Template_AfterSave;

            Template.New +=Template_New;;

            /*
            FileService.SavedTemplate += FileService_SavedTemplate;
            FileService.DeletedTemplate += FileService_DeletedTemplate;
             */

        }

        static void Template_New(Template sender, NewEventArgs e)
        {
            if ( !uSync.EventsPaused)
            {
                LogHelper.Info<uSync>("Template New");
                SaveToDisk(ApplicationContext.Current.Services.FileService.GetTemplate(sender.Alias));
            }
        }

        static void Template_AfterSave(Template sender, SaveEventArgs e)
        {
            if (!uSync.EventsPaused)
            {
                LogHelper.Info<uSync>("Template After Save");
                SaveToDisk(ApplicationContext.Current.Services.FileService.GetTemplate(sender.Alias));
            }
        }

        static void Template_AfterDelete(Template sender, DeleteEventArgs e)
        {
            if (!uSync.EventsPaused)
            {
                LogHelper.Info<uSync>("Template after delete");
                XmlDoc.ArchiveFile("Template", XmlDoc.ScrubFile(sender.Alias));
            }
        }

        static void FileService_DeletedTemplate(IFileService sender, DeleteEventArgs<ITemplate> e)
        {
            if (!uSync.EventsPaused)
            {
                foreach (var item in e.DeletedEntities)
                {
                    XmlDoc.ArchiveFile("Template", XmlDoc.ScrubFile(item.Alias));
                }
            }
        }

        static void FileService_SavedTemplate(IFileService sender, SaveEventArgs<ITemplate> e)
        {
            if (!uSync.EventsPaused)
            {
                LogHelper.Info<uSync>("Saving Templates");
                foreach (var item in e.SavedEntities)
                {
                    SaveToDisk(item);
                }
            }
        }
    }
}
