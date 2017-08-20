using System.Web.Mvc;

namespace EpiCustomRendering.Business.Rendering.Conventions.Base
{
    public abstract class TagBuilderConventionbase : ITagBuilderConvention
    {
        public void Apply(ContentAreaRenderingContext context, TagBuilder tagBuilder)
        {
            if (ShouldApply(context, tagBuilder))
                ApplyCore(context, tagBuilder);
        }

        protected abstract void ApplyCore(ContentAreaRenderingContext context, TagBuilder tagBuilder);

        protected abstract bool ShouldApply(ContentAreaRenderingContext context, TagBuilder tagBuilder);
    }
}