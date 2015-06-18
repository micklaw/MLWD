using Our.Umbraco.Ditto;
using Our.Umbraco.Ditto.Resolvers.Archetype.Resolvers;

namespace Website.Domain.Shared.Ditto.ValueResolvers.ChildContent
{
    public class ChildContentResolverAttribute : DittoValueResolverAttribute
    {
        public ChildContentResolverAttribute()
            : base(typeof(ChildContentValueResolver))
        {
    
        }
    }
}