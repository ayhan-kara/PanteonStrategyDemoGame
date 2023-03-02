using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    private Vector3 offset;


    private void OnMouseDown()
    {
        offset = transform.position - BuildingSystem.MousePosition();
    }

    private void OnMouseDrag()
    {
        //Vector3 position = BuildingSystem.MouseWorldPosition() + offset;
        Vector3 position = BuildingSystem.MousePosition();
        transform.position = BuildingSystem.instance.SnapCoordinateToGrid(position);

        if (Mathf.Abs(transform.position.x) > BuildingSystem.instance.width - .5f  || Mathf.Abs(transform.position.z) > BuildingSystem.instance.height - .5f)
        {
            Debug.Log("111");
        }
    }
}
