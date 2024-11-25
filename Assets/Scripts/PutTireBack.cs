using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;

public class PutTireBack : MonoBehaviour
{
    public Text instructionText;   
    public GameObject tire;
    public GameObject bikeSocket;

    private TutorialManager tutorialManager;

    void Start()
    {
        tutorialManager = FindAnyObjectByType<TutorialManager>();
        instructionText.text = "Please pick up the front tire and place it back on the bike";
        tire.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>().selectEntered.AddListener(OnTireGrabbed);
        bikeSocket.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>().selectEntered.AddListener(OnObjectPlacedInZone);
    }

    void OnDisable()
    {
        tire.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>().selectEntered.RemoveListener(OnTireGrabbed);
        bikeSocket.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>().selectEntered.RemoveListener(OnObjectPlacedInZone);
    }

    private void OnTireGrabbed(SelectEnterEventArgs args)
    {
        instructionText.text = "Place the front tire back on the bike";
    }

    private void OnObjectPlacedInZone(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform == tire.transform)
        {
            Debug.Log("Tire placed in zone");
            tutorialManager.NextStep();
        }
    }
}
