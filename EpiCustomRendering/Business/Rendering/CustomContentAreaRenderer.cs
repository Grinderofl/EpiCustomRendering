using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using EPiServer.Core;
using EPiServer.Core.Html.StringParsing;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using EPiServer.Web.Mvc.Html;
using EPiServer;
using EPiServer.Security;

namespace EpiCustomRendering.Business.Rendering
{
    /// <summary>
    /// Extends the default <see cref="ContentAreaRenderer"/> to apply custom CSS classes to each <see cref="ContentFragment"/>.
    /// </summary>
    public class CustomContentAreaRenderer : ContentAreaRenderer
    {
        private readonly ContentAreaRenderingContext _renderingContext = new ContentAreaRenderingContext();
        private IContentAreaLoader _contentAreaLoader;

        public CustomContentAreaRenderer(IContentAreaLoader contentAreaLoader)
        {
            if (contentAreaLoader == null) throw new ArgumentNullException(nameof(contentAreaLoader));
            _contentAreaLoader = contentAreaLoader;
        }


        public override void Render(HtmlHelper htmlHelper, ContentArea contentArea)
        {
            if (contentArea == null || contentArea.IsEmpty)
                return;

            var viewContext = htmlHelper.ViewContext;
            TagBuilder contentAreaTagBuilder = null;

            if (!IsInEditMode(htmlHelper) && ShouldRenderWrappingElement(htmlHelper))
            {
                contentAreaTagBuilder = new TagBuilder(GetContentAreaHtmlTag(htmlHelper, contentArea));
                AddNonEmptyCssClass(contentAreaTagBuilder, viewContext.ViewData["cssclass"] as string);
                AddContentAreaAttributes(contentAreaTagBuilder, viewContext.ViewData);

                viewContext.Writer.Write(contentAreaTagBuilder.ToString(TagRenderMode.StartTag));
            }
            RenderContentAreaItems(htmlHelper, contentArea.FilteredItems);
            if (contentAreaTagBuilder == null)
                return;

            viewContext.Writer.Write(contentAreaTagBuilder.ToString(TagRenderMode.EndTag));
        }

        protected virtual void AddContentAreaAttributes(TagBuilder tagBuilder, ViewDataDictionary viewContextViewData)
        {
            var contentAreaAttributes = viewContextViewData["customattributes"];

            if (contentAreaAttributes == null)
                return;

            var attributeDictionary = new RouteValueDictionary(contentAreaAttributes);
            if(!attributeDictionary.Any())
                return;

            foreach (var item in attributeDictionary)
                tagBuilder.MergeAttribute(item.Key.Replace("_", "-"), (item.Value as string));
        }

        protected override void BeforeRenderContentAreaItemStartTag(TagBuilder tagBuilder, 
            ContentAreaItem contentAreaItem)
        {
            var childrenCustomAttribute = ObtainChildrenCustomAttributes(_renderingContext);
            var attributes = childrenCustomAttribute != null
                ? new RouteValueDictionary(childrenCustomAttribute)
                    .Where(item => item.Value is string)
                : Enumerable.Empty<KeyValuePair<string, object>>();

            foreach (var attribute in attributes)
            {
                var tagBuilderAttribute = tagBuilder.Attributes
                    .ContainsKey(attribute.Key)
                        ? tagBuilder.Attributes[attribute.Key]
                        : null;

                var originalAttributeValues = SplitAttributeValues(tagBuilderAttribute);
                var additionalAttributeValues = SplitAttributeValues(attribute.Value as string);

                tagBuilder.Attributes[attribute.Key] =
                    string.Join(" ", originalAttributeValues.Union(additionalAttributeValues).Distinct());
            }
        }

        private static object ObtainChildrenCustomAttributes(ContentAreaRenderingContext renderingContext)
        {
            return renderingContext.ViewData
                .FirstOrDefault(x => x.Key.ToLowerInvariant() == "childrencustomattributes")
                .Value;
        }

        private static IEnumerable<string> SplitAttributeValues(string source)
        {
            return source?
                    .Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries) 
                    ?? Enumerable.Empty<string>();
        }

    }

    public class ContentAreaRenderingContext
    {
        public int CurrentItemIndex { get; protected set; }
        public ViewDataDictionary ViewData { get; protected set; }
        public ContentAreaItem CurrentItem { get; protected set; }
        public IContentData ContentData { get; protected set; }


        public void BeginRenderingItem(ViewDataDictionary viewData, ContentAreaItem contentAreaItem,
            IContentData content)
        {
            ViewData = viewData;
            CurrentItem = contentAreaItem;
            ContentData = content;
        }

        public void FinishRenderingItem()
        {
            CurrentItemIndex++;
            CurrentItem = null;
            ContentData = null;
            ViewData = null;
        }
    }

//    protected override string GetContentAreaItemCssClass(HtmlHelper htmlHelper, ContentAreaItem contentAreaItem)
//    {
//    var tag = GetContentAreaItemTemplateTag(htmlHelper, contentAreaItem);
//        return string.Format("block {0} {1} {2}", GetTypeSpecificCssClasses(contentAreaItem, ContentRepository), GetCssClassForTag(tag), tag);
//    }

//    /// <summary>
//    /// Gets a CSS class used for styling based on a tag name (ie a Bootstrap class name)
//    /// </summary>
//    /// <param name="tagName">Any tag name available, see <see cref="Global.ContentAreaTags"/></param>
//    private static string GetCssClassForTag(string tagName)
//    {
//    if (string.IsNullOrEmpty(tagName))
//    {
//    return "";
//}
//switch (tagName.ToLower())
//{
//case "span12":
//return "full";
//case "span8":
//return "wide";
//case "span6":
//return "half";
//default:
//return string.Empty;
//}
//}

//private static string GetTypeSpecificCssClasses(ContentAreaItem contentAreaItem, IContentRepository contentRepository)
//{
//var content = contentAreaItem.GetContent();
//var cssClass = content == null ? String.Empty : content.GetOriginalType().Name.ToLowerInvariant();

//var customClassContent = content as ICustomCssInContentArea;
//if (customClassContent != null && !string.IsNullOrWhiteSpace(customClassContent.ContentAreaCssClass))
//{
//cssClass += string.Format(" {0}", customClassContent.ContentAreaCssClass);
//}

//return cssClass;
//}
}
