using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class FreezeScale : MonoBehaviour
{
    void Awake()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            Freeze();
            DestroyImmediate(this);
        }
    }

    void Freeze()
    {
        Vector3 originalScale = transform.localScale;
        List<Transform> children = new List<Transform>();

        foreach (Transform child in transform)
        {
            children.Add(child);
        }

        foreach (Transform child in children)
        {
            child.localScale = new Vector3(
                child.localScale.x * originalScale.x,
                child.localScale.y * originalScale.y,
                child.localScale.z * originalScale.z
            );
        }

        transform.localScale = Vector3.one;
    }
}