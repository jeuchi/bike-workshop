using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;

public class RemoveTire : MonoBehaviour
{
    public TutorialManager tutorialManager;

    [Header("Audio Settings")]
    public AudioSource positiveDing; // Reference to the AudioSource component
    public AudioSource negativeSound; // Reference to the AudioSource component
    public GameObject tireHighlight; // Reference to the tire GameObject  
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable targetObject;    // The object the user interacts with
    public GameObject targetZone;      // The target zone as an XRSocketInteractor

    private Material originalTireMaterial; // The original material of the tire
    private Material originalMaterial;         // The original material of the target object

    private bool isInTargetZone = false;

    void OnEnable()
    {        
        // Enable the grab interactable, and sphere collider
        targetObject.GetComponent<SphereCollider>().enabled = true;
        targetObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>().enabled = true;

        tutorialManager.instructionText.text = "Please grab the front tire and drop it in the highlighted area";
          
        // Subscribe to the XRGrabInteractable events
       // targetObject.selectEntered.AddListener(OnObjectPickedUp);
       // targetObject.selectExited.AddListener(OnObjectDropped);

        // Subscribe to the XRSocketInteractor events
        targetZone.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>().selectEntered.AddListener(OnObjectPlacedInZone);
        originalMaterial = targetZone.GetComponent<Renderer>().material;

        // Save the original material of the tire
        originalTireMaterial = tireHighlight.GetComponent<Renderer>().material;
        tireHighlight.GetComponent<Renderer>().material = tutorialManager.highlightMaterial;
        //targetZone.selectExited.AddListener(OnObjectRemovedFromZone);
        targetObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        targetZone.SetActive(true);
        targetZone.GetComponent<Renderer>().material = tutorialManager.highlightMaterial;
    }

    void OnDisable()
    {
        // Unsubscribe from the events
        targetObject.selectEntered.RemoveListener(OnObjectPickedUp);
        targetObject.selectExited.RemoveListener(OnObjectDropped);
        targetZone.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>().selectEntered.RemoveListener(OnObjectPlacedInZone);
                   // targetZone.SetActive(false);
        //targetZone.selectExited.RemoveListener(OnObjectRemovedFromZone);
        targetZone.GetComponent<Renderer>().material = originalMaterial;
    }

    private void OnObjectPickedUp(SelectEnterEventArgs args)
    {
        // Play positive sound
        positiveDing.Play();

        // Unfreeze constraints
        targetObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        targetZone.SetActive(true);
        //tireHighlight.GetComponent<Renderer>().material = originalTireMaterial;
        // Highlight the target zone
        if (!isInTargetZone) {
            targetZone.GetComponent<Renderer>().material = tutorialManager.highlightMaterial;
        }
    }

    private void OnObjectDropped(SelectExitEventArgs args)
    {
        // Play negative sound
        negativeSound.Play(); // test that this only plays once (might play continuously)

        tutorialManager.instructionText.text = "Please pick up the front tire";

        // Highlight the tire
        tireHighlight.GetComponent<Renderer>().material = tutorialManager.highlightMaterial;

        // Remove highlight from the target zone
        targetZone.GetComponent<Renderer>().material = originalMaterial;
    }

    private void OnObjectPlacedInZone(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform == targetObject.transform)
        {
            // Play positive sound
            positiveDing.Play();

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
