using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ColumnData
{
    public string columnName;
    public Sprite[] graphImages = new Sprite[3]; // Three graphs per column
    public int currentGraphIndex = 0; // Which graph is showing
}

public class DataStructure : MonoBehaviour
{
    public List<ColumnData> columns = new List<ColumnData>(11);
    public GameObject columnPrefab;
    public Transform spawnParent; // Where to place the 11 graph panels
    // Start is called before the first frame update
    void Start()
    {
        SpawnAllColumns();
    }

    // Update is called once per frame
    void SpawnAllColumns()
    {
        for (int i = 0; i < columns.Count; i++)
        {
            GameObject instance = Instantiate(columnPrefab, spawnParent);
            ColumnCycler cycler = instance.GetComponent<ColumnCycler>();

            // Assign graph images
            cycler.columnData = columns[i];
            cycler.RefreshImage();
        }
    }
}