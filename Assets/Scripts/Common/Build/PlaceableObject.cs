using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
    public bool placed {  get; private set; }
    public Vector3Int size { get; private set; }
    private Vector3[] vertices;


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

    private void Start()
    {
        GetColliderVertex();
        CalculateSizeInCells();
    }

    public virtual void Place()
    {
        ObjectDrag drag = gameObject.GetComponent<ObjectDrag>();
        Destroy(drag);

        placed = true;
    }

    public void Builded()
    {
        BuildingSystem.instance.Builded();
    }
}
