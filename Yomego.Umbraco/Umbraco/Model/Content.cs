using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;

namespace Yomego.Umbraco.Umbraco.Model
{
    public class Content : PublishedContentModel
    {
        public Content(IPublishedContent content) : base(content)
        {
        }
    }
}
