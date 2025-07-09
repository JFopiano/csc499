using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using UnityEngine.UI;

// GameManager class serves as the central hub for managing game state,
// player progression, and coordinating various sub-managers.
public class GameManager : MonoBehaviour {

    // Task Popup
    //TaskPopupManager Script
    // Reference to the TaskPopupManager for displaying task-related UI.
    public TaskPopupManager taskPopupManager;


    // Control Manager
    // Reference to the GameControlManager for handling player movement controls.
    public GameControlManager gameControlManager;

    // User Data Manager
    // Reference to the UserDataManager for saving and loading player data.
    public UserDataManager userDataManager;

    // SVM Manager
    // Reference to the SVMManager for handling Support Vector Machine logic and visualizations.
    public SVMManager svmManager;

    // Serialized field for the CSV file containing data for Level 4's SVM.
    [SerializeField]
    public TextAsset level4SVMCSVFile;

    


    // Doors
    // Reference to the DoorManager for controlling door states and level progression.
    private DoorManager doorManager;

    // Arrow Manager
    // Reference to the ArrowManager for guiding the player with visual cues.
    public ArrowManager arrowManager;

    // Reference to the surveyManager for handling in-game surveys.
    public surveyManager surveyManager;

    // Serialized field for the parent GameObject of Level 1's guidance arrows.
    [SerializeField]
    public GameObject level1ArrowsGameObjectParent;

    // Serialized field for the parent GameObject of Level 2's guidance arrows.
    [SerializeField]
    public GameObject level2ArrowsGameObjectParent;

    // Serialized field for the parent GameObject of Level 3's guidance arrows.
    [SerializeField]
    public GameObject level3ArrowsGameObjectParent;


    // Serialized field for the parent GameObject of Level 4's guidance arrows.
    [SerializeField]
    public GameObject level4ArrowsGameObjectParent;

    // Reference to the decision plane GameObject for Level 1. (Currently not explicitly used in Start/Update)
    private GameObject decisionPlaneLevel1;

    // Flag indicating if the game has been started by the player.
    public bool startedGame;

    // Flags indicating if the player has entered each respective level.
    public bool enteredLevel1 = false;
    public bool enteredLevel2 = false;
    public bool enteredLevel3 = false;
    public bool enteredLevel4 = false;

    // Flags indicating if the player has solved the puzzle/task in each respective level.
    public bool level1Solved = false;
    public bool level2Solved = false;
    public bool level3Solved = false;
    public bool level4Solved = false;

    // Flags indicating if the player has correctly answered the survey for each respective level.
    public bool correctlyAnsweredLevel1 = false;
    public bool correctlyAnsweredLevel2 = false;
    public bool correctlyAnsweredLevel3 = false;
    public bool correctlyAnsweredLevel4 = false;

    // Flag to track if the task hologram has been successfully attached to the player's left hand.
    private bool taskHologramInLeftHand = false;

    // Stores the previous state of the secondary button on the left controller to detect press events.
    private bool previousButtonState = false;

    // Game State variables
    // Flag indicating if the game is currently active (e.g., not paused or in a menu). (Currently not explicitly used)
    public bool isGameActive = false;

    // Example of a shared vector. (Currently not explicitly used beyond initialization)
    public Vector3 sharedVector = new Vector3(1.0f, 2.0f, 3.0f);

    // Singleton instance for GameManager. (Currently not enforced or used as a typical singleton access pattern)
    private static GameManager instance;

    // Checks if the secondary button on the left XR controller was pressed in the current frame.
    private bool CheckSecondaryButtonPressedOnLeftController() {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, devices);
        
        bool currentButtonState = false;
        foreach (var device in devices) {
            if (device.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryButtonPressed)) {
                currentButtonState |= secondaryButtonPressed;
            }
        }

        // Only trigger on button press (transition from not pressed to pressed)
        bool buttonPressed = currentButtonState && !previousButtonState;
        previousButtonState = currentButtonState; // Update the previous state for the next frame
        
        return buttonPressed;
    }

    // Coroutine that continuously tries to find the "PlayerTaskHologram" and "Left Controller" GameObjects
    // and parent the hologram to the controller. This ensures the hologram stays with the player's hand.
    private IEnumerator<object> keepTryingToPutTaskHologramInPlayerLeftHand() {
        while (!taskHologramInLeftHand) {  // Run continuously until the hologram is attached
            GameObject taskHologram = GameObject.Find("PlayerTaskHologram");
            GameObject leftController = GameObject.Find("Left Controller");
            if(taskHologram != null && leftController != null) {
                // Parent the hologram to the left controller
                taskHologram.transform.SetParent(leftController.transform);
                // Set a fixed local position and rotation relative to the controller
                taskHologram.transform.localPosition = new Vector3(0.01f, 0.2f, 0.3f);
                taskHologram.transform.localRotation = Quaternion.identity;
                taskHologramInLeftHand = true; // Mark as attached
            }
            else {
                Debug.LogError("Could not find left controller to attach task hologram to, retrying in 1 second");
            }
            yield return new WaitForSeconds(1f); // Wait for 1 second before retrying
        }
    }

    
    // Called to start the game, typically triggered by a player action (e.g., pressing a start button).
    public void startGame() {
        startedGame = true; // Set the game started flag
        gameControlManager.toggleMovement(true); // Enable player movement
        GameObject.Find("StartingHologram").SetActive(false); // Disable the initial starting hologram
        arrowManager.startLevel1ArrowLightingSequence(); // Start the guidance arrows for level 1
    }
    
    // Initializes all the necessary manager components.
    void initManagers() {
        
        // Task Manager Initialization
        this.taskPopupManager = new TaskPopupManager();

        // Door Manager Initialization, passing a reference to this GameManager
        this.doorManager = new DoorManager(this);

        // Control Manager Initialization
        this.gameControlManager = new GameControlManager();

        // SVM Manager Initialization
        this.svmManager = new SVMManager();

        // Arrow Manager Initialization, passing this GameManager (as MonoBehaviour for coroutines) and arrow parent GameObjects
        this.arrowManager = new ArrowManager(this, level1ArrowsGameObjectParent, level2ArrowsGameObjectParent, level3ArrowsGameObjectParent, level4ArrowsGameObjectParent);

        // Survey Manager Initialization
        this.surveyManager = new surveyManager();

        // User Data Manager Initialization
        this.userDataManager = new UserDataManager();
    }


    // Start is called before the first frame update by Unity.
    void Start() {
        // Initialize Managers
        initManagers();

        gameControlManager.toggleMovement(true); // Enable player movement initially (might be overridden shortly after)
        // Pre-create SVM instances for the first two levels
        // svmManager.createLevel1SVM();
        // svmManager.createLevel2SVM();

        // Set the initial task text to the starting message
        taskPopupManager.setTaskTextByState(TaskPopupManager.TaskPopupState.START);

        // Set the start game to false because the player has just entered the scene/game.
        startedGame = false;
        gameControlManager.toggleMovement(false); // Disable player movement until the game is explicitly started

        // Start the coroutine to attach the task hologram to the player's hand
        StartCoroutine(keepTryingToPutTaskHologramInPlayerLeftHand());
        //arrowManager.startLevel1ArrowLightingSequence(); // This line is commented out, arrows likely start via startGame()

    }

    // Update is called once per frame by Unity.
    void Update() {
        // Check if the secondary button on the left controller is pressed
        if(CheckSecondaryButtonPressedOnLeftController()) {
            // Toggle the visibility of the task popup
            taskPopupManager.ToggleTaskPopup();
        }
    }
}
