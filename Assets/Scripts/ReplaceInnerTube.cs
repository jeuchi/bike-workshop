using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;

public class ReplaceInnerTube : MonoBehaviour
{
    public TutorialManager tutorialManager;
    public GameObject oldTube;
    public GameObject newTube;
    public GameObject tubeSocket;

    void OnEnable()
    {    
        oldTube.SetActive(true);

        oldTube.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>().interactionLayers = LayerMask.GetMask("Default", "Tube");
        oldTube.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>().selectEntered.AddListener(OnOldTubeGrabbed);
        oldTube.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>().enabled = true;

        newTube.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>().selectEntered.AddListener(OnNewTubeGrabbed);

        tubeSocket.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>().selectEntered.AddListener(OnObjectPlacedInZone);
    }

    void OnDisable()
    {
        tubeSocket.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>().selectEntered.RemoveListener(OnObjectPlacedInZone);
        Destroy(oldTube);

        // Freeze inner tube
        newTube.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>().selectEntered.RemoveListener(OnNewTubeGrabbed);
        newTube.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

    private void OnOldTubeGrabbed(SelectEnterEventArgs args)
    {
        tutorialManager.instructionText.text = "Take out the inner red tube from the front tire and grab a new inner tube from underneath this table";
    }

    private void OnNewTubeGrabbed(SelectEnterEventArgs args)
    {
        tutorialManager.instructionText.text = "Place the new inner tube in the front tire";
    }

    private void OnObjectPlacedInZone(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform == newTube.transform)
        {
            Debug.Log("Tube placed in zone");
            tutorialManager.NextStep();
        }
    }
}
