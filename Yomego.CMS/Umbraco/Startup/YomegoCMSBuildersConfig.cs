using System;
using System.Web;
using Yomego.CMS.Core.Umbraco.Model;
using Yomego.CMS.Umbraco.Services;

namespace Yomego.CMS.Umbraco.Startup
{
    public class YomegoCMSBuildersConfig
    {
        public static void Register()
        {
            App.ResolveUsing<ContentBuilderService>(new ContentBuilderService());

            UmbracoContentService.Register<ContentBuilderService, File>(s => s.BindFile);
            UmbracoContentService.Register<ContentBuilderService, Image>(s => s.BindImage);
            UmbracoContentService.Register<ContentBuilderService, CropImage>(s => s.BindCropImage);
            UmbracoContentService.Register<ContentBuilderService, int>(s => s.BindInt);
            UmbracoContentService.Register<ContentBuilderService, int?>(s => s.BindNullableInt);
            UmbracoContentService.Register<ContentBuilderService, float>(s => s.BindFloat);
            UmbracoContentService.Register<ContentBuilderService, float?>(s => s.BindNullFloat);
            UmbracoContentService.Register<ContentBuilderService, decimal>(s => s.BindDecimal);
            UmbracoContentService.Register<ContentBuilderService, bool>(s => s.BindBool);
            UmbracoContentService.Register<ContentBuilderService, IHtmlString>(s => s.BindHtml);
            UmbracoContentService.Register<ContentBuilderService, HtmlString>(s => s.BindHtml);
            UmbracoContentService.Register<ContentBuilderService, string>(s => s.BindString);
            UmbracoContentService.Register<ContentBuilderService, DateTime>(s => s.BindDateTime);
            UmbracoContentService.Register<ContentBuilderService, DateTime?>(s => s.BindNullableDateTime);
            UmbracoContentService.Register<ContentBuilderService, DayOfWeek>(s => s.BindDayOfWeek);
            UmbracoContentService.Register<ContentBuilderService, TimeSpan>(s => s.BindTimeSpan);
        }
    }
}