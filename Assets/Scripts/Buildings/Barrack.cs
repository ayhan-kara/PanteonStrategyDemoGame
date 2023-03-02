using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrack : MonoBehaviour
{
    private MeshRenderer barrackMaterial;

    private void Start()
    {
        barrackMaterial = GetComponent<MeshRenderer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject)
        {
            Debug.Log("Cant Build Here!");
            ChangeMaterial();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject) 
        {
            Debug.Log("Can Build Here!");
            this.barrackMaterial.material.color = Color.white;
        }
    }
    private void OnMouseDown()
    {
        //Debug.Log("Selected");
    }

    public void ChangeMaterial()
    {
        barrackMaterial.material.color = Color.red;
    }
}
