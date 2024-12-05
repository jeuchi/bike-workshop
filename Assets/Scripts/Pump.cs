using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;

public class Pump : MonoBehaviour
{
    public TutorialManager tutorialManager;
    public GameObject pumpHandle; // Reference to the pump handle
    public Text instructions;
    public Text progress;
    public Text warning;

    [Header("Audio Settings")]
    public AudioSource positiveDing; // Reference to the AudioSource component
    public AudioSource negativeSound; // Reference to the AudioSource component
    public AudioSource clickSound; // Reference to the AudioSource component

    private float minY = 0.64f; // Minimum Y position for the pump handle
    private float maxY = 0.9f; // Maximum Y position for the pump handle
    private float rate = 10f; // PSI increase rate per unit handle movement
    private float currentPSI = 55f; // Current tire PSI
    private float targetPSI = 80; // Target tire PSI
    private float previousYPosition; // Tracks the handle's Y position in the previous frame
    private Rigidbody handleRigidbody;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    void OnEnable()
    {
        instructions.text = "Pump the front tire to reach 80 PSI. Pull the handle up and down to pump the tire.";
        progress.text = $"Current PSI: {currentPSI:F2} PSI Target PSI: {targetPSI:F2} PSI";
        warning.text = "";
        tutorialManager.instructionText.text = "Pump the front tire to reach 80 PSI. Pull the handle up and down to pump the tire.";
        tutorialManager.infoText.text = "\x25C7 Did you know? Do you remember our tip from earlier? Road bike tires need much higher pressure than car tires, sometimes up to 120 psi. That's a lot compared to just 32 psi for cars!";
        handleRigidbody = pumpHandle.GetComponent<Rigidbody>();
        grabInteractable = pumpHandle.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
            
        // Unfreeze y position of pump handle
        pumpHandle.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        
        // Subscribe to the XR Interaction Toolkit events
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    void OnDisable()
    {
        tutorialManager.instructionText.text = "";
        tutorialManager.infoText.text = "";
        handleRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        
        // Unsubscribe from the XR Interaction Toolkit events
        grabInteractable.selectEntered.RemoveListener(OnGrabbed);
        grabInteractable.selectExited.RemoveListener(OnReleased);
    }

    void OnGrabbed(SelectEnterEventArgs args)
    {
        // Play click sounds
        clickSound.Play();

        // Initialize previous position when the handle is grabbed
        previousYPosition = pumpHandle.transform.position.y;
    }

    void OnReleased(SelectExitEventArgs args)
    {
        // Optionally reset or perform cleanup when the handle is released
        if (currentPSI >= targetPSI && currentPSI < targetPSI * 1.2f)
        {
            // Play positive ding
            positiveDing.Play();

            instructions.text = "Success! Tire PSI has reached the target.";
            tutorialManager.NextStep();
        }
    }

    void Update()
    {
        // Ensure clamping while the handle is grabbed
        if (grabInteractable.isSelected)
        {
            ClampHandlePosition();

            // Check if the handle is moving down
            if (pumpHandle.transform.position.y < previousYPosition)
            {
                IncreasePSI(previousYPosition - pumpHandle.transform.position.y);
            }

            // Update the previous Y position for the next frame
            previousYPosition = pumpHandle.transform.position.y;
        }
    }

    void FixedUpdate()
    {
        ClampHandlePosition();
    }

    void ClampHandlePosition()
    {
        Vector3 currentPosition = pumpHandle.transform.position;

        // Clamp the handle's Y position dynamically
        float clampedY = Mathf.Clamp(currentPosition.y, minY, maxY);
        pumpHandle.transform.position = new Vector3(currentPosition.x, clampedY, currentPosition.z);
    }

    void IncreasePSI(float movementDelta)
    {
        float overLimit = targetPSI * 1.2f; // 20% over the target PSI

        // Increase PSI proportionally to the downward movement of the handle
        currentPSI += movementDelta * rate;

        if (currentPSI > overLimit)
        {
            // Play negative sound
            negativeSound.Play();

            // Reset progress and show warning
            warning.text = "You exceeded the safe PSI limit! Progress has been reset.";
            warning.color = Color.red;
            currentPSI = 55f; 
            return;
        } 
        else 
        {
            if (currentPSI > 60f)
            {
                warning.text = "";
                warning.color = new Color(1f, 0.5f, 0f);
            }
            instructions.text = "Pump the front tire to reach 80 PSI. Pull the handle up and down to pump the tire.";
            progress.text = $"Current PSI: {currentPSI:F2} PSI Target PSI: {targetPSI:F2} PSI";
        }

        if (currentPSI >= targetPSI)
        {
            if (currentPSI > targetPSI + 5f)
            {
                // Play negative sound
                negativeSound.Play();

                warning.text = "Warning: You are putting too much pressure on the tire!";
                warning.color = new Color(1f, 0.5f, 0f);
            } 
            else if (currentPSI > targetPSI +10f)
            {
                // Play negative sound
                negativeSound.Play();

                warning.text = "Warning: This is dangerous! Please stop!";
                warning.color = Color.red;
            } 
            else
            {
                warning.text = "";
                warning.color = new Color(1f, 0.5f, 0f);
            }
            instructions.text = "You can release the handle now";
        }
    }
}
