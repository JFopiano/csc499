using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnCollision : MonoBehaviour
{
    public string sceneName; // Set the name of the scene to load in the inspector

    // This method is called when the player enters the trigger collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure player has the "Player" tag
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
