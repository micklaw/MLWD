using System;
using System.Web;
using Newtonsoft.Json;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Templates;
using Yomego.CMS.Context;
using Yomego.CMS.Core.Umbraco.Model;
using Yomego.CMS.Core.Utils;
using umbraco.MacroEngines;
using File = Yomego.CMS.Core.Umbraco.Model.File;

namespace Yomego.CMS.Umbraco.Services
{
    public class ContentBuilderService : Service<CoreApp<CoreServiceContainer>>
    {
        private UmbracoHelper _umbracoHelper { get; set; }

        public UmbracoHelper UmbracoContextHelper
        {
            get
            {
                if (_umbracoHelper == null)
                {
                    if (UmbracoContext.Current == null)
                    {
                        throw new NullReferenceException("You need a umbraco contxt to be able to bind this type.");    
                    }

                    _umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
                }

                return _umbracoHelper;
            }
        }

        public File BindFile(string data)
        {
            return null;
        }

        public Image BindImage(string data)
        {
            Image img = BindCropImage(data);

            return img;
        }

        public CropImage BindCropImage(string data)
        {
            var img = new CropImage();

            if (String.IsNullOrWhiteSpace(data))
                return img;

            var mediaItem = new DynamicMedia(data);

            img.Url = mediaItem.GetPropertyAsString("umbracoFile");

            var mediaPath = mediaItem.GetPropertyAsString("umbracoFile");

            img.ImageCrops = JsonConvert.DeserializeObject<ImageCrops>(mediaPath);

            if (img.ImageCrops.focalPoint != null && !string.IsNullOrWhiteSpace(img.ImageCrops.src))
            {
                img.Url = img.ImageCrops.src;
            }
            else
            {
                img.Url = mediaPath;
            }

            int width = 0;
            int.TryParse(mediaItem.GetPropertyAsString("umbracoWidth"), out width);
            img.Width = width;

            int height = 0;
            int.TryParse(mediaItem.GetPropertyAsString("umbracoHeight"), out height);
            img.Height = height;

            img.Alt = mediaItem.GetPropertyAsString("altTag");

            if (string.IsNullOrWhiteSpace(img.Alt))
            {
                img.Alt = mediaItem.Name;
            }

            img.Title = mediaItem.GetPropertyAsString("titleTag");

            img.Id = mediaItem.Id;
            img.Name = mediaItem.Name;

            return img;
        }

        public object BindInt(string data)
        {
            int value;
            int.TryParse(data, out value);
            return value;
        }

        public object BindBool(string data)
        {
            // [ML] - Typically bools are '1' or '0', amending for this

            bool value;
            if (!bool.TryParse(data, out value))
            {
                if (!string.IsNullOrWhiteSpace(data))
                {
                    value = (data == "1");
                }
            }

            return value;
        }

        public object BindDecimal(string data)
        {
            decimal value;
            decimal.TryParse(data, out value);
            return value;
        }

        public object BindFloat(string data)
        {
            float value;
            float.TryParse(data, out value);
            return value;
        }

        public object BindNullFloat(string data)
        {
            float value;
            if (float.TryParse(data, out value))
            {
                return value;
            }
            else
            {
                return (float?) null;
            }
        }

        public HtmlString BindHtml(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return new HtmlString(string.Empty);
            }

            string newBody = TemplateUtilities.ParseInternalLinks(data);

            return new HtmlString(newBody);
        }

        public string BindString(string data)
        {
            return data;
        }

        public object BindNullableDateTime(string data)
        {
            DateTime value;

            if (!DateTime.TryParse(data, out value))
            {
                return null;
            }

            return value;
        }

        public object BindDayOfWeek(string data)
        {
            return Enum.Parse(typeof(DayOfWeek), data);
        }

        public object BindTimeSpan(string data)
        {
            var components = data.Split(new char[]{':'});
            
            var hour = components[0].TrimStart(new char[]{'0'});
            hour = String.IsNullOrWhiteSpace(hour) ? "0" : hour;
 
            var minutes = components[1].TrimStart(new char[] { '0' });
            minutes = String.IsNullOrWhiteSpace(minutes) ? "0" : minutes;
 
            var time = new TimeSpan(int.Parse(hour), int.Parse(minutes), 0);

            return time;
        }
        
        public object BindDateTime(string data)
        {
            DateTime value;

            DateTime.TryParse(data, out value);

            return value;
        }

        public object BindNullableInt(string data)
        {
            int nodeId = 0;
            if (int.TryParse(data, out nodeId))
            {
                return nodeId;
            }

            return null;
        }
    }
}
