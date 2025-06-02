using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;

public class StartButtonScript : MonoBehaviour {

    private GameManager gameManager;
    private Button uiButton;
    
    // Start is called before the first frame update
    void Start() {
        // Get components
        //simpleInteractable = GetComponent<XRSimpleInteractable>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        uiButton = GetComponent<Button>();
        
        // Set up UI Button events as fallback
        if (uiButton != null) {
            uiButton.onClick.AddListener(OnUIButtonClicked);
             //Debug.LogError("UI Button events attached to " + gameObject.name);

        }
    }


    //this one activates on pressing the button
    void OnUIButtonClicked() {
        gameManager.taskPopupManager.setTaskTextByState(TaskPopupManager.TaskPopupState.HALLWAY);
        gameManager.startGame();
    }
    // Important: Unsubscribe from events when the object is destroyed
    private void OnDestroy() {
        if (uiButton != null) {
            uiButton.onClick.RemoveListener(OnUIButtonClicked);
        }
    }
}
