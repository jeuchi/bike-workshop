using UnityEngine;

[RequireComponent(typeof(TextMesh))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class Billboarded3dText : MonoBehaviour
{
    public float distanceFromCamera = 2.0f;

    private MeshRenderer meshRenderer;
    private TextMesh textMesh;

    private void OnEnable()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        textMesh = GetComponent<TextMesh>();
    }

    private void LateUpdate()
    {
        var cam = Camera.main;
        if (cam == null) return;

        // Position the text in front of the camera
        transform.position = cam.transform.position + cam.transform.forward * distanceFromCamera;

        // Optionally, make the text face the camera
        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
    }
}
