using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Barrack : MonoBehaviour
{
    private MeshRenderer barrackMaterial;
    private PlaceableObject placeableObject;

    private void Start()
    {
        barrackMaterial = GetComponent<MeshRenderer>();
        placeableObject = GetComponent<PlaceableObject>();
    }

    private void Update()
    {
        if (this.placeableObject.isTouchedAnything)
        {
            //Debug.LogError("!!!");
            this.placeableObject.unBuildButton.SetActive(true);
            this.placeableObject.buildButton.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Builded"))
        {
            //Debug.Log("Cant Build Here!");
            placeableObject.isTouchedAnything = true;
            DetectMaterial();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("Can Build Here!");
        placeableObject.isTouchedAnything = false;
        UnDetectMaterial();
    }
    private void OnMouseDown()
    {
        //Debug.Log("Selected");
    }

    public void DetectMaterial()
    {
        this.barrackMaterial.material.DOColor(Color.red, .5f);
    }

    public void UnDetectMaterial()
    {
        this.barrackMaterial.material.DOColor(Color.white, .5f);
    }
}
