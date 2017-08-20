using System.Web.Mvc;

namespace EpiCustomRendering.Business.Rendering.Conventions
{
    public interface ITagBuilderConventionComposer
    {
        void Compose(ContentAreaRenderingContext context, TagBuilder tagBuilder);
    }
}