using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yomego.Umbraco.Mvc.Model.Media
{
    public class Asset
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public bool HasUrl
        {
            get { return !string.IsNullOrEmpty(Url); }
        }
    }
}
