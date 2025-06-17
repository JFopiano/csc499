using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class GetText : MonoBehaviour
{
    public Transform contentWindow;

    public GameObject recallTextObject;

    void Start()
    {
        //get the file
        string filePath = Application.streamingAssetsPath + "/data.csv";
        if (!File.Exists(filePath))
        {
            Debug.LogError("CSV file not found at: " + filePath);
            return;
        }

        List<string> fileLines = File.ReadAllLines(filePath).ToList();

        foreach (string line in fileLines)
        {
            // Debug.Log(line);
            recallTextObject.GetComponent<Text>().text = line;
            Instantiate(recallTextObject, contentWindow);
        }
    }
}
