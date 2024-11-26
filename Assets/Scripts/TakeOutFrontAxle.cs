using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;
using UnityEngine.UI;

public class TakeOutFrontAxle : MonoBehaviour
{
    public TutorialManager tutorialManager;

    [Header("Axle Settings")]
    public GameObject axleHighlight; // Reference to the axle GameObject
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable axle; // Reference to the axle GameObject
    public GameObject targetZone; // Reference to the target zone GameObject
    public GameObject axleSocket; // Reference to the axle socket GameObject

    private Material axleOriginalMaterial;
    private Material originalMaterial;
    private bool isInTargetZone = false;

    void OnEnable()
    {
        tutorialManager.instructionText.text = "Pull the front axle (highlighted green) out of the front tire. This will free the tire from the frame.";
        axle.selectEntered.AddListener(OnAxleGrabbed);
        axle.selectExited.AddListener(OnAxleReleased);
        axleOriginalMaterial = axleHighlight.GetComponent<Renderer>().material;
        axleHighlight.GetComponent<Renderer>().material = tutorialManager.highlightMaterial;
        targetZone.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>().selectEntered.AddListener(OnObjectPlacedInZone);
        originalMaterial = targetZone.GetComponent<Renderer>().material;
    }

    void Update()
    {
        // Check if the user is grabbing the axle and the axle is still in the collider of the axleSocket.
        // If it is, freeze the axle to only the z-axis to simulate pulling it out of the socket.
        // If the axle is not in the collider, unfreeze
        if (axle.isSelected && axleSocket.GetComponent<Collider>().bounds.Contains(axle.transform.position))
        {
            axle.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            // Also don't allow for negative z values
            if (axle.transform.position.z < axleSocket.transform.position.z)
            {
                axle.transform.position = new Vector3(axle.transform.position.x, axle.transform.position.y, axleSocket.transform.position.z);
            }
        }
        else if (!isInTargetZone)
        {
            targetZone.SetActive(true);
            //axleHighlight.GetComponent<Renderer>().material = axleOriginalMaterial;
        
            tutorialManager.instructionText.text = "Place the front axle in the target zone on the table";
            targetZone.GetComponent<Renderer>().material = tutorialManager.highlightMaterial;
            axle.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
    }

    void OnDisable()
    {
        targetZone.SetActive(false);
        targetZone.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>().selectEntered.RemoveListener(OnObjectPlacedInZone);
        axle.selectEntered.RemoveListener(OnAxleGrabbed);
        axle.selectExited.RemoveListener(OnAxleReleased);
        axleHighlight.GetComponent<Renderer>().material = axleOriginalMaterial;
    }

    private void OnAxleGrabbed(SelectEnterEventArgs args)
    {

    }

    private void OnAxleReleased(SelectExitEventArgs args)
    {
        axleHighlight.GetComponent<Renderer>().material = tutorialManager.highlightMaterial;
          //  targetZone.SetActive(false);
        //targetZone.GetComponent<Renderer>().material = originalMaterial;
    }

    private void OnObjectPlacedInZone(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform == axle.transform)
        {
            isInTargetZone = true;
            axleHighlight.GetComponent<Renderer>().material = axleOriginalMaterial;

            // Freeze axle in place
            axle.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

            tutorialManager.NextStep();
        }
    } 
}