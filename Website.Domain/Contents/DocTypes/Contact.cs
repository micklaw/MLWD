using System;
using System.Web;
using Yomego.CMS.Core.Attributes;
using Yomego.CMS.Core.Enums;
using Yomego.CMS.Core.Umbraco.Model;

namespace Website.Domain.Contents.DocTypes
{
    [ContentType(Description = "A contact us page", Controller = "Content", Action = "Contact", IconUrl = "icon-message")]
    public class Contact : Page
    {

    }
}
