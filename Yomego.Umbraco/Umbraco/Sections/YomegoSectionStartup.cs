using System.Linq;
using Umbraco.Core;

namespace Yomego.Umbraco.Umbraco.Sections
{
    public class YomegoSectionStartup : ApplicationEventHandler
    {
        private void CreateIfNotExists(ApplicationContext applicationContext, string name, string alias, string icon, int sort)
        {
            //Get SectionService
            var sectionService = applicationContext.Services.SectionService;

            //Try & find a section with the alias
            var mySection = sectionService.GetSections().SingleOrDefault(x => x.Alias == alias);

            //If we can't find the section - doesn't exist
            if (mySection == null)
            {
                //So let's create it the section
                sectionService.MakeNew(name, alias, icon, sort);
            }
        }

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            CreateIfNotExists(applicationContext, "YomegoAdmin", "Yomego", "icon-car", 10);
        }
    }
}