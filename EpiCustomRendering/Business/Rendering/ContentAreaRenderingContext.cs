using System.Linq;
using System.Web.Mvc;
using EPiServer.Core;
using EPiServer.Web;

namespace EpiCustomRendering.Business.Rendering
{
    public class ContentAreaRenderingContext
    {
        public int CurrentItemIndex { get; protected set; }
        public ContentAreaItem CurrentItem { get; protected set; }

        public IContent CurrentItemContent { get; protected set; }
        public IContent PreviousItemContent { get; protected set; }
        public DisplayOption CurrentItemDisplayOption { get; protected set; }

        public ViewDataDictionary ViewData { get; }
        public ContentArea ContentArea { get; }
        public int TotalItems { get; }


        public ContentAreaRenderingContext(ViewDataDictionary viewData, ContentArea contentArea)
        {
            ViewData = viewData;
            ContentArea = contentArea;
            TotalItems = contentArea?.FilteredItems?.Count() ?? 0;
        }
        
        public void BeginRenderingItem(ContentAreaItem contentAreaItem, IContent content, DisplayOption displayOption)
        {
            CurrentItem = contentAreaItem;
            CurrentItemContent = content;
            CurrentItemDisplayOption = displayOption;
        }

        

        public void FinishRenderingItem()
        {
            
            PreviousItemContent = 
                CurrentItemIndex < TotalItems - 1
                    ? CurrentItemContent 
                    : null;
            CurrentItemIndex++;
            CurrentItem = null;
            CurrentItemContent = null;
            CurrentItemDisplayOption = null;
        }

        public bool IsRenderingContentArea()
        {
            return CurrentItem == null && CurrentItemContent == null && ContentArea != null;
        }

        public bool IsRenderingContentAreaItem()
        {
            return CurrentItem != null && CurrentItemContent != null && ContentArea != null;
        }

    }
}