using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnClick : MonoBehaviour
{
    public string sceneName; // Set the name of the scene to load in the inspector

    // This method is called when the object is clicked
    private void OnMouseDown()
    {
        SceneManager.LoadScene(sceneName);
    }
}
