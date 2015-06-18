using System.Net.Http.Formatting;
using umbraco.BusinessLogic.Actions;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;

namespace Yomego.Umbraco.Umbraco.Sections
{
    [PluginController("YomegoAdmin")]
    [Tree("Yomego", "YomegoAdminTree", "Yomego admin", "icon-doc")]
    public class YomegoTreeController : TreeController
    {
        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
        {
            var nodes = new TreeNodeCollection();
            var item = this.CreateTreeNode("documenttypes", id, queryStrings, "Sync", "icon-truck", false);
            nodes.Add(item);
            return nodes;
        }

        protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
        {
            var menu = new MenuItemCollection();
            menu.DefaultMenuAlias = ActionNew.Instance.Alias;
            return menu;
        }
    }
}
