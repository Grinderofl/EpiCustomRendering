using System.Web.Mvc;

namespace EpiCustomRendering.Business.Rendering.Conventions.Base
{
    public abstract class ContentAreaItemTagBuilderConvention : TagBuilderConventionbase
    {
        protected override bool ShouldApply(ContentAreaRenderingContext context, TagBuilder tagBuilder)
        {
            return context.IsRenderingContentAreaItem();
        }
    }
}