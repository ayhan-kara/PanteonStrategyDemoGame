using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Barrack : BuildingFeatures
{
    int x;
    int y;

    private MeshRenderer barrackMaterial;
    private PlaceableObject placeableObject;

    Vector3 position;

    [SerializeField] Pooler soldier1Pooler;
    [SerializeField] Pooler soldier2Pooler;
    [SerializeField] Pooler soldier3Pooler;

    #region Get Soldier
    public SoldierManager GetSoldier1()
    {
        return soldier1Pooler.GetGo<SoldierManager>();
    }
    public SoldierManager GetSoldier2()
    {
        return soldier2Pooler.GetGo<SoldierManager>();
    }
    public SoldierManager GetSoldier3()
    {
        return soldier3Pooler.GetGo<SoldierManager>();
    }
    #endregion

    #region Monobehaviour
    private void Start()
    {
        barrackMaterial = GetComponent<MeshRenderer>();
        placeableObject = GetComponent<PlaceableObject>();
    }

    private void Update()
    {
        if (this.placeableObject.isTouchedAnything)
        {
            this.placeableObject.unBuildButton.SetActive(true);
            this.placeableObject.buildButton.SetActive(false);
        }
        position = this.transform.position;
    }

    #endregion


    #region Trigger
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("GridStats"))
        {
            x = other.GetComponent<GetGridStats>().x;
            y = other.GetComponent<GetGridStats>().y;
            Destroy(other.GetComponent<GridStats>());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Builded") || other.CompareTag("SelectedSoldier") || other.CompareTag("Selected"))
        {
            placeableObject.isTouchedAnything = true;
            DetectMaterial();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        placeableObject.isTouchedAnything = false;
        UnDetectMaterial();
        if (other.CompareTag("GridStats") && other.GetComponent<GridStats>() == null)
        {
            other.gameObject.AddComponent<GridStats>();
            other.GetComponent<GridStats>().x = other.GetComponent<GetGridStats>().x;
            other.GetComponent<GridStats>().y = other.GetComponent<GetGridStats>().y;
        }
    }

    #endregion

    #region Set Material Color
    public void DetectMaterial()
    {
        this.barrackMaterial.material.DOColor(Color.red, .5f);
    }

    public void UnDetectMaterial()
    {
        this.barrackMaterial.material.DOColor(Color.white, .5f);
    }

    #endregion

    #region Spawn Soldier
    public void SpawnSoldier1()
    {
        SoldierManager soldier1 = GetSoldier1();
        soldier1.transform.position = this.position; /*SoldierSpawnManager.instance.spawnPoint.position;*/
        soldier1.gameObject.SetActive(true);
        soldier1.tag = "Selected";

    }
    public void SpawnSoldier2()
    {
        SoldierManager soldier2 = GetSoldier2();
        soldier2.gameObject.SetActive(true);
        soldier2.tag = "SelectedSoldier";
    }
    public void SpawnSoldier3()
    {
        SoldierManager soldier3 = GetSoldier3();
        soldier3.gameObject.SetActive(true);
        soldier3.tag = "SelectedSoldier";
    }
    #endregion

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
        placeableObject.attackTarget.SetActive(true);
        GridAI.instance.attack = true;
        //GridAI.instance.startX = GridAI.instance.endX;
        //GridAI.instance.startY = GridAI.instance.endY;
        yield break;
    }
}
