using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class Step1 : MonoBehaviour
{
    public TMP_Text instructionText;           // Reference to the TMP_Text component
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable targetObject;    // The object the user interacts with
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor targetZone;      // The target zone as an XRSocketInteractor

    private bool isHoldingObject = false;
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
                instructionText.text = "Please pick up the object";
                break;
        }

        // Subscribe to the XRGrabInteractable events
        targetObject.selectEntered.AddListener(OnObjectPickedUp);
        targetObject.selectExited.AddListener(OnObjectDropped);

        // Subscribe to the XRSocketInteractor events
        targetZone.selectEntered.AddListener(OnObjectPlacedInZone);
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
        isHoldingObject = true;
        switch (tutorialManager.currentStepIndex)
        {
            case 0:
                instructionText.text = "Please drop the front tire in the highlighted area";
                break;
            case 1:
                instructionText.text = "Please drop the rear tire in the highlighted area";
                break;
            default:
                instructionText.text = "Please drop the object in the highlighted area";
                break;
        }

        // Highlight the target zone
        if (!isInTargetZone) {
            targetZone.GetComponent<Highlighter>().Highlight();
        }
    }

    private void OnObjectDropped(SelectExitEventArgs args)
    {
        isHoldingObject = false;

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
                    instructionText.text = "Please pick up the object";
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
        targetZone.GetComponent<Highlighter>().RemoveHighlight();
    }

    private void OnObjectPlacedInZone(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform == targetObject.transform)
        {
            isInTargetZone = true;
            targetZone.GetComponent<Highlighter>().RemoveHighlight();
            tutorialManager.NextStep();
        }
    }

/*
    private void OnObjectRemovedFromZone(SelectExitEventArgs args)
    {
        if (args.interactableObject.transform == targetObject.transform)
        {
            isInTargetZone = false;

            if (!isHoldingObject)
            {
                instructionText.text = "Please pick up the object.";
            }
        }
    }
    */
}
