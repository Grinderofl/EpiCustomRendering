using System.Web.Mvc;
using System.Web.Routing;

namespace EpiCustomRendering.Business.Rendering.Conventions
{
    public class CustomAttributesTagBuilderConvention : ITagBuilderConvention
    {
        public virtual void Apply(ContentAreaRenderingContext context, TagBuilder tagBuilder)
        {
            if (context.IsRenderingContentArea())
                ApplyCore(context, tagBuilder, "customattributes");
            else if(context.IsRenderingContentAreaItem())
                ApplyCore(context, tagBuilder, "childrencustomattributes");
        }

        protected virtual void ApplyCore(ContentAreaRenderingContext context, TagBuilder tagBuilder, string viewDataKey)
        {
            var attributes = context.ViewData[viewDataKey];

            if (attributes == null)
                return;
            
            tagBuilder.MergeAttributes(new RouteValueDictionary(attributes));
        }
    }
}