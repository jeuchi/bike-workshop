using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDetection : MonoBehaviour
{
    // references to the cube GameObject and fingertip Transform for touch detection
    [SerializeField] private GameObject cube;
    [SerializeField] private Transform fingertip;

    // variables to store distance between cube and fingertip, touch threshold, and predefined distance threshold
    private float distance;
    private float touch_threshold;
    private float distance_threshold = 0.05f;

    // bools to track if the fingertip is currently touching, was previously touching, and to detect entry/exit events
    private bool isHit = false;
    private bool wasHit = false;
    private bool isEnter = false;
    private bool isExit  = false;
    // AudioSource to play sound when touch is detected
    [SerializeField] AudioSource buttonsound;

    // Update is called once per frame
    void Update()
    {
        // calculate the touch threshold based on the cube's current scale
        touch_threshold = distance_threshold + 0.5f*cube.transform.localScale.y;
        distance = DistanceCalculator(cube, fingertip);

        // update hit status and detect entry/exit events
        isHit = distance < touch_threshold;
        isEnter = isHit && !wasHit;
        isExit = !isHit && wasHit;

        // Once the fingertip leaves the cube, change the cube's color and play sound
        if (isExit)
        {
            ChangeColor(cube);
            buttonsound.Play();
        }
        
        Debug.Log(isExit);
        wasHit = isHit;
    }

    private void ChangeColor(GameObject cube)
    {
        // get the Renderer component from the new cube
        var cubeRenderer = cube.GetComponent<Renderer>();

        // generate a random color and apply it to the cube's material
        Color newColor = new Color(Random.value, Random.value, Random.value, 1.0f);
        cubeRenderer.material.SetColor("_Color", newColor);
    }

    private float DistanceCalculator(GameObject target, Transform fingertip)
    {
        return Vector3.Distance(target.transform.position, fingertip.position);
    }
}
