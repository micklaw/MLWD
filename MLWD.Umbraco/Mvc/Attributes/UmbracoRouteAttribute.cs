using System;

namespace MLWD.Umbraco.Mvc.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class UmbracoRouteAttribute : Attribute
    {
        public UmbracoRouteAttribute(string controller, string action = "Index")
        {
            Controller = controller;

            Action = action;
        }

        public string Controller { get; set; }

        public string Action { get; set; }
    }
}
