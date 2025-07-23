using UnityEngine;
using System.Collections.Generic;

public class QuizManager : MonoBehaviour
{
    [Header("Quiz Settings")]
    public List<QuizQuestion> questions;
    public GameObject questionPanelPrefab;
    public Transform questionPanelParent;

    [Header("Quiz UI")]
    public GameObject quizCanvas;

    private Dictionary<int, int> playerAnswers = new();
    private bool quizVisible = false;

    public void ToggleQuiz()
    {
        quizVisible = !quizVisible;
        quizCanvas.SetActive(quizVisible);

        if (quizVisible)
            ShowQuestions();
    }

    void ShowQuestions()
    {
        foreach (Transform child in questionPanelParent)
            Destroy(child.gameObject);

        for (int i = 0; i < questions.Count; i++)
        {
            var panel = Instantiate(questionPanelPrefab, questionPanelParent);
            var panelScript = panel.GetComponent<QuestionPanel>();
            int selectedOption = playerAnswers.ContainsKey(i) ? playerAnswers[i] : -1;
            panelScript.Setup(questions[i], i, this, selectedOption);
        }
    }

    public void OnAnswerSelected(int questionIndex, int optionIndex)
    {
        playerAnswers[questionIndex] = optionIndex;
        ShowQuestions(); // Refresh to show explanation
    }
}
