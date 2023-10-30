using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassObstacle : MonoBehaviour
{
    [SerializeField] private float transparentAlpha = 0.2f;
    private Material originalMaterial;
    private bool isTransparent = false;

    private void Start()
    {
        Renderer grassRenderer = GetComponent<Renderer>();
        originalMaterial = grassRenderer.material;
    }

    private void OnTriggerStay(Collider other)
    {
        Renderer grassRenderer = GetComponent<Renderer>();
        Material transparentMaterial = new Material(originalMaterial);
        transparentMaterial.color = new Color(1f, 1f, 1f, transparentAlpha);
        grassRenderer.material = transparentMaterial;
    }

    private void OnTriggerExit(Collider other)
    {
        Renderer grassRenderer = GetComponent<Renderer>();
        grassRenderer.material = originalMaterial;
    }
}