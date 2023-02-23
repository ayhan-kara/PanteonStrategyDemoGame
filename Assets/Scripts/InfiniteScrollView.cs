using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfiniteScrollView : MonoBehaviour, IBeginDragHandler,IDragHandler,IScrollHandler
{
    #region Private
    [SerializeField] private ScrollContent scrollContent;

    /// <summary>
    /// How far can objects go.
    /// 'Belirlenen alan'
    /// </summary>
    [SerializeField] private float outOfBounds;

    /// <summary>
    /// Scrool Rect component.
    /// 'Kaydýrma komponenti'
    /// </summary>
    private ScrollRect scrollRect;

    /// <summary>
    /// Last position where was he last dragged.
    /// 'Sürükleme iþleminde ki son pozisyon deðeri'
    /// </summary>
    private Vector2 lastDragPosition;

    /// <summary>
    /// On which axis the user is dragging.
    /// 'Dikey düzlemde yukarý veya aþaðý kontrolü'
    /// </summary>
    private bool negOrPosDrag;
    #endregion


    #region Monobehaviour
    /// <summary>
    /// Necessary components
    /// 'Baþlangýçta gerekli komponentlere eriþim'
    /// </summary>
    private void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        scrollRect.vertical = scrollContent.Vertical;
        scrollRect.horizontal = scrollContent.Horizontal;
        scrollRect.movementType = ScrollRect.MovementType.Unrestricted;
    }
    #endregion

    /// <summary>
    /// Whenever the user started dragging.
    /// 'Kaydýrma yapýldýysa son pozissyonu al'
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        lastDragPosition = eventData.position;
    }

    /// <summary>
    /// Whenever the user continued to drag.
    /// 'Sürükleme iþlemi devamýnda yukarý aþaðý deðerini kontrol et'
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        //Vertical or Horizontal check.
        if(scrollContent.Vertical)
        {
            negOrPosDrag = eventData.position.y > lastDragPosition.y;
        }
        else if(scrollContent.Horizontal)
        {
            negOrPosDrag = eventData.position.x > lastDragPosition.x;
        }

        lastDragPosition=eventData.position;
    }

    /// <summary>
    /// If used scrool button.
    /// 'Eðer Mouse buton deðil de kaydýrma butonu kullanýrsa'
    /// </summary>
    /// <param name="eventData"></param>
    public void OnScroll(PointerEventData eventData)
    {
        //Check Drag control up or down.
        if (scrollContent.Vertical)
        {
            negOrPosDrag = eventData.scrollDelta.y > 0;
        }
        else
        {
            negOrPosDrag = eventData.scrollDelta.y < 0;
        }
    }

    /// <summary>
    /// Is the user dragging or scrolling.
    /// 'Sürükleyeme ya da kaydýrma yapýldýðý takdirde dikey sonsuz döngüyü çaðýr'
    /// </summary>
    public void OnViewScrolling()
    {
        if (scrollContent.Vertical)
        {
            VerticalHandleScrolling();
        }
    }

    /// <summary>
    /// Vertical scroll control function.
    /// 'Dikey kaydýrma kontrolü ve tekrar iþlemi'
    /// </summary>
    private void VerticalHandleScrolling()
    {
        int currentIndex = negOrPosDrag ? scrollRect.content.childCount - 1 : 0;
        var currentItem = scrollRect.content.GetChild(currentIndex);

        if(!ReachedOutOfBounds(currentItem))
        {
            return;
        }

        int lastIndex = negOrPosDrag ? 0 : scrollRect.content.childCount - 1;
        Transform lastItem = scrollRect.content.GetChild(lastIndex);
        Vector2 newPosition = lastItem.position;

        if(negOrPosDrag) 
        {
            newPosition.y = lastItem.position.y - scrollContent.ChildHeight * 1.5f + scrollContent.ItemSpacing;
        }
        else
        {
            newPosition.y = lastItem.position.y + scrollContent.ChildHeight * 1.5f - scrollContent.ItemSpacing;
        }

        currentItem.position = newPosition;
        currentItem.SetSiblingIndex(lastIndex);
    }
    /// <summary>
    /// Call when you are outside the specified area.
    /// 'Alan dýþýna çýktýðýnda çaðýrma iþlemi'
    /// </summary>
    /// <param name="currentItem"></param>
    /// <returns></returns>
    private bool ReachedOutOfBounds(Transform currentItem)
    {
        if(scrollContent.Vertical) 
        {
            float positiveYBounds = transform.position.y + scrollContent.Height * .5f + outOfBounds;
            float negativeYBounds = transform.position.y - scrollContent.Height * .5f - outOfBounds;
            return negOrPosDrag ? currentItem.position.y - scrollContent.ChildWidth * .5f > positiveYBounds
                : currentItem.position.y + scrollContent.ChildWidth * .5f < negativeYBounds; 
        }
        else
        {
            float positiveXBounds = transform.position.x + scrollContent.Height * .5f + outOfBounds;
            float negativeXBounds = transform.position.x - scrollContent.Height * .5f - outOfBounds;
            return negOrPosDrag ? currentItem.position.x - scrollContent.ChildWidth * .5f > positiveXBounds 
                : currentItem.position.x + scrollContent.ChildWidth * .5f < negativeXBounds;
        }
    }

}
