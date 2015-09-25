using System.Collections.Generic;
using System.Web;
using System.Web.Optimization;
using System.Linq;

namespace Website.UI
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            var styles = new List<string> 
            { 
                "~/content/css/bootstrap.css",
                "~/content/css/style.css",
                "~/content/css/font-awesome.css",
                "~/content/css/flexslider.css",
                "~/content/css/animate.css",
                "~/content/css/owl.carousel.css",
                "~/content/css/owl.theme.css",
                "~/content/css/yamm.css"
            };

            bundles.Add(new StyleBundle("~/bundles/css").Include(styles.ToArray()));

            var js = new []
            {
                "~/content/js/vendor/jquery*",
                "~/content/js/vendor/bootstrap.js",
                "~/content/js/vendor/bootstrap-hover-dropdown.js",
                "~/content/js/vendor/waypoints.js",
                "~/content/js/vendor/wow.js",
                "~/content/js/vendor/owl.carousel.js",
                "~/content/js/theme/theme.js",
                "~/content/js/custom/utils.js",
                "~/content/js/custom/global.js"
            };

            bundles.Add(new ScriptBundle("~/bundles/js").Include(js.ToArray()));

            var styleJs = new[]
            {
                "~/content/js/style/html5shiv.js",
                "~/content/js/style/respond.js",
            };

            bundles.Add(new ScriptBundle("~/bundles/js/style").Include(styleJs.ToArray()));
        }
    }
}


