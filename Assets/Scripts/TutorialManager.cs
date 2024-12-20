using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class TutorialStep
{
    public GameObject stepObject; // The object to show/hide
    private Material originalMaterial;
}

public class TutorialManager : MonoBehaviour
{
    public List<TutorialStep> steps;
    public Material highlightMaterial;
    public int currentStepIndex = 0;
    public Text instructionText;
    public Text infoText;

    void Start()
    {
        // Set all stepObjects to inactive at the start
        foreach (var step in steps)
        {
            if (step.stepObject != null)
            {
                // Disable script attached to the step object
                step.stepObject.SetActive(false);
            }
        }

        ShowCurrentStep();
    }

    public void GoToQuiz()
    {
        // Hide the current step's object
        HideCurrentStepObject();

        // Go to the quiz step which is the final step
        currentStepIndex = steps.Count - 1;
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
    }

    private void HideCurrentStepObject()
    {
        var step = steps[currentStepIndex];

        if (step.stepObject != null)
        {
            step.stepObject.SetActive(false);
        }

        instructionText.text = "";
        infoText.text = "";
    }
}

