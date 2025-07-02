using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// Manages the state and interactions for doors in different levels of the game.
// It handles opening/closing doors based on player interaction and game progression.
public class DoorManager {
    // GameObjects representing the parent containers for doors of each level.
    // These are expected to be found in the scene by name.
    private GameObject level1DoorParent; // Single Door for Level 1
    private GameObject level2DoorParent; // Double Doors for Level 2
    private GameObject level3DoorParent; // Double Doors for Level 3
    private GameObject level4DoorParent; // Double Doors for Level 4

    // Boolean flags to track the open/closed state of each door.
    private bool level1DoorOpen = false;
    private bool level2DoorRightDoorOpen = false;
    private bool level2DoorLeftDoorOpen = false;
    private bool level3DoorRightDoorOpen = false;
    private bool level3DoorLeftDoorOpen = false;
    private bool level4DoorRightDoorOpen = false;
    private bool level4DoorLeftDoorOpen = false;

    // Reference to the GameManager to access game state and other managers.
    public GameManager gameManager;

    // Constructor for DoorManager.
    // Initializes references to door GameObjects and sets up XR interactions.
    public DoorManager(GameManager gameManager) {
        this.gameManager = gameManager;
        // Find door parent GameObjects in the scene by their names.
        this.level1DoorParent = GameObject.Find("Level1RoomDoorParent");
        this.level2DoorParent = GameObject.Find("Level2RoomDoorParent"); 
        this.level3DoorParent = GameObject.Find("Level3RoomDoorParent");
        this.level4DoorParent = GameObject.Find("Level4RoomDoorParent");

        // Check if all door parents were found. If not, log an error as it will break functionality.
        if(level1DoorParent == null || level2DoorParent == null || level3DoorParent == null || level4DoorParent == null) {
            Debug.LogError("[DoorManager] One or many door parents not found. The game will not function properly.");
            Debug.Log("[DoorManager] Door Parents: " + level1DoorParent + ", " + level2DoorParent + ", " + level3DoorParent + ", " + level4DoorParent);
        }
        else {
            Debug.Log("[DoorManager] Door Parents found. Script successfully initialized.");
            // If all parents are found, proceed to set up XR interaction components on the doors.
            SetupXRInteractions();
        }
    }

    // Sets up XRSimpleInteractable components on all doors to enable player interaction.
    private void SetupXRInteractions() {
        // Setup XR interaction for the single door in Level 1.
        SetupSingleDoorInteraction(level1DoorParent, "Level1Door");
        
        // Setup XR interactions for the double doors in Levels 2, 3, and 4.
        SetupDoubleDoorInteraction(level2DoorParent, "Level2");
        SetupDoubleDoorInteraction(level3DoorParent, "Level3");
        SetupDoubleDoorInteraction(level4DoorParent, "Level4");
    }

    // Configures an XRSimpleInteractable for a single door.
    private void SetupSingleDoorInteraction(GameObject doorParent, string doorName) {
        // Find the actual door GameObject within its parent.
        GameObject door = doorParent.transform.Find(doorName).gameObject;
        // Add an XRSimpleInteractable component to the door. This allows interaction without grabbing.
        XRSimpleInteractable interactable = door.AddComponent<XRSimpleInteractable>();
        
        // Add a listener to the selectEntered event of the interactable.
        // This event fires when the player interacts with (selects) the door.
        interactable.selectEntered.AddListener((args) => {
            ToggleSingleDoor(door); // Toggle the door's open/closed state.
            // If the player hasn't entered Level 1 yet, mark it as entered and update the task popup.
            if(!gameManager.enteredLevel1) {
                gameManager.enteredLevel1 = true;
                gameManager.taskPopupManager.setTaskTextByState(TaskPopupManager.TaskPopupState.Level1_Intro);
            }
        });
    }

    // Configures XRSimpleInteractable components for a pair of double doors.
    private void SetupDoubleDoorInteraction(GameObject doorParent, string levelPrefix) {
        // Find the right and left door GameObjects within their parent, using the level prefix for naming convention.
        GameObject rightDoor = doorParent.transform.Find($"{levelPrefix}DoorRight").gameObject;
        GameObject leftDoor = doorParent.transform.Find($"{levelPrefix}DoorLeft").gameObject;
        
        // Add XRSimpleInteractable to the right door.
        XRSimpleInteractable rightInteractable = rightDoor.AddComponent<XRSimpleInteractable>();
        
        // Add XRSimpleInteractable to the left door.
        XRSimpleInteractable leftInteractable = leftDoor.AddComponent<XRSimpleInteractable>();
        
        // Add listeners to the selectEntered event for both doors.
        // Interaction with either door will trigger the ToggleDoubleDoor logic for the pair.
        rightInteractable.selectEntered.AddListener((args) => {
            ToggleDoubleDoor(levelPrefix, rightDoor, leftDoor);
        });
        
        leftInteractable.selectEntered.AddListener((args) => {
            ToggleDoubleDoor(levelPrefix, rightDoor, leftDoor);
        });
    }

    // Toggles the open/closed state of a single door and animates it using LeanTween.
    private void ToggleSingleDoor(GameObject door) {
        level1DoorOpen = !level1DoorOpen; // Invert the open state.
        // Determine the target Y rotation based on the new open state (90 degrees for open, 0 for closed).
        float targetRotation = level1DoorOpen ? 90f : 0f;
        // Animate the door's Y rotation over 1 second with an easeInOutCirc easing function.
        LeanTween.rotateY(door, targetRotation, 1f).setEase(LeanTweenType.easeInOutCirc);
    }

    // Toggles the open/closed state of a pair of double doors and animates them using LeanTween.
    // Also handles game progression logic (checking if previous levels are solved).
    private void ToggleDoubleDoor(string levelPrefix, GameObject rightDoor, GameObject leftDoor) {
        bool currentRightDoorOpenState = false; // Temporary variable to hold the target open state for the right door this action.
        bool currentLeftDoorOpenState = false;  // Temporary variable to hold the target open state for the left door this action.

        // Switch based on the level prefix to handle level-specific logic and state.
        switch (levelPrefix) {
            case "Level2":
                // Check if Level 1 is solved and the survey is correctly answered before allowing Level 2 doors to open.
                if(!gameManager.level1Solved
                    // || !gameManager.correctlyAnsweredLevel1
                    ) {
                    Debug.Log("[DoorManager Stopped Level 2] " + levelPrefix + " CA: " + gameManager.correctlyAnsweredLevel2 + " LS: " + gameManager.level1Solved);
                    // If conditions are not met, do nothing (door remains closed or in its current state).
                    return; // Changed from break to return to prevent animation if conditions fail.
                }
                
                // Invert the open states for Level 2 doors.
                level2DoorRightDoorOpen = !level2DoorRightDoorOpen;
                level2DoorLeftDoorOpen = !level2DoorLeftDoorOpen;
                currentRightDoorOpenState = level2DoorRightDoorOpen;
                currentLeftDoorOpenState = level2DoorLeftDoorOpen;
                // If the player hasn't entered Level 2 yet, mark it as entered and update the task popup.
                if(!gameManager.enteredLevel2) {
                    gameManager.enteredLevel2 = true;
                    gameManager.taskPopupManager.setTaskTextByState(TaskPopupManager.TaskPopupState.Level2_Intro);
                }
                break;
            case "Level3":
                // Check if Level 2 is solved and the survey is correctly answered.
                if(!gameManager.level2Solved
                //    || !gameManager.correctlyAnsweredLevel2
                    ) {
                    Debug.Log("[DoorManager Stopped Level 3] " + levelPrefix + " CA: " + gameManager.correctlyAnsweredLevel2 + " LS: " + gameManager.level2Solved);
                    return; // Changed from break to return.
                }
                Debug.Log("[DoorManager Allowed Level 3] " + levelPrefix + " CA: " + gameManager.correctlyAnsweredLevel2 + " LS: " + gameManager.level2Solved);
                // Invert the open states for Level 3 doors.
                level3DoorRightDoorOpen = !level3DoorRightDoorOpen;
                level3DoorLeftDoorOpen = !level3DoorLeftDoorOpen;
                currentRightDoorOpenState = level3DoorRightDoorOpen;
                currentLeftDoorOpenState = level3DoorLeftDoorOpen;
                // If the player hasn't entered Level 3 yet, mark it as entered and update the task popup.
                if(!gameManager.enteredLevel3) {
                    gameManager.enteredLevel3 = true;
                    gameManager.taskPopupManager.setTaskTextByState(TaskPopupManager.TaskPopupState.Level3_Intro);
                }
                break;
            case "Level4":
                // Check if Level 3 is solved and the survey is correctly answered.
                if(!gameManager.level3Solved
                    // || !gameManager.correctlyAnsweredLevel3
                    ) {
                    return; // Changed from break to return.
                }
                // Invert the open states for Level 4 doors.
                level4DoorRightDoorOpen = !level4DoorRightDoorOpen;
                level4DoorLeftDoorOpen = !level4DoorLeftDoorOpen;
                currentRightDoorOpenState = level4DoorRightDoorOpen;
                currentLeftDoorOpenState = level4DoorLeftDoorOpen;
                // If the player hasn't entered Level 4 yet, mark it as entered and update the task popup.
                if(!gameManager.enteredLevel4) {
                    gameManager.enteredLevel4 = true;
                    gameManager.taskPopupManager.setTaskTextByState(TaskPopupManager.TaskPopupState.Level4_Intro);
                }
                break;
            default:
                // If the levelPrefix is unknown, do nothing.
                return;
        }

        // Determine target local Y rotations for right and left doors (e.g., 90 for open right, -90 for open left).
        float targetRightDoorRotation = currentRightDoorOpenState ? 90f : 0f;
        float targetLeftDoorRotation = currentLeftDoorOpenState ? -90f : 0f;

        // Animate the local Y rotation of both doors over 1 second with an easeInOutSine easing function.
        LeanTween.rotateLocal(rightDoor, new Vector3(0, targetRightDoorRotation, 0), 1f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.rotateLocal(leftDoor, new Vector3(0, targetLeftDoorRotation, 0), 1f).setEase(LeanTweenType.easeInOutSine);
    }

    // Legacy method that appears to be an older way of handling door interactions.
    // It is currently commented out and not used in the active game logic.
    // This method might have used raycasting or player proximity to determine which door to interact with.
    // public void toggleDoorOpenClose() {
    //     // Get player camera and position
    //     Camera playerCamera = Camera.main;
    //     Vector3 playerPosition = playerCamera.transform.position;

    //     // Check each door parent to find which one player is looking at
    //     GameObject[] doorParents = { level1DoorParent, level2DoorParent, level3DoorParent, level4DoorParent };
    //     GameObject targetDoorParent = null;
    //     float closestValidDistance = float.MaxValue;

    //     foreach (GameObject doorParent in doorParents) {
    //         Vector3 doorParentPos = doorParent.transform.position;
    //         Vector3 directionToDoor = (doorParentPos - playerPosition).normalized;
    //         float dotProduct = Vector3.Dot(playerCamera.transform.forward, directionToDoor);
    //         float distance = Vector3.Distance(playerPosition, doorParentPos);

    //         if (dotProduct > 0.7f && distance < 5f && distance < closestValidDistance) {
    //             targetDoorParent = doorParent;
    //             closestValidDistance = distance;
    //         }
    //     }

    //     if (targetDoorParent == null) {
    //         return; // No door in view
    //     }

    //     // Determine if single or double door based on parent name
    //     if (targetDoorParent.name == "Level1RoomDoorParent") {
    //         Debug.Log("[DoorManager] Level 1 door parent found. Setting task text to Level 1 Intro.");
    //         // Handle single door
    //         GameObject door = targetDoorParent.transform.Find("Level1Door").gameObject;
    //         Debug.Log("[DoorManager] Level 1 door found. Toggling door.");
    //         ToggleSingleDoor(door);
    //         Debug.Log("[DoorManager] Toggled door. Setting task text to Level 1 Intro.");
    //         taskPopupManager.setTaskTextByState(TaskPopupManager.TaskPopupState.Level1_Intro);
    //         Debug.Log("[DoorManager] Task text set to Level 1 Intro.");
    //     } else {
    //         // Handle double doors
    //         string levelPrefix = targetDoorParent.name.Substring(0, 6); // Gets "Level#"
    //         GameObject rightDoor = targetDoorParent.transform.Find($"{levelPrefix}DoorRight").gameObject;
    //         GameObject leftDoor = targetDoorParent.transform.Find($"{levelPrefix}DoorLeft").gameObject;
            
    //         ToggleDoubleDoor(levelPrefix, rightDoor, leftDoor);
    //     }
    // }
}