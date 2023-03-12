using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PowerPlant : BuildingFeatures
{
    int x, y;

    private MeshRenderer powerPlantMaterial;
    private PlaceableObject placeableObject;


    private void Start()
    {
        powerPlantMaterial = GetComponent<MeshRenderer>();
        placeableObject = GetComponent<PlaceableObject>();
    }

    private void Update()
    {
        if (this.placeableObject.isTouchedAnything)
        {
            this.placeableObject.unBuildButton.SetActive(true);
            this.placeableObject.buildButton.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Builded") || other.CompareTag("SelectedSoldier"))
        {
            //Cant Build Here!
            placeableObject.isTouchedAnything = true;
            DetectMaterial();
        }
        if (other.CompareTag("GridStats"))
        {
            x = other.GetComponent<GetGridStats>().x;
            y = other.GetComponent<GetGridStats>().y;
            Destroy(other.GetComponent<GridStats>());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("Can Build Here!");
        placeableObject.isTouchedAnything = false;
        UnDetectMaterial();
        if (other.CompareTag("GridStats") && other.GetComponent<GridStats>() == null)
        {
            other.gameObject.AddComponent<GridStats>();
            other.GetComponent<GridStats>().x = other.GetComponent<GetGridStats>().x;
            other.GetComponent<GridStats>().y = other.GetComponent<GetGridStats>().y;
        }
    }

    public void DetectMaterial()
    {
        this.powerPlantMaterial.material.DOColor(Color.red, .5f);
    }

    public void UnDetectMaterial()
    {
        this.powerPlantMaterial.material.DOColor(Color.white, .5f);
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            GridAI.instance.endX = x;
            GridAI.instance.endY = y;
            StartCoroutine(StartAttack());
        }
    }

    IEnumerator StartAttack()
    {
        GridAI.instance.attackDistance = true;
        yield return new WaitForSeconds(.5f);
        GridAI.instance.attack = true;
        yield break;
    }
}
