using System.Net.Http.Formatting;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;
using umbraco.BusinessLogic.Actions;

namespace MLWD.Umbraco.Umbraco.Sections
{
    [PluginController("MLWDAdmin")]
    [Tree("MLWD", "MLWDAdminTree", "MLWD admin", "icon-doc")]
    public class MLWDTreeController : TreeController
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
