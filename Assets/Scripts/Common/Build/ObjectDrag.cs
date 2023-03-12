using UnityEngine;
using DG.Tweening;

public class ObjectDrag : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private PlaceableObject placeableObject;

    public bool isDrag = false;


    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        placeableObject = GetComponent<PlaceableObject>();
    }

    private void OnMouseDown()
    {
        if (placeableObject.CompareTag("Builded") && !placeableObject.isSelected)
        {
            placeableObject.panel.SetActive(true);
            placeableObject.isSelected = true;
            if(gameObject.layer == 6)
                InformationManager.instance.OpenBarrackInformationPanel();

        }
    }

    private void OnMouseDrag()
    {
        if (placeableObject.isSelected)
        {
            gameObject.tag = "Selected";
            isDrag = true;
            MoveBuild();
            CheckArea();
        }
    }

    public void MoveBuild()
    {
        Vector3 position = BuildingSystem.MousePosition();
        transform.position = BuildingSystem.instance.SnapCoordinateToGrid(position);
        if (gameObject.layer == 7)
            transform.position += new Vector3(0, 0, .16f);
    }

    public void CheckArea()
    {
        var material = meshRenderer.material;

        var buildButton = placeableObject.buildButton;
        var unBuildButton = placeableObject.unBuildButton;


        if(gameObject.layer == 6)
        {
            if (Mathf.Abs(transform.position.x) > BuildingSystem.instance.width - .5f || Mathf.Abs(transform.position.z) > BuildingSystem.instance.height - .5f || placeableObject.isTouchedAnything)
            {
                material.DOColor(Color.red, .25f);

                buildButton.SetActive(false);
                unBuildButton.SetActive(true);
            }
            else
            {
                material.DOColor(Color.white, .5f);

                buildButton.SetActive(true);
                unBuildButton.SetActive(false);
            }
        }
        else if (gameObject.layer == 7)
        {
            if (Mathf.Abs(transform.position.x) > BuildingSystem.instance.width - .2f || Mathf.Abs(transform.position.z) > BuildingSystem.instance.height - .3f || placeableObject.isTouchedAnything)
            {
                material.DOColor(Color.red, .25f);

                buildButton.SetActive(false);
                unBuildButton.SetActive(true);
            }
            else
            {
                material.DOColor(Color.white, .5f);

                buildButton.SetActive(true);
                unBuildButton.SetActive(false);
            }
        }
    }
}
