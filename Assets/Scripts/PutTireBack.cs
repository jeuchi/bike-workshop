using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;

public class PutTireBack : MonoBehaviour
{
    public TutorialManager tutorialManager;
    public GameObject tire;
    public GameObject tireHighlight;
    public GameObject bikeSocket;
    public GameObject newTube;
    public GameObject pumpHandle;
    public GameObject axle;
    public GameObject axleHighlight;
    public GameObject axleSocket;
    public GameObject pumpSocket;
    public GameObject pumpHighlight;

    private Material originalTireMaterial;
    private Material originalAxleMaterial;
    private Material originalPumpMaterial;

    [Header("Audio Settings")]
    public AudioSource positiveDing; // Reference to the AudioSource component
    public AudioSource negativeSound; // Reference to the AudioSource component
    public AudioSource clickSound; // Reference to the AudioSource component

    void OnEnable()
    {
        tutorialManager.infoText.text = "Did you know? Low tire pressure increases the likelihood of getting a flat tire, so make sure to keep your tires properly inflated!";
        tutorialManager.instructionText.text = "Please pick up the front tire and place it back on the bike";
        originalTireMaterial = tireHighlight.GetComponent<Renderer>().material;
        tireHighlight.GetComponent<Renderer>().material = tutorialManager.highlightMaterial;
        originalAxleMaterial = axleHighlight.GetComponent<Renderer>().material;
        axleHighlight.GetComponent<Renderer>().material = tutorialManager.highlightMaterial;
        tire.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>().enabled = true;
        originalPumpMaterial = pumpHighlight.GetComponent<Renderer>().material;
        pumpHighlight.GetComponent<Renderer>().material = tutorialManager.highlightMaterial;
        tire.GetComponent<SphereCollider>().enabled = true;
        tire.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        bikeSocket.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>().selectEntered.AddListener(OnTirePlacedOnBike);
        pumpSocket.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>().selectEntered.AddListener(OnPumpPlacedOnBike);
        axleSocket.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>().selectEntered.AddListener(OnAxlePlacedOnBike);
    }

    void OnDisable()
    {
        tutorialManager.infoText.text = "";
    }

    void Update()
    {
        // Check if the user is grabbing the axle and the axle is still in the collider of the axleSocket.
        // If it is, freeze the axle to only the z-axis to simulate pulling it out of the socket.
        // If the axle is not in the collider, unfreeze
        if (axle.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>().isSelected && axleSocket.GetComponent<Collider>().bounds.Contains(axle.transform.position))
        {
            axle.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            // Also don't allow for negative z values
            if (axle.transform.position.z < axleSocket.transform.position.z)
            {
                axle.transform.position = new Vector3(axle.transform.position.x, axle.transform.position.y, axleSocket.transform.position.z);
            }
        }
        else
        {
            axle.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
    }

    private void OnTirePlacedOnBike(SelectEnterEventArgs args)
    {
        // Play click sound
        clickSound.Play();

        if (args.interactableObject.transform == tire.transform)
        {
            tutorialManager.instructionText.text = "Place the front axle back in the front tire";
            axle.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            axle.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>().interactionLayers = LayerMask.GetMask("Default");
            newTube.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            tire.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            tireHighlight.GetComponent<Renderer>().material = originalTireMaterial;
        }
    }

    private void OnAxlePlacedOnBike(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform == axle.transform)
        {
            // Play click sound
            clickSound.Play();

            // Freeze axle in place
            axle.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            axle.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>().interactionLayers = LayerMask.GetMask("");
            tutorialManager.instructionText.text = "Place the pump handle in the pump socket";
            axleHighlight.GetComponent<Renderer>().material = originalAxleMaterial;
            // Unfreeze pump handle
            pumpHandle.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            Debug.Log("Axle placed in zone");
        }
    }

    private void OnPumpPlacedOnBike(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform == pumpHandle.transform)
        {
            // Play click sound
            clickSound.Play();

            Debug.Log("Pump placed in zone");
            pumpHighlight.GetComponent<Renderer>().material = originalPumpMaterial;

            // Play positive ding to indicate completed step
            positiveDing.Play();

            tutorialManager.NextStep();
        }
    }
}
