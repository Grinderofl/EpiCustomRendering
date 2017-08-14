using EPiServer.Core;

namespace EpiCustomRendering.Models.Pages
{
    public interface IHasRelatedContent
    {
        ContentArea RelatedContentArea { get; }
    }
}
