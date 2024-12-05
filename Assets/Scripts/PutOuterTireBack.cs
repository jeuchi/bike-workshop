using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;

public class PutOuterTireBack : MonoBehaviour
{
    public TutorialManager tutorialManager;

    [Header("Tire Point Settings")]
    public GameObject tire; // Reference to the tire GameObject
    public List<GameObject> tirePoints; // List of tire points
    public Material pointHighlightMaterial; // Material to highlight tire points

    private Dictionary<GameObject, Material> pointOriginalMaterials = new Dictionary<GameObject, Material>();

    private HashSet<GameObject> touchedPoints = new HashSet<GameObject>();

    [Header("Audio Settings")]
    public AudioSource positiveDing; // Reference to the AudioSource component
    public AudioSource negativeSound; // Reference to the AudioSource component
    public AudioSource clickSound; // Reference to the AudioSource component
    private bool isActive = false;

    // Getter for checking if script is enabled or disabled
    public bool isActiveScript
    {
        get { return isActive; }
    }

    void OnEnable()
    {
        isActive = true;
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

        ResetProgress();
    }

    void OnDisable()
    {
        isActive = false;
        foreach (GameObject point in tirePoints)
        {
            point.SetActive(false);
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
        touchedPoints.Clear();
        HighlightTirePoints();
        tire.transform.rotation = Quaternion.Euler(90, 0, -90);
        tutorialManager.instructionText.text = "Please use your hands to touch all the points to put the tire back. This will allow the deflated innertube to sit back in the groove of your tire, ready for the next step. ";
    }

    private void SetPointMaterial(GameObject point, Material material)
    {
        Renderer renderer = point.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = material;
        }
    }

    public void OnTireTouchPoint(GameObject point)
    {
        if (touchedPoints.Contains(point))
        {
            return;
        }

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

        // Mark point as completed
        SetPointMaterial(point, pointOriginalMaterials[point]);
        touchedPoints.Add(point);

        // Play click sound
        clickSound.Play();

        // Check if all points are completed
        if (touchedPoints.Count == tirePoints.Count)
        {
            // Play positive reward sound
            positiveDing.Play();

            OnTaskCompleted();
        }
    }

    private void OnTaskCompleted()
    {
        tutorialManager.NextStep();
    }
}
