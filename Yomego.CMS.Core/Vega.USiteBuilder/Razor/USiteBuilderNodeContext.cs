﻿using System.Web;
using Yomego.CMS.Core.Umbraco.Model;

namespace Vega.USiteBuilder.Razor
{
    /// <summary>
    /// Base class for Razor views. Inherits directly from Umbraco's DynamicNodeContext.
    /// </summary>
    public abstract class USiteBuilderNodeContext<T> : umbraco.MacroEngines.DynamicNodeContext
        where T : Content, new()
    {
        private T _currentContent;

        /// <summary>
        /// Gets the current node being rendered throug strongly typed property
        /// </summary>
        public T CurrentContent
        {
            get
            {
                return _currentContent;
            }
        }

        public override void SetMembers(umbraco.cms.businesslogic.macro.MacroModel macro, umbraco.interfaces.INode node)
        {
            T content = ContentHelper.GetByNodeId<T>(node.Id);

            if (!HttpContext.Current.Items.Contains(node.Id))
            {
                HttpContext.Current.Items.Add(node.Id, content);
            }
            _currentContent = content;
            base.SetMembers(macro, node);
        }
    }
}
