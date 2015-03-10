﻿using System;

namespace Yomego.CMS.Core.Attributes
{
    /// <summary>
    /// Provides access to various Umbraco properties of this document type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ContentTypeAttribute : Attribute
    {
        /// <summary>
        /// Provides access to various Umbraco properties of this document type.
        /// </summary>
        public ContentTypeAttribute()
        {
            // setting up default values
            //this.IconUrl = DocumentTypeDefaultValues.IconUrl; //TODO: Take care of the defaults somewhere else
            //this.Thumbnail = DocumentTypeDefaultValues.Thumbnail; //TODO: Take care of the defaults somewhere else
            this.Description = "";
        }

        /// <summary>
        /// Name of this document type
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Checks if this is a container document type
        /// </summary>
        public bool IsContainer { get; set; }

        /// <summary>
        /// Alias of this document type. If not set, this class name will be used as alias.
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Icon property of a Document Type.
        /// Default value: folder.gif.
        /// </summary>
        public string IconUrl { get; set; }

        /// <summary>
        /// Thumbnail property of a Document Type.
        /// Default value: folder.png
        /// </summary>
        public string Thumbnail { get; set; }

        /// <summary>
        /// Description of a Document Type
        /// Default value: empty
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Get's the Default Tab
        /// </summary>
        public string DefaultTab { get; set; }

        /// <summary>
        /// Default template for this Document Type.
        /// Value of this property can be either string (name of Template that will be set as default) or Type (Type of template's masterpage, for example typeof(MyProject.MyTemplate)).
        /// Default value: If there's only one template in application that is strongly typed with this Document Type,
        /// then DefaultTemplate will be automatically set to that Document Type. If there are two or more
        /// templates strongly typed with this Document Type, then DefaultTemplate will be left empy
        /// (so user can choose template when creating the content).
        /// </summary>
        public object DefaultTemplate { get; set; }

        /// <summary>
        /// Get's the DefaultTemplate as string
        /// </summary>
        public string GetDefaultTemplateAsString()
        {
 
                string retVal = null;

                if (this.DefaultTemplate != null)
                {
                    if (this.DefaultTemplate is Type)
                    {
                        retVal = ((Type)this.DefaultTemplate).Name;
                    }
                    else
                    {
                        retVal = this.DefaultTemplate.ToString();
                    }
                }

                return retVal;
 
        }

        /// <summary>
        /// Defines which child nodes can be created as a content nodes under content node of this document type.
        /// </summary>
        public Type[] AllowedChildNodeTypes { get; set; }

        /// <summary>
        /// Defines which class properties to mix in to this doc type
        /// </summary>
        public Type[] Mixins { get; set; }

        /// <summary>
        /// Defines which parent nodes can have content nodes of this document type.
        /// </summary>
        public Type[] AllowedChildNodeTypeOf { get; set; }

        /// <summary>
        /// Defines which templates are allowed for this document type.
        /// NOTE: default value of this property is null which means that allowed templates 
        /// for this document type will be all templates that are using this document type
        /// as its generic parameter. By setting this property to some not null value, default behaviour is overriden.
        /// </summary>
        public string[] AllowedTemplates { get; set; }

        public bool AllowAtRoot { get; set; }



        /// <summary>
        /// MVC Action Name
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// MVC Controller Name
        /// </summary>
        public string Controller { get; set; }
    }
}
