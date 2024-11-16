using UnityEngine;

public class Highlighter : MonoBehaviour
{
    public Material highlightMaterial;
    private Material originalMaterial;
    private Renderer objectRenderer;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        originalMaterial = objectRenderer.material;
    }

    public void Highlight()
    {
        objectRenderer.material = highlightMaterial;
    }

    public void RemoveHighlight()
    {
        objectRenderer.material = originalMaterial;
    }
}