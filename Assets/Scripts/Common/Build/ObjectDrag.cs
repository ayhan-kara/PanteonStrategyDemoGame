using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class ObjectDrag : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private PlaceableObject placeableObject;


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
            InformationManager.instance.OpenBarrackInformationPanel();
            placeableObject.isSelected = true;
            Vector3Int start = BuildingSystem.instance.gridLayout.WorldToCell(placeableObject.GetStartPosition());
            placeableObject.RemoveTiles(start);
        }
    }

    private void OnMouseDrag()
    {
        if (placeableObject.isSelected)
        {
            MoveBuild();
            CheckArea();
        }
    }

    public void MoveBuild()
    {
        Vector3 position = BuildingSystem.MousePosition();
        transform.position = BuildingSystem.instance.SnapCoordinateToGrid(position);
    }

    public void CheckArea()
    {
        var material = meshRenderer.material;

        var buildButton = placeableObject.buildButton;
        var unBuildButton = placeableObject.unBuildButton;

        if (Mathf.Abs(transform.position.x) > BuildingSystem.instance.width - .5f || Mathf.Abs(transform.position.z) > BuildingSystem.instance.height - .5f || placeableObject.isTouchedAnything)
        {
            Debug.Log("Can't Build Here");
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
