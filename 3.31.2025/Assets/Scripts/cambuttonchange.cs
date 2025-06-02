using UnityEngine;
using UnityEngine.SceneManagement;

public class SpecificSceneLoader : MonoBehaviour
{
    // Name of the scene to load
    [SerializeField] private string sceneName;

    // Method to load the specified scene
    public void LoadSpecificScene()
    {
        // Check if the scene name is valid and the scene is in the build
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError($"Scene '{sceneName}' cannot be loaded. Make sure it's in the build settings and spelled correctly.");
        }
    }
}
