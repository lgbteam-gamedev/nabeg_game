using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class HighlightManager : MonoBehaviour
{
    public static HighlightManager Instance { get; private set; }

    [Header("Highlight Settings")]
    public Material highlightMaterial;
    private Material originalMaterial;
    private Renderer lastHighlighted;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void HighlightObject(GameObject obj)
    {
        if (lastHighlighted != null)
        {
            lastHighlighted.material = originalMaterial;
        }

        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            originalMaterial = renderer.material;
            renderer.material = highlightMaterial;
            lastHighlighted = renderer;
        }
    }

    public void RemoveHighlight()
    {
        if (lastHighlighted != null)
        {
            lastHighlighted.material = originalMaterial;
            lastHighlighted = null;
        }
    }
}