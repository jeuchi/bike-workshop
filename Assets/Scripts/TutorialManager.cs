using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TutorialStep
{
    public GameObject stepObject; // The object to show/hide
    public GameObject targetPart; // For highlighting, if needed
    private Material originalMaterial;

    public void HighlightTargetPart( Material highlightMaterial)
    {
        if (targetPart != null)
        {
            originalMaterial = targetPart.GetComponent<Renderer>().material;
            targetPart.GetComponent<Renderer>().material = highlightMaterial;
        }
    }

    public void RemoveHighlightFromTargetPart()
    {
        if (targetPart != null)
        {
            targetPart.GetComponent<Renderer>().material = originalMaterial;
        }
    }
}

public class TutorialManager : MonoBehaviour
{
    public List<TutorialStep> steps;
    public Material highlightMaterial;
    public int currentStepIndex = 0;

    void Start()
    {
        // Set all stepObjects to inactive at the start
        foreach (var step in steps)
        {
            if (step.stepObject != null)
            {
                step.stepObject.SetActive(false);
            }
        }

        ShowCurrentStep();
    }

    public void NextStep()
    {
        if (currentStepIndex < steps.Count - 1)
        {
            // Hide the current step's object
            HideCurrentStepObject();

            currentStepIndex++;
            ShowCurrentStep();
        }
        else
        {
            Debug.Log("Tutorial Completed!");
        }
    }

    private void ShowCurrentStep()
    {
        var step = steps[currentStepIndex];

        // Show the current step's object
        if (step.stepObject != null)
        {
            step.stepObject.SetActive(true);
        }

        // Highlight the target part if needed
        if (step.targetPart != null)
        {
            step.HighlightTargetPart(highlightMaterial);
        }
    }

    private void HideCurrentStepObject()
    {
        var step = steps[currentStepIndex];

        if (step.stepObject != null)
        {
            step.stepObject.SetActive(false);
        }

        // Remove highlight from target part if needed
        if (step.targetPart != null)
        {
            step.RemoveHighlightFromTargetPart();
        }
    }
}

