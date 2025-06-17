using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI; // Needed for Text
// using TMPro; // If using TextMeshPro

public class CSVLoader : MonoBehaviour
{
    public Text scrollText; // Assign this in the inspector
    // public TMP_Text scrollText; // Use this if you're using TextMeshPro

    void Start()
    {
        LoadCSVToText("data"); // Pass the filename without the .csv extension
    }

    void LoadCSVToText(string fileName)
    {
        TextAsset csvData = Resources.Load<TextAsset>(fileName);

        if (csvData != null)
        {
            Debug.Log("CSV file loaded successfully!");
            scrollText.text = csvData.text;
        }
        else
        {
            Debug.LogWarning("CSV file not found!");
            scrollText.text = "CSV file not found!";
        }
    }
}

