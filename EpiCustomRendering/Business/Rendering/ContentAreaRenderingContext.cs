using System.Web.Mvc;
using EPiServer.Core;
using EPiServer.Web;

namespace EpiCustomRendering.Business.Rendering
{
    public class ContentAreaRenderingContext
    {
        public int CurrentItemIndex { get; protected set; }
        public ContentAreaItem CurrentItem { get; protected set; }
        public IContent CurrentItemContentData { get; protected set; }
        public DisplayOption CurrentItemDisplayOption { get; protected set; }

        public ViewDataDictionary ViewData { get; }
        public ContentArea ContentArea { get; }
        public int TotalItems { get; }


        public ContentAreaRenderingContext(ViewDataDictionary viewData, ContentArea contentArea, int availableItems)
        {
            ViewData = viewData;
            ContentArea = contentArea;
            TotalItems = availableItems;
        }
        
        public void BeginRenderingItem(ContentAreaItem contentAreaItem, IContent content, DisplayOption displayOption)
        {
            CurrentItem = contentAreaItem;
            CurrentItemContentData = content;
            CurrentItemDisplayOption = displayOption;
        }

        

        public void FinishRenderingItem()
        {
            CurrentItemIndex++;
            CurrentItem = null;
            CurrentItemContentData = null;
            CurrentItemDisplayOption = null;
        }

        public bool IsRenderingContentArea()
        {
            return CurrentItem == null && CurrentItemContentData == null && ContentArea != null;
        }

        public bool IsRenderingContentAreaItem()
        {
            return CurrentItem != null && CurrentItemContentData != null && ContentArea != null;
        }

    }
}