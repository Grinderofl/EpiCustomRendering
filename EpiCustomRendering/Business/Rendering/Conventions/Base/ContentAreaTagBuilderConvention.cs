using System.Web.Mvc;

namespace EpiCustomRendering.Business.Rendering.Conventions.Base
{
    public abstract class ContentAreaTagBuilderConvention : TagBuilderConventionbase
    {
        protected override bool ShouldApply(ContentAreaRenderingContext context, TagBuilder tagBuilder)
        {
            return context.IsRenderingContentArea();
        }
    }
}