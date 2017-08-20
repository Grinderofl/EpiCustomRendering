using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace EpiCustomRendering.Business.Rendering.Conventions
{
    public class TagBuilderConventionComposer : ITagBuilderConventionComposer
    {
        private readonly ITagBuilderConvention[] _registry;

        public TagBuilderConventionComposer(IEnumerable<ITagBuilderConvention> registry)
        {
            _registry = registry.ToArray();
        }


        public void Compose(ContentAreaRenderingContext context, TagBuilder tagBuilder)
        {
            foreach(var item in _registry)
                item.Apply(context, tagBuilder);
        }
    }
}