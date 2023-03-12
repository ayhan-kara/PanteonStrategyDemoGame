using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MenuVisual : MonoBehaviour
{
    [SerializeField] GameObject menuOnButton;
    [SerializeField] Image menuOffButton;
    [SerializeField] Image infiniteScroll;

    public void MenuOpen()
    {
        infiniteScroll.DOFade(.3f, .5f);
        infiniteScroll.gameObject.SetActive(true);

    }

    public void MenuClose() 
    {
        infiniteScroll.DOFade(.8f, .1f);
        infiniteScroll.gameObject.SetActive(false);
    }
}
