using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    // This method reloads the current active scene
    public void ReloadScene()
    {
        Debug.Log("Reloading scene...");
        // Get the current active scene and reload it
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}