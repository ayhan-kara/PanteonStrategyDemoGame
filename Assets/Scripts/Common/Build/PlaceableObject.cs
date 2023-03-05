using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
    public bool placed {  get; private set; }
    public bool isTouchedAnything = false;
    public bool isSelected = true;
    public Vector3Int size { get; private set; }
    private Vector3[] vertices;
    public GameObject buildButton, unBuildButton, panel;

    private void Awake()
    {
        if (isTouchedAnything)
        {
            Debug.Break();
        }
    }
    private void Start()
    {
        GetColliderVertex();
        CalculateSizeInCells();
    }
    private void GetColliderVertex()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        vertices = new Vector3[4];
        vertices[0] = collider.center + new Vector3(-collider.size.x, -collider.size.y, -collider.size.z) * .5f;
        vertices[1] = collider.center + new Vector3(collider.size.x, -collider.size.y, -collider.size.z) * .5f;
        vertices[2] = collider.center + new Vector3(collider.size.x, -collider.size.y, collider.size.z) * .5f;
        vertices[3] = collider.center + new Vector3(-collider.size.x, -collider.size.y, collider.size.z) * .5f;
    }

    private void CalculateSizeInCells()
    {
        Vector3Int[] ver = new Vector3Int[vertices.Length];

        for (int i = 0; i < ver.Length; i++)
        {
            Vector3 worldPos = transform.TransformPoint(vertices[i]);
            ver[i] = BuildingSystem.instance.gridLayout.WorldToCell(worldPos);
        }

        size = new Vector3Int(Mathf.Abs((ver[0] - ver[1]).x), Mathf.Abs((ver[0] - ver[3]).y), 1);
    }

    public Vector3 GetStartPosition()
    {
        return transform.TransformPoint(vertices[0]);
    }


    public virtual void Place()
    {
        placed = true;
    }

    public void SetTiles(Vector3Int start, Vector3Int size)
    {
        //only barrack size
        for (int x = start.x; x < size.x - 2; x++)
        {
            for (int y = start.y; y < size.y - 2; y++)
            {
                Pathfinding.Instance.tilemap.SetTile(new Vector3Int(x, y), null);
            }
        }
    }

    public void Builded()
    {
        BuildingSystem.instance.Builded();
        tag = "Builded";
        isSelected = false;
        if (InformationManager.instance.barrackInformationPanel.activeInHierarchy) 
        {
            Debug.Log("Close Panel");
            InformationManager.instance.barrackInformationPanel.SetActive(false);
        }
    }
}
