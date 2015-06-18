using System;

namespace Yomego.Umbraco.Mvc.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class UmbracoRouteAttribute : Attribute
    {
        public UmbracoRouteAttribute(string controller, string action = "Index", string alias = null)
        {
            Controller = controller;

            Action = action;

            Alias = alias;
        }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string Alias { get; set; }
    }
}
