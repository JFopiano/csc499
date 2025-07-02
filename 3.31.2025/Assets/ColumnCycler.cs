using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColumnCycler : MonoBehaviour
{

    public Image graphImage;
    public ColumnData columnData;

    public void CycleGraph(int direction)
    {
        if (columnData.graphImages.Length == 0) return;

        columnData.currentGraphIndex = (columnData.currentGraphIndex + direction + columnData.graphImages.Length) % columnData.graphImages.Length;

        RefreshImage();
    }

    public void RefreshImage()
    {
        graphImage.sprite = columnData.graphImages[columnData.currentGraphIndex];
    }
}
