using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Define the namespace, indicating this code uses API from the Oculus Interaction Samples framework
namespace Oculus.Interaction.Samples
{
    // the classname must be the same as the script name
    public class GestureDetection : MonoBehaviour
    {
        // Booleans to track if a thumb-up or thumb-down gesture is currently detected
        private bool isThumbup = false;
        private bool isThumbdown = false;
        // References to ActiveStateSelector components for thumb-up and thumb-down gestures
        // These will trigger specific actions when the gestures are detected
        [SerializeField] private ActiveStateSelector thumbup;
        [SerializeField] private ActiveStateSelector thumbdown;
        // Reference to the cube GameObject that will scale up or down based on the gestures
        [SerializeField] private GameObject cube;
        // Define the increment in scale for the cube when gestures are detected
        private Vector3 scale_inc = new Vector3(0.01f,0.01f,0.01f);

        // Start is called before the first frame update
        // This method initializes the gesture detection by subscribing to the selection and unselection events of thumb gestures
        
        void Start() // if you wanna further simply the code, you may call Sizeup() and Sizedown() here.
        {
            // Subscribe to thumb-up gesture events to update isThumbup when selected/unselected            
            thumbup.WhenSelected += () => {isThumbup = true;};
            thumbup.WhenUnselected += () => {isThumbup = false;};
            // Subscribe to thumb-down gesture events to update isThumbdown when selected/unselected
            thumbdown.WhenSelected += () => {isThumbdown = true;};
            thumbdown.WhenUnselected += () => {isThumbdown = false;};
        }

        // Update is called once per frame
        // This method checks the state of gestures and calls appropriate scaling methods
        void Update()
        {
            if(isThumbup) Sizeup();
            if(isThumbdown) Sizedown();
        }

        private void Sizeup()
        {
            // Increase the cube's scale by scale_inc
            cube.transform.localScale += scale_inc;
            Debug.Log("thumb up detected");
        }

        private void Sizedown()
        {
            // Check if cube's y-scale is greater than zero to avoid inverting the cube
            if (cube.transform.localScale.y > 0)
            {
                // Decrease the cube's scale by scale_inc
                cube.transform.localScale -= scale_inc;
                Debug.Log("thumb down detected");
            }
            Debug.Log("thumb down is detected but I am too small to shrink!");
            
        }
    }
}
