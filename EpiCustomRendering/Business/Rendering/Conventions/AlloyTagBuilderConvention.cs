using System.Collections.Generic;
using System.Web.Mvc;
using EpiCustomRendering.Business.Rendering.Conventions.Base;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web;

namespace EpiCustomRendering.Business.Rendering.Conventions
{
    public class AlloyTagBuilderConvention : ITagBuilderConvention
    {
        protected virtual void ApplyCore(ContentAreaRenderingContext context, TagBuilder tagBuilder)
        {
            var tag = GetContentAreaItemTemplateTag(context.ViewData, context.CurrentItemDisplayOption);
            tagBuilder.AddCssClass(string.Format($"block {GetTypeSpecificClasses(context.CurrentItemContentData)} {GetCssClassForTag(tag)} {tag}"));
        }

        protected virtual string GetCssClassForTag(string tagName)
        {
            if (string.IsNullOrEmpty(tagName))
                return "";

            switch (tagName.ToLower())
            {
                case "span12":
                    return "full";
                case "span8":
                    return "wide";
                case "span6":
                    return "half";
                default:
                    return string.Empty;
            }
        }

        protected virtual string GetTypeSpecificClasses(IContent content)
        {
            var cssClass = content?.GetOriginalType().Name.ToLowerInvariant() ?? string.Empty;

            var customClassContent = content as ICustomCssInContentArea;

            if (!string.IsNullOrWhiteSpace(customClassContent?.ContentAreaCssClass))
                cssClass += $" {customClassContent.ContentAreaCssClass}";

            return cssClass;
        }

        protected virtual string GetContentAreaItemTemplateTag(ViewDataDictionary viewData, DisplayOption displayOption)
        {
            if (displayOption != null)
                return displayOption.Tag;
            return GetContentAreaTemplateTag(viewData);
        }
        
        protected virtual string GetContentAreaTemplateTag(ViewDataDictionary viewData)
        {
            return viewData["tag"] as string;
        }

        public void Apply(ContentAreaRenderingContext context, TagBuilder tagBuilder)
        {
            if(context.IsRenderingContentAreaItem())
                ApplyCore(context, tagBuilder);
        }
    }
}