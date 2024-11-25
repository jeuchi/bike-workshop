using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;

public class RemoveTire : MonoBehaviour
{
    public Text instructionText;   
    public GameObject tireHighlight; // Reference to the tire GameObject  
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable targetObject;    // The object the user interacts with
    public GameObject targetZone;      // The target zone as an XRSocketInteractor

    private Material originalTireMaterial; // The original material of the tire
    private Material originalMaterial;         // The original material of the target object

    private bool isInTargetZone = false;
    private bool isPickedUp = false;
    private Vector3 initialPosition;
    private Quaternion originalRotation;
    private TutorialManager tutorialManager;

    void Start()
    {
        tutorialManager = FindAnyObjectByType<TutorialManager>();
        
        // Enable the grab interactable, and sphere collider
        targetObject.GetComponent<SphereCollider>().enabled = true;
        targetObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>().enabled = true;

        // Save initial position
        initialPosition = targetObject.transform.position;
        originalRotation = targetObject.transform.rotation;

        instructionText.text = "Please pick up the front tire";
          
        // Subscribe to the XRGrabInteractable events
        targetObject.selectEntered.AddListener(OnObjectPickedUp);
        targetObject.selectExited.AddListener(OnObjectDropped);

        // Subscribe to the XRSocketInteractor events
        targetZone.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>().selectEntered.AddListener(OnObjectPlacedInZone);
        originalMaterial = targetZone.GetComponent<Renderer>().material;

        // Save the original material of the tire
        originalTireMaterial = tireHighlight.GetComponent<Renderer>().material;
        tireHighlight.GetComponent<Renderer>().material = tutorialManager.highlightMaterial;
        //targetZone.selectExited.AddListener(OnObjectRemovedFromZone);
    }

    void OnDisable()
    {
        // Unsubscribe from the events
        targetObject.selectEntered.RemoveListener(OnObjectPickedUp);
        targetObject.selectExited.RemoveListener(OnObjectDropped);
        targetZone.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>().selectEntered.RemoveListener(OnObjectPlacedInZone);
                   // targetZone.SetActive(false);
        //targetZone.selectExited.RemoveListener(OnObjectRemovedFromZone);
    }

    private void OnObjectPickedUp(SelectEnterEventArgs args)
    {
        // Unfreeze constraints
        targetObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
targetZone.SetActive(true);
        instructionText.text = "Please drop the front tire in the highlighted area";
        tireHighlight.GetComponent<Renderer>().material = originalTireMaterial;
        // Highlight the target zone
        if (!isInTargetZone) {
            targetZone.GetComponent<Renderer>().material = tutorialManager.highlightMaterial;
        }
    }

    private void OnObjectDropped(SelectExitEventArgs args)
    {
        instructionText.text = "Please pick up the front tire";

        // Highlight the tire
        tireHighlight.GetComponent<Renderer>().material = tutorialManager.highlightMaterial;

        // Remove highlight from the target zone
        targetZone.GetComponent<Renderer>().material = originalMaterial;
    }

    private void OnObjectPlacedInZone(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform == targetObject.transform)
        {
            isInTargetZone = true;

            // Remove highlight from the tire
            tireHighlight.GetComponent<Renderer>().material = originalTireMaterial;

            // Disable the tire interactable, collider, and gravity
            /*
            targetObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>().enabled = false;
            targetObject.GetComponent<SphereCollider>().enabled = false;
            targetObject.GetComponent<Rigidbody>().useGravity = false;
            targetObject.GetComponent<Rigidbody>().isKinematic = true;
*/
            // Remove highlight from the target zone
            targetZone.GetComponent<Renderer>().material = originalMaterial;
            tutorialManager.NextStep();
        }
    }
}
