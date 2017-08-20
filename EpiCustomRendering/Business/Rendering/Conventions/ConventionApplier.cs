using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace EpiCustomRendering.Business.Rendering.Conventions
{
    public class ConventionApplier : IConventionApplier
    {
        private readonly ITagBuilderConvention[] _registry;

        public ConventionApplier(IEnumerable<ITagBuilderConvention> registry)
        {
            _registry = registry.ToArray();
        }


        public void Apply(ContentAreaRenderingContext context, TagBuilder tagBuilder)
        {
            foreach(var item in _registry)
                item.Apply(context, tagBuilder);
        }
    }
}