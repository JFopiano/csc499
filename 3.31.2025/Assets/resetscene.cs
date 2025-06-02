using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneResetter : MonoBehaviour
{
    // Public method to reload the current scene
    public void ResetScene()
    {
        // Get the active scene and reload it
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
