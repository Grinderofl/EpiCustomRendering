using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using EpiCustomRendering.Business.Rendering.Conventions;
using EPiServer.Core;
using EPiServer.Core.Html.StringParsing;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using EPiServer.Web.Mvc.Html;
using EPiServer.Security;

namespace EpiCustomRendering.Business.Rendering
{
    /// <summary>
    /// Extends the default <see cref="ContentAreaRenderer"/> to apply custom CSS classes to each <see cref="ContentFragment"/>.
    /// </summary>
    public class AlloyContentAreaRenderer : ContentAreaRenderer
    {

        private ContentAreaRenderingContext _renderingContext;
        private readonly IContentAreaLoader _contentAreaLoader;
        private readonly IConventionApplier _conventionApplier;

        public AlloyContentAreaRenderer(IContentAreaLoader contentAreaLoader, IConventionApplier conventionApplier)
        {
            if (contentAreaLoader == null) throw new ArgumentNullException(nameof(contentAreaLoader));
            _contentAreaLoader = contentAreaLoader;
            _conventionApplier = conventionApplier;
        }


        public override void Render(HtmlHelper htmlHelper, ContentArea contentArea)
        {
            if (contentArea == null || contentArea.IsEmpty)
                return;

            var viewContext = htmlHelper.ViewContext;
            TagBuilder contentAreaTagBuilder = null;

            _renderingContext = new ContentAreaRenderingContext(viewContext.ViewData, contentArea, contentArea?.FilteredItems?.Count() ?? 0);

            if (!IsInEditMode(htmlHelper) && ShouldRenderWrappingElement(htmlHelper))
            {
                contentAreaTagBuilder = new TagBuilder(GetContentAreaHtmlTag(htmlHelper, contentArea));
                AddNonEmptyCssClass(contentAreaTagBuilder, viewContext.ViewData["cssclass"] as string);
                _conventionApplier.Apply(_renderingContext, contentAreaTagBuilder);
                viewContext.Writer.Write(contentAreaTagBuilder.ToString(TagRenderMode.StartTag));
            }
            RenderContentAreaItems(htmlHelper, contentArea.FilteredItems);

            _renderingContext = null;
            if (contentAreaTagBuilder == null)
                return;

            viewContext.Writer.Write(contentAreaTagBuilder.ToString(TagRenderMode.EndTag));
        }

        protected override void RenderContentAreaItem(HtmlHelper htmlHelper, ContentAreaItem contentAreaItem, string templateTag, string htmlTag,
            string cssClass)
        {
            _renderingContext.BeginRenderingItem(contentAreaItem, _contentAreaLoader.Get(contentAreaItem), _contentAreaLoader.LoadDisplayOption(contentAreaItem));
            base.RenderContentAreaItem(htmlHelper, contentAreaItem, templateTag, htmlTag, cssClass);
            _renderingContext.FinishRenderingItem();
        }

        protected override void BeforeRenderContentAreaItemStartTag(TagBuilder tagBuilder, 
            ContentAreaItem contentAreaItem)
        {
            _conventionApplier.Apply(_renderingContext, tagBuilder);

        }
    }

}
