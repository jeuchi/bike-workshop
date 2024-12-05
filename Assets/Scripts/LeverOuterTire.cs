using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;
using UnityEngine.UI;

public class LeverOuterTire : MonoBehaviour
{
    public TutorialManager tutorialManager;

    [Header("Lever Settings")]
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable lever; // Reference to the lever GameObject
    public Material leverHighlightMaterial; // Material to highlight the lever when dropped
    private Vector3 leverOriginalPosition;

    [Header("Tire Point Settings")]
    public GameObject tire; // Reference to the tire GameObject
    public List<GameObject> tirePoints; // List of tire points in the correct order
    public Material pointHighlightMaterial; // Material to highlight tire points
    private Material leverOriginalMaterial;

    [Header("Audio Settings")]
    public AudioSource positiveDing; // Reference to the AudioSource component
    public AudioSource negativeSound; // Reference to the AudioSource component
    public AudioSource clickSound; // Reference to the AudioSource component
    private Dictionary<GameObject, Material> pointOriginalMaterials = new Dictionary<GameObject, Material>();
    private int currentGroupIndex = 0;
    private bool isLeverHeld = false;
    private List<List<GameObject>> pointGroups = new List<List<GameObject>>();
    private HashSet<GameObject> touchedPointsInCurrentGroup = new HashSet<GameObject>();
    private bool isActive = false;

    // Getter for checking if script is enabled or disabled
    public bool isActiveScript
    {
        get { return isActive; }
    }


    void OnEnable()
    {
        isActive = true;

        // Unfreeze lever
        lever.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

        // Subscribe to lever grab and release events
        lever.selectEntered.AddListener(OnLeverGrabbed);
        lever.selectExited.AddListener(OnLeverReleased);

        // Initially, lever is not held
        isLeverHeld = false;
        leverOriginalPosition = lever.transform.position;

        // Save the original material of the lever
        Renderer leverRenderer = lever.GetComponent<Renderer>();
        if (leverRenderer != null)
        {
            leverOriginalMaterial = leverRenderer.material;
        }

        // Save the original materials of the tire points
        foreach (GameObject point in tirePoints)
        {
            point.SetActive(true);
            Renderer renderer = point.GetComponent<Renderer>();
            if (renderer != null)
            {
                pointOriginalMaterials[point] = renderer.material;
            }
        }

        // Initialize pointGroups
        pointGroups.Add(new List<GameObject> { tirePoints[0], tirePoints[1] }); // Group 1
        pointGroups.Add(new List<GameObject> { tirePoints[2], tirePoints[3] }); // Group 2
        pointGroups.Add(new List<GameObject> { tirePoints[4], tirePoints[5] }); // Group 3

        // Reset tire points to default state
        ResetProgress();

        tutorialManager.instructionText.text = "Woohoo! Next, pick up the lever highlighted green on the desk in front of you";
    }

    void OnDisable()
    {
        isActive = false;
        
        // Unsubscribe to prevent memory leaks
        lever.selectEntered.RemoveListener(OnLeverGrabbed);
        lever.selectExited.RemoveListener(OnLeverReleased);
        foreach (GameObject point in tirePoints)
        {
            point.SetActive(false);
        }

    }

    private void OnLeverGrabbed(SelectEnterEventArgs args)
    {
        // Play positive ding
        clickSound.Play();

        isLeverHeld = true;
        HighlightLever(false); // Remove lever highlight
        HighlightTirePoints(); // Highlight the tire points
        tutorialManager.instructionText.text = "With the lever, touch the top two tire points opposite of the valve stem";
    }

    private void OnLeverReleased(SelectExitEventArgs args)
    {
        // Play negative sounds
        negativeSound.Play();

        isLeverHeld = false;
        ResetProgress();
    }

    private void HighlightLever(bool highlight)
    {
        Renderer renderer = lever.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = highlight ? leverHighlightMaterial : leverOriginalMaterial;
        }
    }

    private void HighlightTirePoints()
    {
        foreach (GameObject point in tirePoints)
        {
            SetPointMaterial(point, pointHighlightMaterial);
        }
    }

    private void ResetProgress()
    {

        if (!isLeverHeld) 
        {
            HighlightLever(true); // Highlight the lever to prompt pickup
            tutorialManager.instructionText.text = "Woohoo!! Next, pick up the lever highlighted green";
        } else {
            currentGroupIndex = 0;
            touchedPointsInCurrentGroup.Clear();
            HighlightTirePoints();
            tire.transform.rotation = Quaternion.Euler(90, 0, -90);
            tutorialManager.instructionText.text = "With the lever, touch the top two tire points";
        }
    }

    private void SetPointMaterial(GameObject point, Material material)
    {
        Renderer renderer = point.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = material;
        }
    }

    public void OnLeverTouchPoint(GameObject point)
    {
        if (!isLeverHeld)
            return;

        if (currentGroupIndex >= pointGroups.Count)
            return; // All groups completed.

        var currentGroup = pointGroups[currentGroupIndex];

        if (currentGroup.Contains(point))
        {
            if (!touchedPointsInCurrentGroup.Contains(point))
            {
                Debug.Log("Correct point touched");
                 
                // Play click sounds ding
                clickSound.Play();

                Vector3 rotation = tire.transform.rotation.eulerAngles;
                switch (tirePoints.IndexOf(point))
                {
                    case 0:
                    case 3:
                    case 5:
                        rotation.x += 3;
                        break;
                    case 1:
                    case 4:
                    case 6:
                        rotation.x -= 3;
                        break;
                    default:
                        break;
                }
                tire.transform.rotation = Quaternion.Euler(rotation);

                // Correct point touched
                SetPointMaterial(point, pointOriginalMaterials[point]);
                touchedPointsInCurrentGroup.Add(point);

                if (touchedPointsInCurrentGroup.Count == currentGroup.Count)
                {
                    // Offset the tire x axis to make it look like it's being released
                    Vector3 position = tire.transform.position;
                    position.y -= 0.005f;
                    tire.transform.position = position;

                    // Update tire instructions
                    if (currentGroupIndex == 0)
                    {
                        // Play positive ding
                        positiveDing.Play();

                        tutorialManager.instructionText.text = "With the lever, touch the next two tire points in the middle";
                    }
                    else if (currentGroupIndex == 1)
                    {
                        // Play positive ding
                        positiveDing.Play();

                        tutorialManager.instructionText.text = "With the lever, touch the last two tire points on the bottom";
                    }
                    else
                    {
                        // Play positive ding
                        positiveDing.Play();

                        tutorialManager.instructionText.text = "";
                    }

                    // All points in current group have been touched
                    touchedPointsInCurrentGroup.Clear();
                    currentGroupIndex++;

                    if (currentGroupIndex >= pointGroups.Count)
                    {
                        // All groups completed
                        OnTaskCompleted();
                    }
                }
            }
            else
            {
                // Point already touched in current group, optional handling
                Debug.Log("Point already touched in current group");
            }
        }
        // Check if the point is already been selected so ignore
        else if (point.GetComponent<Renderer>().material != pointOriginalMaterials[point])
        {
            // Play negative sound
            negativeSound.Play();

            // Incorrect point touched, reset progress
            ResetProgress();
        }
    }

    private void OnTaskCompleted()
    {
        // Play positive ding
        positiveDing.Play();

        tutorialManager.NextStep();
    }
}