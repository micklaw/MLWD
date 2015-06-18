using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Serialization;

namespace Yomego.Umbraco.Mvc.Serializing
{
    public class ExcludeContractResolver : DefaultContractResolver
    {
        private readonly IList<string> _ignoreProperties;

        public ExcludeContractResolver(IList<string> ignoreProperties)
        {
            _ignoreProperties = ignoreProperties;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, Newtonsoft.Json.MemberSerialization memberSerialization)
        {
            var properties = base.CreateProperties(type, memberSerialization);

            return properties.Where(p => !_ignoreProperties.Contains(p.PropertyName)).ToList();
        }
    }
}
