using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;

public class PutTireBack : MonoBehaviour
{
    public TutorialManager tutorialManager;
    public GameObject tire;
    public GameObject bikeSocket;
    public GameObject newTube;
    public GameObject pumpHandle;
    public GameObject pumpSocket;

    void OnEnable()
    {
        tutorialManager.instructionText.text = "Please pick up the front tire and place it back on the bike";
        tire.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>().enabled = true;
        tire.GetComponent<SphereCollider>().enabled = true;
        tire.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        bikeSocket.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>().selectEntered.AddListener(OnTirePlacedOnBike);
        pumpSocket.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>().selectEntered.AddListener(OnPumpPlacedOnBike);
    }

    private void OnTirePlacedOnBike(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform == tire.transform)
        {
            tutorialManager.instructionText.text = "Place the pump handle in the pump socket";
            newTube.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            tire.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    private void OnPumpPlacedOnBike(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform == pumpHandle.transform)
        {
            Debug.Log("Pump placed in zone");
            tutorialManager.NextStep();
        }
    }
}
