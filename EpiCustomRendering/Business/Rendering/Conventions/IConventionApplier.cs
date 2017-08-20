using System.Web.Mvc;

namespace EpiCustomRendering.Business.Rendering.Conventions
{
    public interface IConventionApplier
    {
        void Apply(ContentAreaRenderingContext context, TagBuilder tagBuilder);
    }
}