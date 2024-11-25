using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;

public class ReplaceInnerTube : MonoBehaviour
{
    public Text instructionText;   
    public GameObject oldTube;
    public GameObject newTube;
    public GameObject tubeSocket;

    private TutorialManager tutorialManager;

    void OnEnable()
    {
        tutorialManager = FindAnyObjectByType<TutorialManager>();
    
        oldTube.SetActive(true);

        oldTube.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>().selectEntered.AddListener(OnOldTubeGrabbed);
        oldTube.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>().enabled = true;

        newTube.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>().selectEntered.AddListener(OnNewTubeGrabbed);

        tubeSocket.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>().selectEntered.AddListener(OnObjectPlacedInZone);
    }

    void OnDisable()
    {
        tubeSocket.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>().selectEntered.RemoveListener(OnObjectPlacedInZone);
        Destroy(oldTube);
        Destroy(newTube);
    }

    private void OnOldTubeGrabbed(SelectEnterEventArgs args)
    {
        instructionText.text = "Take out the inner red tube from the front tire and grab a new inner tube from underneath this table";
    }

    private void OnNewTubeGrabbed(SelectEnterEventArgs args)
    {
        instructionText.text = "Place the new inner tube in the front tire";
    }

    private void OnObjectPlacedInZone(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform == newTube.transform)
        {
            tutorialManager.NextStep();
        }
    }
}
