using System.Collections.Generic;
using System.Linq;

namespace Yomego.Umbraco.Umbraco.Services.Search.Models
{
    public class Facet
    {
        public string Name { get; set; }

        public int Count { get; set; }

        private IList<string> _fields { get; set; }

        public IList<string> Fields
        {
            get
            {
                if (_fields == null)
                {
                    _fields = Name.Split(',').ToList();
                }

                return _fields;
            }
        }
    }
}