using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quiz Question")]
public class QuestionSO : ScriptableObject
{
    [TextArea(2,6)] [SerializeField] string question = "Give me a question.";
    [TextArea(1,2)] [SerializeField] string answer = "I don't know the Answer.";
    [TextArea(1, 2)] [SerializeField] string correctionText = "Why am I wrong?";
    [TextArea(1, 2)] [SerializeField] string[] possibleAnswers = new string[5];
    [TextArea(1, 2)] [SerializeField] string citation = "According to...";


    [SerializeField] bool isTrueOrFalse = false;
    [SerializeField] float allottedTime = 10f;

    int correctanswerIndex = -1;

    public string GetQuestion() { return question; }
    public float GetAllottedTime() { return allottedTime; }
    public string GetCorrectionText() { return correctionText; }
    public string GetCitation() { return citation; }
    public bool GetTrueOrFalse() { return isTrueOrFalse; }

    public int GetCorrectAnswerIndex()
    {
        if (correctanswerIndex < 0) Debug.LogError("Index out of range! You must call Get Answers before getting the Correct Answer Index!");
        return correctanswerIndex;
    }

    public string[] GetAnswers()
    {
        List<string> answerList = new List<string> { answer };
        foreach (var possibility in possibleAnswers) answerList.Add(possibility);

        string[] answers = new string[answerList.Count];

        for (int a = 0; a < answers.Length; a++)
        {
            int candidateIndex = Random.Range(0, answerList.Count - 1);
            string candidate = answerList[candidateIndex];
            if (candidate == answer) correctanswerIndex = a;
            answers[a] = candidate;
            answerList.RemoveAt(candidateIndex);
            answerList.TrimExcess();
        }
        return answers;
    }
}
