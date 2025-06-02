using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Linq; // Used for .Select() in dictionary to list conversion for JSON serialization.

// Represents data for a survey at a specific level, primarily tracking wrong answers.
// This class is marked as Serializable for potential use with Unity's JSON utility or Inspector.
[System.Serializable]
public class LevelSurveyData {
    // List to store the text of wrong answers selected by the user for this level's survey.
    public List<string> selectedWrongAnswers = new List<string>();

    // Calculates the number of attempts based on the count of wrong answers recorded.
    // This implies each wrong answer recorded counts as an attempt leading to that wrong answer.
    public int getAttemptsTookToSolve() {
        return selectedWrongAnswers.Count;
    }
}

// A struct to facilitate serialization of a dictionary entry (string key, LevelSurveyData value).
// Unity's JsonUtility does not directly serialize dictionaries, so this intermediate structure is used.
[System.Serializable]
public struct SerializableLevelEntry {
    public string levelName; // The key of the dictionary entry (e.g., "Level1").
    public LevelSurveyData surveyData; // The value of the dictionary entry.
}

// A class to hold a list of SerializableLevelEntry, effectively making a dictionary serializable.
// This is the top-level object that will be converted to/from JSON.
[System.Serializable]
public class SerializableLevelsData {
    // List of serializable dictionary entries.
    public List<SerializableLevelEntry> levelEntries = new List<SerializableLevelEntry>();
}

// Manages user data, specifically focusing on survey attempts for different levels.
// It handles saving this data to a JSON file.
// This class is marked as Serializable, though it's primarily a manager class and not directly serialized itself as a whole usually.
[System.Serializable]
public class UserDataManager {

    // Dictionary to store LevelSurveyData for each level, keyed by level name (e.g., "Level1").
    // This is the primary in-memory storage for user survey data.
    private Dictionary<string, LevelSurveyData> levels = new Dictionary<string, LevelSurveyData>();

    // Path to the JSON file where user data will be stored.
    // Note: "CaSe SeNsItIvE" comment suggests potential issues on some platforms if path casing is inconsistent.
    private string userDataFilePath = Directory.GetCurrentDirectory() + "/Assets/local-game-data/user-data/user-data.json"; 

    // Constructor for UserDataManager.
    public UserDataManager() {
        // Initialize the dictionary with entries for Level1, Level2, and Level3.
        // This ensures that data structures are ready even if the user hasn't interacted with these levels yet.
        levels.Add("Level1", new LevelSurveyData());
        levels.Add("Level2", new LevelSurveyData());
        levels.Add("Level3", new LevelSurveyData());
        //Debug.Log($"User Data File Path: {userDataFilePath}");

        // Check if the user data file exists at the specified path upon initialization.
        // This is a check for existence, not for loading data from it yet.
        if (!File.Exists(userDataFilePath)) {
            Debug.LogError($"User Data File does not exist at path: {userDataFilePath}");
            // If the file doesn't exist, further operations that rely on it might fail or create it.
            // Currently, it just logs an error and returns, which might be fine if the file is created on first save.
            return;
        }

        Debug.Log($"Found User Data File at path: {userDataFilePath}");

    }

    // Writes the provided JSON string to the user data file.
    private void writeJsonToUserDataFile(string json) {
        // Ensure the directory for the user data file exists before attempting to write the file.
        string directoryPath = Path.GetDirectoryName(userDataFilePath);
        if (!Directory.Exists(directoryPath)) {
            Directory.CreateDirectory(directoryPath); // Create the directory if it doesn't exist.
            Debug.Log($"Created directory: {directoryPath}");
        }
        File.WriteAllText(userDataFilePath, json); // Write the JSON string to the file, overwriting if it exists.
        Debug.Log($"User data saved to: {userDataFilePath}");
    }

    // Saves the current user survey data (the `levels` dictionary) to the JSON file.
    public void saveUserData() {
        // Create an instance of SerializableLevelsData to hold the data in a serializable format.
        SerializableLevelsData dataToSave = new SerializableLevelsData();
        // Convert the `levels` dictionary to a list of SerializableLevelEntry objects using LINQ.
        dataToSave.levelEntries = levels.Select(kvp => new SerializableLevelEntry { levelName = kvp.Key, surveyData = kvp.Value }).ToList();

        // Convert the SerializableLevelsData object to a JSON string.
        // The `true` argument enables pretty printing for readability of the JSON file.
        string json = JsonUtility.ToJson(dataToSave, true); 
        // Debug.Log($"Saving JSON: {json}"); // Optional: Log the JSON being saved for debugging.
        
        // Write the generated JSON string to the user data file.
        writeJsonToUserDataFile(json);
    }

    // Adds a record of a wrong answer selected by the user for a specific level's survey.
    public void addWrongAnswerToLevelSurvey(string level, string wrongAnswer) {
        // Check if the specified level exists as a key in the `levels` dictionary.
        if (levels.ContainsKey(level)) {
             // Add the wrong answer to the list of selected wrong answers for that level.
             levels[level].selectedWrongAnswers.Add(wrongAnswer);
             // Immediately save all user data to the JSON file after adding the wrong answer.
             saveUserData();
        } else {
            // Log an error if an attempt is made to add a wrong answer to a non-existent level key.
            Debug.LogError($"Attempted to add wrong answer to non-existent level: {level}");
        }
    }

    // TODO: Implement a corresponding LoadUserData method that converts
    // the JSON back from SerializableLevelsData into the dictionary.
    // This is crucial for data persistence across game sessions.

}