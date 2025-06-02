// using UnityEngine;
// using UnityEngine.UI;

// // This script should be attached to trigger zones in different areas of the school
// public class TaskPopupHandler : MonoBehaviour
// {
//     [Header("Task Configuration")]
//     [SerializeField] private TaskPopupManager.TaskPopupState popupState;
//     [SerializeField] private bool showOnlyOnce = true;
//     [SerializeField] private float autoHideDelay = 0f; // 0 means don't auto-hide

//     [Header("Complete Trigger")]
//     [SerializeField] private bool hasCompletionTrigger = false;
//     [SerializeField] private TaskPopupManager.TaskPopupState completionState;
//     [SerializeField] private string completionTag = "CompletionTrigger";

//     private GameManager gameManager;
//     private bool hasTriggered = false;

//     void Start()
//     {
//         gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
//         if (gameManager == null)
//         {
//             Debug.LogError("Cannot find GameManager. Make sure it exists in the scene.");
//         }
//     }

//     private void OnTriggerEnter(Collider other)
//     {
//         // Only trigger for the player
//         if (other.CompareTag("Player") && (!showOnlyOnce || !hasTriggered))
//         {
//             hasTriggered = true;
            
//             // Show the appropriate popup
//             gameManager.taskPopupManager.updateTaskByState(popupState);
            
//             // Auto-hide after delay if set
//             if (autoHideDelay > 0)
//             {
//                 Invoke("HidePopup", autoHideDelay);
//             }
//         }
        
//         // Check for completion trigger
//         if (hasCompletionTrigger && other.CompareTag(completionTag))
//         {
//             gameManager.taskPopupManager.updateTaskByState(completionState);
//         }
//     }
//     private void HidePopup()
//     {
//         gameManager.taskPopupManager.hideTaskPopup();
//     }
// }