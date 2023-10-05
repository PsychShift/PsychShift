using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outliner : MonoBehaviour
{
    [SerializeField] private bool UsesSkinnedMeshRenderer;
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private Color highlightColor = Color.blue; // Set the color you want when the player hovers over the object
    private Color nonHighlightColor;

    private void Awake()
    {
        // Get the outline material attached to the character
        Material[] materials = null;
        if(UsesSkinnedMeshRenderer)
        {
            materials = GetComponentInChildren<SkinnedMeshRenderer>().materials;
            outlineMaterial = materials[1];
        }
        else
        {
            materials = GetComponentInChildren<Renderer>().materials;
            outlineMaterial = materials[0];
        }

        // Store the original color
        nonHighlightColor = outlineMaterial.GetColor("_Outline_Color");
    }

    public void ActivateOutline(bool isActive)
    {
        if(isActive)
            outlineMaterial.SetColor("_Outline_Color", highlightColor);
        else
            outlineMaterial.SetColor("_Outline_Color", nonHighlightColor);
    }
}
