using UnityEngine;
using System.Collections.Generic;

public class IrisDataLoader : MonoBehaviour {
    public TextAsset csvFile;
    public List<IrisData> allIrisData = new List<IrisData>();

    public IrisDataLoader(TextAsset csvFile) {
        this.csvFile = csvFile;
    }

    public List<IrisData> LoadIrisData() {
        Debug.Log("Starting to load Iris data..."); // New debug line

        if (csvFile == null)
        {
            Debug.LogError("CSV file not assigned in the Inspector!");
            return new List<IrisData>();
        }

        Debug.Log($"CSV file name: {csvFile.name}"); // New debug line
        Debug.Log($"First few lines of CSV: {csvFile.text.Substring(0, 200)}"); // New debug line

        string[] lines = csvFile.text.Split('\n');
        Debug.Log($"CSV file contains {lines.Length} lines");

        allIrisData.Clear();
        Debug.Log("line Length "+lines.Length);

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            string[] values = line.Split(',');
            Debug.Log($"Line {i}: {string.Join(", ", values)}");

            Debug.Log(values.Length);

            if (values.Length == 6)
            {
                try
                {
                    IrisData newData = new IrisData
                    {
                        sepalLength = float.Parse(values[1]),
                        sepalWidth = float.Parse(values[2]),
                        petalLength = float.Parse(values[3]),
                        petalWidth = float.Parse(values[4]),
                        species = values[5].Trim()
                    };
                    allIrisData.Add(newData);
                    Debug.Log($"Added data point: {newData}");
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Error parsing line {i}: {e.Message}");
                }
            }
            else
            {
                Debug.LogWarning($"Skipping line {i}: incorrect number of values");
            }
        }
        
        Debug.Log($"Total data points loaded: {allIrisData.Count}"); // New debug line
        
        if (allIrisData.Count > 0)
        {
            Debug.Log($"First data point: {allIrisData[0]}");
            Debug.Log($"Last data point: {allIrisData[allIrisData.Count - 1]}");
        }
        else
        {
            Debug.LogError("No data points were loaded!");
        }
        return allIrisData;
    }

    public bool IsDataLoaded(){
        return allIrisData != null && allIrisData.Count > 0;
    }

    public int GetDataCount(){
        return allIrisData?.Count ?? 0;
    }
}