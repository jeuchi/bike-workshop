using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;

public class ReplaceInnerTube : MonoBehaviour
{
    public TutorialManager tutorialManager;

    [Header("Audio Settings")]
    public AudioSource positiveDing; // Reference to the AudioSource component
    public AudioSource negativeSound; // Reference to the AudioSource component
    public AudioSource clickSound; // Reference to the AudioSource component
    public GameObject oldTube;
    public GameObject newTube;
    public GameObject tubeSocket;

    void OnEnable()
    {    
        tutorialManager.infoText.text = "\x25C7 Did you know? Some tires don't even have tubes?? Instead, tubeless tires use a sealant (liquid) to prevent leaks. With tubeless tires, there is more maintenance. You need to fill up your sealant every 2-3 months to keep it effective!";
        oldTube.SetActive(true);

        oldTube.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>().interactionLayers = LayerMask.GetMask("Default", "Tube");
        oldTube.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>().selectEntered.AddListener(OnOldTubeGrabbed);
        oldTube.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>().enabled = true;

        newTube.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>().selectEntered.AddListener(OnNewTubeGrabbed);

        tubeSocket.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>().selectEntered.AddListener(OnObjectPlacedInZone);
    }

    void OnDisable()
    {
        tutorialManager.infoText.text = "";

        tubeSocket.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>().selectEntered.RemoveListener(OnObjectPlacedInZone);
        Destroy(oldTube);

        // Freeze inner tube
        newTube.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>().selectEntered.RemoveListener(OnNewTubeGrabbed);
        newTube.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        newTube.GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnOldTubeGrabbed(SelectEnterEventArgs args)
    {
        // Play click
        clickSound.Play();

        tutorialManager.instructionText.text = "Now place the lever down, and take out the inner red tube from the front tire and place it on the ground on your right. Then grab the new inner tube on the ground placed to your left";
    }

    private void OnNewTubeGrabbed(SelectEnterEventArgs args)
    {
        // Play click
        clickSound.Play();

        tutorialManager.instructionText.text = "Place the new inner tube in the front tire";
    }

    private void OnObjectPlacedInZone(SelectEnterEventArgs args)
    {
        // Play positive ding
        positiveDing.Play();

        if (args.interactableObject.transform == newTube.transform)
        {
            tutorialManager.NextStep();
        }
    }
}
