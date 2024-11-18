using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabAndDropStep : MonoBehaviour
{
    public TMP_Text instructionText;           // Reference to the TMP_Text component
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable targetObject;    // The object the user interacts with
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor targetZone;      // The target zone as an XRSocketInteractor

    private Material originalMaterial;         // The original material of the target object

    private bool isInTargetZone = false;
    private Vector3 initialPosition;
    private Quaternion originalRotation;
    private TutorialManager tutorialManager;

    void Start()
    {
        tutorialManager = FindAnyObjectByType<TutorialManager>();

        // Save initial position
        initialPosition = targetObject.transform.position;
        originalRotation = targetObject.transform.rotation;

        switch (tutorialManager.currentStepIndex)
        {
            case 0:
                instructionText.text = "Please pick up the front tire";
                break;
            case 1:
                instructionText.text = "Please pick up the rear tire";
                break;
            default:
                instructionText.text = "You have finished the tutorial!";
                break;
        }

        // Subscribe to the XRGrabInteractable events
        targetObject.selectEntered.AddListener(OnObjectPickedUp);
        targetObject.selectExited.AddListener(OnObjectDropped);

        // Subscribe to the XRSocketInteractor events
        targetZone.selectEntered.AddListener(OnObjectPlacedInZone);
        originalMaterial = targetZone.GetComponent<Renderer>().material;
        //targetZone.selectExited.AddListener(OnObjectRemovedFromZone);
    }

    void OnDestroy()
    {
        // Unsubscribe from the events
        targetObject.selectEntered.RemoveListener(OnObjectPickedUp);
        targetObject.selectExited.RemoveListener(OnObjectDropped);
        targetZone.selectEntered.RemoveListener(OnObjectPlacedInZone);
        //targetZone.selectExited.RemoveListener(OnObjectRemovedFromZone);
    }

    private void OnObjectPickedUp(SelectEnterEventArgs args)
    {
        switch (tutorialManager.currentStepIndex)
        {
            case 0:
                instructionText.text = "Please drop the front tire in the highlighted area";
                break;
            case 1:
                instructionText.text = "Please drop the rear tire in the highlighted area";
                break;
            default:
                break;
        }

        // Highlight the target zone
        if (!isInTargetZone) {
            targetZone.GetComponent<Renderer>().material = tutorialManager.highlightMaterial;
        }
    }

    private void OnObjectDropped(SelectExitEventArgs args)
    {
        if (!isInTargetZone)
        {
            switch (tutorialManager.currentStepIndex)
            {
                case 0:
                    instructionText.text = "Please pick up the front tire";
                    break;
                case 1:
                    instructionText.text = "Please pick up the rear tire";
                    break;
                default:
                    break;
            }

            targetObject.transform.position = initialPosition;
            targetObject.transform.rotation = originalRotation;
            // Reset the Rigidbody's velocity and angular velocity
            Rigidbody rb = targetObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

        // Remove highlight from the target zone
        targetZone.GetComponent<Renderer>().material = originalMaterial;
    }

    private void OnObjectPlacedInZone(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform == targetObject.transform)
        {
            isInTargetZone = true;
            targetZone.GetComponent<Renderer>().material = originalMaterial;
            tutorialManager.NextStep();
        }
    }
}
