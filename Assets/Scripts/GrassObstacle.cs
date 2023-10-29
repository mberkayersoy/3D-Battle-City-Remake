using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassObstacle : MonoBehaviour
{
    private float grassMaterial;
    void Start()
    {
        grassMaterial = GetComponent<MeshRenderer>().material.color.a;
    }

    private void OnTriggerEnter(Collider other)
    {
        grassMaterial = 0.1f;
    }

    private void OnTriggerExit(Collider other)
    {
        grassMaterial = 1f;
    }
}
