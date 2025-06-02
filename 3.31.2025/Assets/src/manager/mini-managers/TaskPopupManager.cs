using UnityEngine;
using UnityEngine.UI;

// Manages the display of task-related information to the player via a UI hologram.
// This class is a MonoBehaviour, suggesting it might be attached to a GameObject in the scene,
// however, its constructor logic (GameObject.Find) is more typical of a non-MonoBehaviour manager
// that is instantiated by another script (like GameManager).
// If it IS a MonoBehaviour attached to an object, the constructor will not be called automatically by Unity;
// Awake() or Start() would be used for initialization. If instantiated via `new TaskPopupManager()`, then it behaves as a plain C# class.
public class TaskPopupManager : MonoBehaviour {

    // Enum defining the different states or messages the task popup can display.
    // These correspond to various stages and instructions in the game.
    public enum TaskPopupState {
        START,          // Initial message when the game begins.
        HALLWAY,        // Instructions for navigating the hallway.
        Level1_Intro,   // Introduction to Level 1.
        Level1_Complete,// Message upon completing Level 1.
        Level2_Intro,   // Introduction to Level 2.
        Level2_Complete,// Message upon completing Level 2.
        Level3_Intro,   // Introduction to Level 3.
        Level3_Complete,// Message upon completing Level 3.
        Level4_Intro,   // Introduction to Level 4.
        Level4_Complete,// Message upon completing Level 4.
        FINISH          // Final message upon completing the game/tutorial.
    }

    // Current state of the task popup, determining which message is shown.
    // Public, so it can be inspected or set externally, though primarily managed by setTaskTextByState.
    public TaskPopupState taskPopupState;

    // Getter for the current taskPopupState.
    public TaskPopupState getTaskPopupState() {
        return taskPopupState;
    }

    // Reference to the main GameObject of the task hologram UI.
    public GameObject taskHologram;
    // Reference to the child GameObject containing the Text component for displaying messages.
    public GameObject taskText;

    // Constructor for TaskPopupManager.
    // Finds the necessary GameObjects in the scene by name.
    // This approach can be fragile if names change or objects are not present at instantiation.
    // If this class were a MonoBehaviour consistently attached to a scene object, serialized fields would be more robust.
    public TaskPopupManager() {
        taskHologram = GameObject.Find("PlayerTaskHologram");
        if (taskHologram != null) {
            // Find the Text GameObject, assuming a specific child hierarchy: "Scroll UI Sample/Text".
            Transform textTransform = taskHologram.transform.Find("Scroll UI Sample/Text");
            if (textTransform != null) {
                taskText = textTransform.gameObject;
            } else {
                Debug.LogError("[TaskPopupManager] Could not find child 'Scroll UI Sample/Text' under 'PlayerTaskHologram'.");
            }
        } else {
            Debug.LogError("[TaskPopupManager] Could not find GameObject named 'PlayerTaskHologram'. Task popup will not function.");
        }
    }

    // Sets the text of the task popup based on the provided TaskPopupState.
    public void setTaskTextByState(TaskPopupState newState) {
        this.taskPopupState = newState; // Update the current state.
        string message = getTaskTextByState(newState); // Get the corresponding message string.
        updateTaskText(message); // Update the UI Text component.
    }

    // Returns the instructional or informational string associated with a given TaskPopupState.
    public string getTaskTextByState(TaskPopupState stateToGetTextFor) {
        switch(stateToGetTextFor) {
            case TaskPopupState.START:
                return "Welcome to the Machine Learning Academy!\n\n" +
                       "Today you'll learn about Support Vector Machines (SVMs),\n" +
                       "one of the most powerful classification techniques in machine learning.\n\n" +
                       "Press the START button to begin your journey.\n\nPress Y (left controller) to turn this on/off";
                
            case TaskPopupState.HALLWAY:
                return "You're in the main hallway of the Machine Learning Academy.\n\n" +
                       "Turn right and enter Classroom 1 to begin your SVM training.\n" +
                       "Follow the signs to find your way.\n\nPress Y (left controller) to turn this on/off";
                
            case TaskPopupState.Level1_Intro:
                return "CLASSROOM 1: INTRODUCTION TO SVMs\n\n" +
                       "Support Vector Machines (SVMs) are powerful classifiers used in machine learning.\n\n" +
                       "An SVM works by finding the optimal dividing line (or hyperplane) that\n" +
                       "separates different classes of data with the maximum margin.\n\n" +
                       "The 'support vectors' are the data points closest to this dividing line.\n\nPress Y (left controller) to turn this on/off";
                
            case TaskPopupState.Level1_Complete:
                return "Excellent work! You've successfully separated the data.\n\n" +
                       "You've created your first linear SVM classifier!\n\n" +
                       "In this perfect scenario, the data was cleanly separable with a straight line.\n" +
                       "But real-world data is rarely this perfect...\n\n" +
                       "Proceed to Classroom 2 to tackle a more challenging scenario.\n\nPress Y (left controller) to turn this on/off";
                
            case TaskPopupState.Level2_Intro:
                return "CLASSROOM 2: DEALING WITH OUTLIERS\n\n" +
                       "Real-world data often contains outliers - data points that don't\n" +
                       "follow the general pattern of their class.\n\n" +
                       "SVMs handle outliers through a concept called 'soft margin' classification,\n" +
                       "which allows some misclassification to achieve better overall results.\n\nPress Y (left controller) to turn this on/off";
                
            case TaskPopupState.Level2_Complete:
                return "Well done! You've positioned the boundary optimally despite the outlier.\n\n" +
                       "This demonstrates how SVMs use 'soft margins' in practice - allowing some\n" +
                       "misclassification to achieve a better overall model.\n\n" +
                       "But what if data can't be separated with a straight line at all?\n" +
                       "Continue to Classroom 3 to find out!\n\nPress Y (left controller) to turn this on/off";
                
            case TaskPopupState.Level3_Intro:
                return "CLASSROOM 3: NON-LINEAR CLASSIFICATION\n\n" +
                       "Sometimes data cannot be separated with a straight line in its\n" +
                       "original dimensions. SVMs solve this using a technique called\n" +
                       "the 'kernel trick'.\n\n" +
                       "The kernel trick transforms data into higher dimensions where\n" +
                       "it becomes linearly separable. For example, 2D data might become\n" +
                       "separable when transformed into 3D space.\n\nPress Y (left controller) to turn this on/off";
                
            case TaskPopupState.Level3_Complete:
                return "Fantastic! You've mastered non-linear classification.\n\n" +
                       "By pushing the data into 3D and finding the right plane orientation,\n" +
                       "you've demonstrated the kernel trick that makes SVMs so powerful.\n\n" +
                       "In real SVMs, this transformation happens mathematically, allowing\n" +
                       "separation in higher dimensions that would be impossible in the original space.\n\n" +
                       "Now, let's apply what you've learned to real-world data in Classroom 4!\n\nPress Y (left controller) to turn this on/off";
                
            case TaskPopupState.Level4_Intro:
                return "CLASSROOM 4: REAL DATASET CLASSIFICATION\n\n" +
                       "The famous Iris dataset contains measurements from three species of iris flowers.\n" +
                       "Each data point has four features:\n" +
                       "• Sepal Length\n" +
                       "• Sepal Width\n" +
                       "• Petal Length\n" +
                       "• Petal Width\n\n" +
                       "These measurements can be used to classify the species: setosa, versicolor, and virginica.\n" +
                       "This is a classic multi-class classification problem in machine learning.\n\nPress Y (left controller) to turn this on/off";
                
            case TaskPopupState.Level4_Complete:
                return "Congratulations! You've completed the SVM training course.\n\n" +
                       "You've learned how SVMs work, from basic linear separation to\n" +
                       "handling outliers and non-linear data, and finally applying these\n" +
                       "concepts to a real-world dataset.\n\n" +
                       "Key takeaways:\n" +
                       "• SVMs find optimal boundaries with maximum margins\n" +
                       "• Soft margins handle imperfect, real-world data\n" +
                       "• The kernel trick enables non-linear classification\n" +
                       "• Feature selection impacts classification performance\n\n" +
                       "Feel free to continue exploring the Iris dataset in this room\n" +
                       "or return to previous classrooms to reinforce what you've learned!\n\nPress Y (left controller) to turn this on/off";
                
            case TaskPopupState.FINISH:
                return "Thank you for completing the Machine Learning Academy's\n" +
                       "SVM training course!\n\n" +
                       "You now understand the fundamental concepts behind one of\n" +
                       "machine learning's most powerful classification algorithms.\n\n" +
                       "We hope you enjoyed this interactive learning experience!\n\nPress Y (left controller) to turn this on/off";
                
            default:
                // Return an empty string if the state is not recognized.
                return "";
        }
    }

    // Updates the Text component of the task display with the provided string.
    public void updateTaskText(string textToShow) {
        if (taskText != null) {
            Text uiTextComponent = taskText.GetComponent<Text>();
            if (uiTextComponent != null) {
                uiTextComponent.text = textToShow;
            } else {
                Debug.LogError("[TaskPopupManager] taskText GameObject does not have a Text component.");
            }
        } else {
            // This error would likely have been caught in the constructor if taskText wasn't found.
            Debug.LogError("[TaskPopupManager] taskText GameObject reference is null.");
        }
    }

    // Activates the task hologram GameObject, making it visible.
    private void ShowTaskPopup() {
        if (taskHologram != null) {
            taskHologram.SetActive(true);   
        } else {
            Debug.LogError("[TaskPopupManager] taskHologram GameObject reference is null. Cannot show popup.");
        }
    }

    // Deactivates the task hologram GameObject, making it hidden.
    private void HideTaskPopup() {
        if (taskHologram != null) {
            taskHologram.SetActive(false);
        } else {
            Debug.LogError("[TaskPopupManager] taskHologram GameObject reference is null. Cannot hide popup.");
        }
    }

    // Toggles the visibility of the task hologram.
    // If it's active, it will be hidden. If it's inactive, it will be shown.
    public void ToggleTaskPopup() {
        if (taskHologram != null) {
            if(taskHologram.activeSelf) {
                HideTaskPopup();
            }
            else {
                ShowTaskPopup();
            }
        } else {
            Debug.LogError("[TaskPopupManager] taskHologram GameObject reference is null. Cannot toggle popup.");
        }
    }
}
