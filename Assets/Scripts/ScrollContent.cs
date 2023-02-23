using UnityEngine;

public class ScrollContent : MonoBehaviour
{
    #region Private
    private RectTransform rectTransform;

    private RectTransform[] rectChildren;

    private float width, height;
    private float childWidth, childHeight;

    [SerializeField] private float itemSpacing;

    [SerializeField] private float verticalMargin, horizontalMargin;

    [SerializeField] private bool horizontal, vertical;
    #endregion

    #region Public
    /// <summary>
    /// Decoupling between items. 
    /// '�geler aras� bo�luk'
    /// </summary>
    public float ItemSpacing { get { return itemSpacing; } }
    /// <summary>
    /// The appearance of the scroll area. 
    /// 'Kayd�rma alan�'
    /// </summary>
    public float VerticalMargin { get { return verticalMargin; } }

    public float HorizontalMargin { get { return horizontalMargin; } }
    /// <summary>
    /// Vertically scrolling? 
    /// 'Dikey kayd�rma m�?' 
    /// </summary>
    public bool Vertical { get { return vertical; } }

    public bool Horizontal { get { return horizontal; } }
    /// <summary>
    /// The width of the items. 
    /// '�gelerin geni�li�i'
    /// </summary>
    public float Width { get { return width; } }
    /// <summary>
    /// The height of the items. 
    /// '�gelerin uzunlu�u'
    /// </summary>
    public float Height { get { return height; } }
    /// <summary>
    /// The width of each child. 
    /// 'Her bir �ocuk �genin geni�li�i'
    /// </summary>
    public float ChildWidth { get { return childWidth; } }
    /// <summary>
    /// The height of each child
    /// 'Her bir �ocuk �genin uzunlu�u'
    /// </summary>
    public float ChildHeight { get { return childHeight; } }
    #endregion


    #region Monobehaviour
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectChildren = new RectTransform[rectTransform.childCount];

        //Access to children '�ocuklara eri�im'
        for (int i = 0; i < rectTransform.childCount; i++)
        {
            rectChildren[i] = rectTransform.GetChild(i) as RectTransform;
        }

        // Subtract the margin from both sides. 'Kenar bo�luklar�'
        width = rectTransform.rect.width - (2 * horizontalMargin);

        // Subtract the margin from the top and bottom. 'Yukar� ve a�a�� bo�luklar�'
        height = rectTransform.rect.height - (2 * verticalMargin);

        childWidth = rectChildren[0].rect.width;
        childHeight = rectChildren[0].rect.height;

        InitializeContentVertical();
    }

    #endregion

    /// <summary>
    /// Placing children in a vertical position.
    /// 'Dikey d�zlemde �ocuklar� diz'
    /// </summary>
    private void InitializeContentVertical()
    {
        float originY = 0 - (height * .5f);
        float positionOffset = childHeight * .5f;

        for (int i = 0; i < rectChildren.Length; i++)
        {
            Vector2 childPositions = rectChildren[i].localPosition;
            childPositions.y = originY + positionOffset + i * (childHeight + itemSpacing);
            rectChildren[i].localPosition = childPositions;
        }
    }
}
