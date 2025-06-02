using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO; // Added for File operations (though not directly used in this script for file reading/writing)
using System; // Added for Exception handling (though no explicit try-catch blocks are present)

// Helper class to structure data for a single survey question.
// This class is marked as Serializable to potentially be used with Unity's JSON utility or Inspector.
[System.Serializable]
public class SurveyQuestionData
{
    public string question; // The text of the survey question.
    public List<string> answers; // A list of possible answer texts.
    public int correctAnswerIndex; // The 0-based index of the correct answer in the `answers` list.
}

// Helper class to hold a collection of survey questions, organized by level.
// This class is marked as Serializable.
[System.Serializable]
public class SurveyQuestions
{
    // A Dictionary to store SurveyQuestionData for each level, keyed by a string (e.g., "Level1").
    public Dictionary<string, SurveyQuestionData> Levels = new Dictionary<string, SurveyQuestionData>();

    // Optional: An indexer to allow dictionary-like access to the Levels data (e.g., surveyQuestions["Level1"]).
    // Provides a convenient way to get or set survey data for a specific level.
    public SurveyQuestionData this[string levelKey]
    {
        get => Levels.ContainsKey(levelKey) ? Levels[levelKey] : null; // Returns null if the key doesn't exist.
        set => Levels[levelKey] = value;
    }
}

// surveyManager class handles the logic for in-game surveys, including displaying questions,
// processing answers, and interacting with the GameManager and UserDataManager.
public class surveyManager : MonoBehaviour
{
    // Reference to the GameManager to access game state information.
    private GameManager gameManager;

    // Hardcoded survey data. This structure holds all questions and answers for different levels.
    // TODO: Consider loading this data from an external file (e.g., JSON) for easier modification.
    private SurveyQuestions surveyData = new SurveyQuestions
    {
        Levels = new Dictionary<string, SurveyQuestionData>
        {
            {
                "Level1", new SurveyQuestionData
                {
                    question = "How many supporting vectors are there after the SVM solved this level?",
                    answers = new List<string> { "2", "6", "4" },
                    correctAnswerIndex = 2
                }
            },
            {
                "Level2", new SurveyQuestionData
                {
                    question = "What is the difference between this level and the previous level",
                    answers = new List<string> { "There are no outliers", "There is a red outlier", "There is a green outlier" },
                    correctAnswerIndex = 2
                }
            },
            {
                "Level3", new SurveyQuestionData
                {
                    question = "What is the purpose of the green margin",
                    answers = new List<string> { "There is no purpose", "To minimize the width of the margin", "To maximize the width of the margin" },
                    correctAnswerIndex = 2
                }
            }
            // Add Level4 here if needed
            // {
            //     "Level4", new SurveyQuestionData { /* ... data ... */ } 
            // }
        }
    };

    // Serialized field for a UI Button. This might be a reference to the button component
    // on the GameObject this script is attached to, or a specific button related to surveys.
    [SerializeField]
    private Button uiButton;

    // Serialized field for the GameObject representing the survey UI panel for a level.
    [SerializeField]
    private GameObject levelSurvey;    
    // This is a property that always returns the current count (Comment seems outdated or refers to a removed property)

    

    // Start is called before the first frame update by Unity.
    void Start() {
        // Prevent it from creating multiple copies since this is attached to many buttons (This comment suggests the script might be on multiple GameObjects, which could be problematic if not handled carefully, e.g. for singleton-like behavior or event registration)
        // Get a reference to the GameManager instance in the scene.
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        // Example: Access a vector from the GameManager script (This is a non-functional example comment)

        // Get the Button component attached to the same GameObject as this script.
        uiButton = GetComponent<Button>();

        // If a Button component is found, subscribe the OnUIButtonClicked method to its onClick event.
        // This is a common pattern for handling button presses without manually assigning listeners in the Inspector.
        if (uiButton != null) {
            uiButton.onClick.AddListener(OnUIButtonClicked);
            //Debug.Log("UI Button events attached to " + gameObject.name);
        }

        // Initialize the survey holograms with questions and answers.
        initSurveyHolograms();
    }

    // This function sets up the text for questions and answers on the survey UI elements (holograms).
    // It assumes GameObjects with specific names (e.g., "Level1QuestionText", "Level1answerA") exist in the scene.
    // The comment about "json file (src/Data/survey_questions.json)" is aspirational, as data is currently hardcoded.
    void initSurveyHolograms() {
        List<string> surveyHologramPrefixes = new List<string>();
        surveyHologramPrefixes.Add("Level1");
        surveyHologramPrefixes.Add("Level2");
        surveyHologramPrefixes.Add("Level3");
        surveyHologramPrefixes.Add("Level4"); // Includes Level4, though no hardcoded data for it exists yet.

        foreach(string hologramPrefix in surveyHologramPrefixes) {
            // Find the Text component for the question.
            Text questionText = GameObject.Find($"{hologramPrefix}QuestionText").GetComponent<Text>();
            if(questionText == null) {
                Debug.LogError($"[surveyManager] Could not find question text for {hologramPrefix}");
                continue; // Skip to the next level if the question text UI is not found.
            }
           // Debug.Log($"[surveyManager] Found question text for {hologramPrefix}: {questionText.text}");
            
            // Try to get the survey data for the current level prefix from the hardcoded surveyData.
            if (surveyData.Levels.TryGetValue(hologramPrefix, out SurveyQuestionData levelData)) {
                questionText.text = levelData.question; // Set the question text.
                
                // TODO: Set up answer texts as well using levelData.answers (This is actually done below, so the TODO might be outdated or refer to a more robust setup).

                // Get the parent Viewport of the question text. This assumes a specific UI hierarchy.
                Transform viewport = questionText.transform.parent;
                
                // Find the Text components for the answers (A, B, C) within the viewport.
                // This relies on a specific naming convention and hierarchy for answer GameObjects.
                Text answerA = viewport.Find("answerA/Image/Text").GetComponent<Text>();
                Text answerB = viewport.Find("answerB/Image/Text").GetComponent<Text>();
                Text answerC = viewport.Find("answerC/Image/Text").GetComponent<Text>();

                // Check if all answer Text components were found.
                if (answerA == null || answerB == null || answerC == null) {
                    Debug.LogError($"[surveyManager] Could not find one or more answer texts for {hologramPrefix}");
                    continue; // Skip if any answer UI is missing.
                }

                // Set the text for each answer option.
                answerA.text = levelData.answers[0];
                answerB.text = levelData.answers[1];
                answerC.text = levelData.answers[2];
            } else {
                Debug.LogWarning($"[surveyManager] No survey data found for key: {hologramPrefix}");
            }
        }
    }

    // This method is called when a UI Button (likely one this script is attached to, or an answer button) is clicked.
    void OnUIButtonClicked() {
        Debug.Log("Button clicked: " + gameObject.name); // Log which button was clicked.
                
        // Check the name of the GameObject this script is attached to (if it's a button itself)
        // or the name of the button that triggered this event (if registered to multiple buttons).
        // This section handles actions for level-specific solver buttons.
        if (gameObject.name == "Level1_Solver_Button") {
            // Adjusts the Y position of the `levelSurvey` GameObject. This might be to show or reposition the survey panel.
            Vector3 newPosition = levelSurvey.transform.localPosition;
            newPosition.y = -0.88f;
            levelSurvey.transform.localPosition = newPosition;
        } else if (gameObject.name == "Level2_Solver_Button") {
            Vector3 newPosition = levelSurvey.transform.localPosition;
            newPosition.y = -0.88f;
            levelSurvey.transform.localPosition = newPosition;
            
        } else if (gameObject.name == "Level3_Solver_Button") {
            Debug.Log("Level 3 Solver button clicked");
            Vector3 newPosition = levelSurvey.transform.localPosition;
            newPosition.y = -0.88f;
            levelSurvey.transform.localPosition = newPosition;
           
        } else if (gameObject.name == "Level4_Solver") { // Note: Mismatch with "Level4_Solver_Button" pattern.
            Vector3 newPosition = levelSurvey.transform.localPosition;
            newPosition.y = -0.88f;
            levelSurvey.transform.localPosition = newPosition;
        
        } 


        // This section processes answers based on the current game state (which level is active and solved).
        // It assumes the `gameObject.name` will be "answerA", "answerB", or "answerC" if an answer button was clicked.
        // The order of these checks is important (Level 4, then 3, then 2, then 1).
        if (gameManager.enteredLevel4 && gameManager.level4Solved) {
            // Logic for Level 4 survey - currently only checks if "answerC" is correct.
            // Assumes Level 4 survey also has "answerC" as the correct one, but no data is defined for it.
            if(gameObject.name == "answerC"){ // Potential issue: Hardcoded correct answer name.
                gameManager.correctlyAnsweredLevel4 = true;
                levelSurvey.GetComponentInChildren<Text>().text = "That is correct!\n Move on to the next level done the hall :)";
            } else if (gameObject.name == "answerA" || gameObject.name == "answerB") {
                 // If Level 4 data was present, this would record a wrong answer.
                // gameManager.userDataManager.addWrongAnswerToLevelSurvey("Level4", gameObject.GetComponentInChildren<Text>().text);
            }
            
        } else if (gameManager.enteredLevel3 && gameManager.level3Solved) {
            // Check if the clicked button corresponds to the correct answer for Level 3 (hardcoded as answerC).
            if(gameObject.name == "answerC"){ // Potential issue: Assumes SurveyData.Levels["Level3"].correctAnswerIndex corresponds to "answerC".
                gameManager.correctlyAnsweredLevel3 = true;
                // Update the survey panel text to give positive feedback.
                levelSurvey.GetComponentInChildren<Text>().text = "That is correct!\n Move on to the next level done the hall :)";
            }
            // If an incorrect answer button was clicked for Level 3.
            else if (gameObject.name == "answerA" || gameObject.name == "answerB") {
                // Record the wrong answer using the UserDataManager.
                gameManager.userDataManager.addWrongAnswerToLevelSurvey("Level3", gameObject.GetComponentInChildren<Text>().text);
            }
        } else if (gameManager.enteredLevel2 && gameManager.level2Solved) {
            // Check if the clicked button corresponds to the correct answer for Level 2 (hardcoded as answerC).
              if(gameObject.name == "answerC") { // Potential issue: Assumes SurveyData.Levels["Level2"].correctAnswerIndex corresponds to "answerC".
                gameManager.correctlyAnsweredLevel2 = true;
                Debug.Log($"[surveyManager] Set correctlyAnsweredLevel2 to {gameManager.correctlyAnsweredLevel2} for Level 2.");
                levelSurvey.GetComponentInChildren<Text>().text = "That is correct!\n Move on to the next level done the hall :)";
            }
            else if (gameObject.name == "answerA" || gameObject.name == "answerB") {
                gameManager.userDataManager.addWrongAnswerToLevelSurvey("Level2", gameObject.GetComponentInChildren<Text>().text);
            }
        } else if (gameManager.enteredLevel1 && gameManager.level1Solved) {
            // Check if the clicked button corresponds to the correct answer for Level 1 (hardcoded as answerC).
            if(gameObject.name == "answerC"){ // Potential issue: Assumes SurveyData.Levels["Level1"].correctAnswerIndex corresponds to "answerC".
                gameManager.correctlyAnsweredLevel1 = true;
                levelSurvey.GetComponentInChildren<Text>().text = "That is correct!\n Move on to the next level done the hall :)";
            }
            else if (gameObject.name == "answerA" || gameObject.name == "answerB") {
                gameManager.userDataManager.addWrongAnswerToLevelSurvey("Level1", gameObject.GetComponentInChildren<Text>().text);
            }
        } else {
            // This case is reached if no level is currently active for survey checking, or if all are done.
            Debug.Log("All levels completed or invalid state for survey checking based on button name: " + gameObject.name);
        }
    }
}
