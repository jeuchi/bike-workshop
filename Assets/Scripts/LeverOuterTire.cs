using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;
using UnityEngine.UI;

public class LeverOuterTire : MonoBehaviour
{
    [Header("Instruction Settings")]
    public Text instructionText;

    [Header("Lever Settings")]
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable lever; // Reference to the lever GameObject
    public Material leverHighlightMaterial; // Material to highlight the lever when dropped
    private Vector3 leverOriginalPosition;

    [Header("Tire Point Settings")]
    public GameObject tire; // Reference to the tire GameObject
    public List<GameObject> tirePoints; // List of tire points in the correct order
    public Material pointHighlightMaterial; // Material to highlight tire points
    public Material pointCompletedMaterial; // Material for completed points

    private Material leverOriginalMaterial;
    private Dictionary<GameObject, Material> pointOriginalMaterials = new Dictionary<GameObject, Material>();
    private int currentGroupIndex = 0;
    private bool isLeverHeld = false;
    private TutorialManager tutorialManager;
    private List<List<GameObject>> pointGroups = new List<List<GameObject>>();
    private HashSet<GameObject> touchedPointsInCurrentGroup = new HashSet<GameObject>();

    void Start()
    {
        tutorialManager = FindAnyObjectByType<TutorialManager>();

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

        instructionText.text = "Please pick up the lever highlighted green on the desk in front of you";
    }

    void OnDisable()
    {
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
        isLeverHeld = true;
        HighlightLever(false); // Remove lever highlight
        HighlightTirePoints(); // Highlight the tire points
        instructionText.text = "With the lever, touch the top two tire points";
    }

    private void OnLeverReleased(SelectExitEventArgs args)
    {
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
            instructionText.text = "Please pick up the lever highlighted green";
        } else {
            currentGroupIndex = 0;
            touchedPointsInCurrentGroup.Clear();
            HighlightTirePoints();
            tire.transform.rotation = Quaternion.Euler(90, 0, -90);
            instructionText.text = "With the lever, touch the top two tire points";
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
                        instructionText.text = "With the lever, touch the next two tire points in the middle";
                    }
                    else if (currentGroupIndex == 1)
                    {
                        instructionText.text = "With the lever, touch the last two tire points on the bottom";
                    }
                    else
                    {
                        instructionText.text = "";
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
            // Incorrect point touched, reset progress
            ResetProgress();
        }
    }

    private void OnTaskCompleted()
    {
        tutorialManager.NextStep();
    }
}