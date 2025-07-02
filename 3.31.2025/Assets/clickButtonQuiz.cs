using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickButtonQuiz : MonoBehaviour
{
    public GameObject level1Surveys;

    public void ShowLevel1Surveys()
    {
        if (level1Surveys != null)
        {
            level1Surveys.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Level1Surveys is not assigned.");
        }
    }
}
