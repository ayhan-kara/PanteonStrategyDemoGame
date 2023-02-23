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
    /// 'Ögeler arasý boþluk'
    /// </summary>
    public float ItemSpacing { get { return itemSpacing; } }
    /// <summary>
    /// The appearance of the scroll area. 
    /// 'Kaydýrma alaný'
    /// </summary>
    public float VerticalMargin { get { return verticalMargin; } }

    public float HorizontalMargin { get { return horizontalMargin; } }
    /// <summary>
    /// Vertically scrolling? 
    /// 'Dikey kaydýrma mý?' 
    /// </summary>
    public bool Vertical { get { return vertical; } }

    public bool Horizontal { get { return horizontal; } }
    /// <summary>
    /// The width of the items. 
    /// 'Ögelerin geniþliði'
    /// </summary>
    public float Width { get { return width; } }
    /// <summary>
    /// The height of the items. 
    /// 'Ögelerin uzunluðu'
    /// </summary>
    public float Height { get { return height; } }
    /// <summary>
    /// The width of each child. 
    /// 'Her bir çocuk ögenin geniþliði'
    /// </summary>
    public float ChildWidth { get { return childWidth; } }
    /// <summary>
    /// The height of each child
    /// 'Her bir çocuk ögenin uzunluðu'
    /// </summary>
    public float ChildHeight { get { return childHeight; } }
    #endregion


    #region Monobehaviour
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectChildren = new RectTransform[rectTransform.childCount];

        //Access to children 'Çocuklara eriþim'
        for (int i = 0; i < rectTransform.childCount; i++)
        {
            rectChildren[i] = rectTransform.GetChild(i) as RectTransform;
        }

        // Subtract the margin from both sides. 'Kenar boþluklarý'
        width = rectTransform.rect.width - (2 * horizontalMargin);

        // Subtract the margin from the top and bottom. 'Yukarý ve aþaðý boþluklarý'
        height = rectTransform.rect.height - (2 * verticalMargin);

        childWidth = rectChildren[0].rect.width;
        childHeight = rectChildren[0].rect.height;

        InitializeContentVertical();
    }

    #endregion

    /// <summary>
    /// Placing children in a vertical position.
    /// 'Dikey düzlemde çocuklarý diz'
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
