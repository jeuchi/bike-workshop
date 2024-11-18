using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TutorialStep
{
    public GameObject stepObject; // The object to show/hide
    public GameObject targetPart; // For highlighting, if needed
}

public class TutorialManager : MonoBehaviour
{
    public List<TutorialStep> steps;
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
            // Optionally, handle tutorial completion here
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
            // Optionally, add animation here
            // For example: StartCoroutine(FadeIn(step.stepObject));
        }

        // Highlight the target part if needed
        if (step.targetPart != null)
        {
            HighlightPart(step.targetPart);
        }
    }

    private void HideCurrentStepObject()
    {
        var step = steps[currentStepIndex];

        if (step.stepObject != null)
        {
            // Optionally, add animation here
            // For example: StartCoroutine(FadeOut(step.stepObject));
            step.stepObject.SetActive(false);
        }

        // Remove highlight from target part if needed
        if (step.targetPart != null)
        {
            RemoveHighlightFromPart(step.targetPart);
        }
    }

    private void HighlightPart(GameObject part)
    {
        // Implement your highlighting logic here
        var highlighter = part.GetComponent<Highlighter>();
        if (highlighter != null)
        {
            highlighter.Highlight();
        }
    }

    private void RemoveHighlightFromPart(GameObject part)
    {
        // Implement your logic to remove highlighting here
        var highlighter = part.GetComponent<Highlighter>();
        if (highlighter != null)
        {
            highlighter.RemoveHighlight();
        }
    }
}
/*
    // Optional coroutine for fading in
    private IEnumerator FadeIn(GameObject obj)
    {
        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = obj.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 0;
        obj.SetActive(true);

        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime / 0.5f; // Fade in over 0.5 seconds
            yield return null;
        }
    }

    // Optional coroutine for fading out
    private IEnumerator FadeOut(GameObject obj)
    {
        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = obj.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 1;

        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime / 0.5f; // Fade out over 0.5 seconds
            yield return null;
        }

        obj.SetActive(false);
    }
}*/
