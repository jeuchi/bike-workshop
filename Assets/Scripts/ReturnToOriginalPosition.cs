using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ReturnToOriginalPosition : UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Rigidbody rb;

    [SerializeField]
    private Transform targetTransform; // Reference position to drop the object

    [SerializeField]
    private float precisionThreshold = 0.1f; // Acceptable distance to the target position

    protected override void Awake()
    {
        base.Awake();
        // Store the original position and rotation
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        // Cache the Rigidbody component
        rb = GetComponent<Rigidbody>();
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        // Check if the targetTransform is set
        if (targetTransform != null)
        {
            // Calculate the distance between the current position and the target position
            float distance = Vector3.Distance(transform.position, targetTransform.position);
            Debug.Log("Distance: " + distance);

            if (distance > precisionThreshold)
            {
                // Return to the original position and rotation
                transform.position = originalPosition;
                transform.rotation = originalRotation;
            }
        }
        else
        {
            // If no targetTransform is set, default to returning to original position
            transform.position = originalPosition;
            transform.rotation = originalRotation;
        }

        // Reset the Rigidbody's velocity and angular velocity
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
