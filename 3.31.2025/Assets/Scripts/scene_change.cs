using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // Set the name of the scene you want to load in the Unity Inspector
    public string sceneToLoad;

    // This method is called when another collider enters the trigger collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger has the "Player" tag
        if (other.CompareTag("Player"))
        {
            // Load the specified scene
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
