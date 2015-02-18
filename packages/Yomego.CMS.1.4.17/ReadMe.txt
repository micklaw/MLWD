Thanks for installing the Yomego Umbraco CMS framework.

To full enable this service you will be required to follow a couple of manual steps:

	:NOTE: First things first. Install umbraco and create your database. Then do the below steps. :NOTE:

	1. Update RouteConfig.cs and WebApiConfig.cs
	2. Update and override your Global.asax
	3. Configure Framework

1a.	Place the following code in your RouteConfig.cs

	YomegoCMSRouteConfig.RegisterRoutes(routes);

1b.	Place the following code in your WebApiConfig.cs

	YomegoCMSApiConfig.Register(config);

2.  Amend your global asax to inherit from our CMS implementation e.g:

	/* NOTE: Do not forget to change the markup of the asax inheritance also to whatever yours is called, common gotcha.
	*	     Umbraco installed via nuget will change this to the UmbracoApplication =)
	*/

	public class MvcApplication : Yomego.CMS.MvcApplication
    {
        protected override void OnApplicationStarted(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            base.OnApplicationStarted(sender, e);
        }
    }

3. Log in to the CMS and grant the admin user access to the Yomego section. When inside, click 'syncronize doc types'

Cheers and Enjoy =)