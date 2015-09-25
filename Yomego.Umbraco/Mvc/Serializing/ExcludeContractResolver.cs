using System;
using System.Collections.Generic;
using System.Linq;
using Archetype.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Umbraco.Core.Models.PublishedContent;

namespace Yomego.Umbraco.Mvc.Serializing
{
    public class ExcludeContractResolver : DefaultContractResolver
    {
        private readonly IDictionary<Type, IList<string>> _ignoreProperties;

        private IList<string> IsAssignable(Type input)
        {
            if (input != null)
            {
                foreach (var type in _ignoreProperties.Keys)
                {
                    if (type.IsAssignableFrom(input))
                    {
                        return _ignoreProperties[type];
                    }
                }
            }

            return null;
        }

        public ExcludeContractResolver(IDictionary<Type, IList<string>> ignoreProperties)
        {
            _ignoreProperties = ignoreProperties ?? new Dictionary<Type, IList<string>>();
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            // [ML] - Potentially cache for performance improvements?

            var properties = base.CreateProperties(type, memberSerialization);

            var excludeWords = IsAssignable(type);

            if (excludeWords != null && excludeWords.Any())
            {
                return properties.Where(p => !excludeWords.Contains(p.PropertyName)).ToList();
            }

            return properties;
        }
    }
}
