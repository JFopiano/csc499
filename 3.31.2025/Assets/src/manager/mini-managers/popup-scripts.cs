// using UnityEngine;
// using UnityEngine.UI;

// public class TaskPopupScript {
//     public enum TaskPopupState {
//         START,
//         HALLWAY,
//         Level1_Intro,
//         Level1_Complete,
//         Level2_Intro,
//         Level2_Complete,
//         Level3_Intro,
//         Level3_Complete,
//         Level4_Intro,
//         Level4_Complete,
//         FINISH
//     }

//     public TaskPopupState taskPopupState;
//     public GameObject taskPopupPanelReference;
//     public GameObject taskTextReference;

//     public void TaskPopupManager(GameObject taskPopupPanelReference, GameObject taskTextReference) {
//         this.taskPopupPanelReference = taskPopupPanelReference;
//         this.taskTextReference = taskTextReference;
//     }

//     public void setTaskText(string task) {
//         taskTextReference.GetComponent<Text>().text = task;
//     }

//     public void showTaskPopup() {
//         taskPopupPanelReference.SetActive(true);
//     }

//     public void hideTaskPopup() {
//         taskPopupPanelReference.SetActive(false);
//     }

//     public string getTaskTextByState(TaskPopupState taskPopupState) {
//         switch(taskPopupState) {
//             case TaskPopupState.START:
//                 return "Welcome to the Machine Learning Academy!\n\n" +
//                        "Today you'll learn about Support Vector Machines (SVMs),\n" +
//                        "one of the most powerful classification techniques in machine learning.\n\n" +
//                        "Press the START button to begin your journey.";
                
//             case TaskPopupState.HALLWAY:
//                 return "You're in the main hallway of the Machine Learning Academy.\n\n" +
//                        "Turn right and enter Classroom 1 to begin your SVM training.\n" +
//                        "Follow the signs to find your way.";
                
//             case TaskPopupState.Level1_Intro:
//                 return "CLASSROOM 1: INTRODUCTION TO SVMs\n\n" +
//                        "Support Vector Machines (SVMs) are powerful classifiers used in machine learning.\n\n" +
//                        "An SVM works by finding the optimal dividing line (or hyperplane) that\n" +
//                        "separates different classes of data with the maximum margin.\n\n" +
//                        "The 'support vectors' are the data points closest to this dividing line.";
                
//             case TaskPopupState.Level1_Complete:
//                 return "Excellent work! You've successfully separated the data.\n\n" +
//                        "You've created your first linear SVM classifier!\n\n" +
//                        "In this perfect scenario, the data was cleanly separable with a straight line.\n" +
//                        "But real-world data is rarely this perfect...\n\n" +
//                        "Proceed to Classroom 2 to tackle a more challenging scenario.";
                
//             case TaskPopupState.Level2_Intro:
//                 return "CLASSROOM 2: DEALING WITH OUTLIERS\n\n" +
//                        "Real-world data often contains outliers - data points that don't\n" +
//                        "follow the general pattern of their class.\n\n" +
//                        "SVMs handle outliers through a concept called 'soft margin' classification,\n" +
//                        "which allows some misclassification to achieve better overall results.";
                
//             case TaskPopupState.Level2_Complete:
//                 return "Well done! You've positioned the boundary optimally despite the outlier.\n\n" +
//                        "This demonstrates how SVMs use 'soft margins' in practice - allowing some\n" +
//                        "misclassification to achieve a better overall model.\n\n" +
//                        "But what if data can't be separated with a straight line at all?\n" +
//                        "Continue to Classroom 3 to find out!";
                
//             case TaskPopupState.Level3_Intro:
//                 return "CLASSROOM 3: NON-LINEAR CLASSIFICATION\n\n" +
//                        "Sometimes data cannot be separated with a straight line in its\n" +
//                        "original dimensions. SVMs solve this using a technique called\n" +
//                        "the 'kernel trick'.\n\n" +
//                        "The kernel trick transforms data into higher dimensions where\n" +
//                        "it becomes linearly separable. For example, 2D data might become\n" +
//                        "separable when transformed into 3D space.";
                
//             case TaskPopupState.Level3_Complete:
//                 return "Fantastic! You've mastered non-linear classification.\n\n" +
//                        "By pushing the data into 3D and finding the right plane orientation,\n" +
//                        "you've demonstrated the kernel trick that makes SVMs so powerful.\n\n" +
//                        "In real SVMs, this transformation happens mathematically, allowing\n" +
//                        "separation in higher dimensions that would be impossible in the original space.\n\n" +
//                        "Now, let's apply what you've learned to real-world data in Classroom 4!";
                
//             case TaskPopupState.Level4_Intro:
//                 return "CLASSROOM 4: REAL DATASET CLASSIFICATION\n\n" +
//                        "The famous Iris dataset contains measurements from three species of iris flowers.\n" +
//                        "Each data point has four features:\n" +
//                        "• Sepal Length\n" +
//                        "• Sepal Width\n" +
//                        "• Petal Length\n" +
//                        "• Petal Width\n\n" +
//                        "These measurements can be used to classify the species: setosa, versicolor, and virginica.\n" +
//                        "This is a classic multi-class classification problem in machine learning.";
                
//             case TaskPopupState.Level4_Complete:
//                 return "Congratulations! You've completed the SVM training course.\n\n" +
//                        "You've learned how SVMs work, from basic linear separation to\n" +
//                        "handling outliers and non-linear data, and finally applying these\n" +
//                        "concepts to a real-world dataset.\n\n" +
//                        "Key takeaways:\n" +
//                        "• SVMs find optimal boundaries with maximum margins\n" +
//                        "• Soft margins handle imperfect, real-world data\n" +
//                        "• The kernel trick enables non-linear classification\n" +
//                        "• Feature selection impacts classification performance\n\n" +
//                        "Feel free to continue exploring the Iris dataset in this room\n" +
//                        "or return to previous classrooms to reinforce what you've learned!";
                
//             case TaskPopupState.FINISH:
//                 return "Thank you for completing the Machine Learning Academy's\n" +
//                        "SVM training course!\n\n" +
//                        "You now understand the fundamental concepts behind one of\n" +
//                        "machine learning's most powerful classification algorithms.\n\n" +
//                        "We hope you enjoyed this interactive learning experience!";
                
//             default:
//                 return "";
//         }
//     }

//     private void animateScaleUp() {
//         taskPopupPanelReference.transform.localScale = new Vector3(0, 0, 0);
//         showTaskPopup();
//         LeanTween.scale(taskPopupPanelReference, new Vector3(0.25f, 0.25f, 0.25f), 0.5f).setEase(LeanTweenType.easeInOutCirc);
//     }

//     public void updateTaskByState(TaskPopupState taskPopupState) {
//         animateScaleUp();
//         setTaskText(getTaskTextByState(taskPopupState));
//     }
// }