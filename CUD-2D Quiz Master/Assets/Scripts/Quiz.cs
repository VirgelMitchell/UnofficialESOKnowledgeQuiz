using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


namespace core
{
	  public class Quiz : MonoBehaviour
	  {
			#region Variables Set in Inspector

			[Header("Question & Answer")]
			[SerializeField] Text questionText;
			[SerializeField] Text citationText;
			[SerializeField] List<Subject> subjects;

			[Header("Buttons & Controls")]
			[SerializeField] Button answerButtonPrefab;
			[SerializeField] GameObject answerButtonGroup;
			[SerializeField] Sprite[] buttonImages;             //TODO: Add tooltip for buttonImages array

			#endregion

			#region Variables set in Script

			int correctAnswerIndex;
			string[] answers;
			string correctionMessage;
			string citation;

			List<QuestionSO> questions;
			Button[] buttons;
			ScoreKeeper scoreKeeper;
			QuestionSO question;

			#endregion

			#region Public Methods

			internal float GetAllottedTime() => question.GetAllottedTime();
			internal void LoadQuestions(string subject, int count)
			{
				  SetReferences();
				  Subject topic = subjects[0];
				  for (int i = 0; i < subjects.Count; i++)
				  {
						if (subjects[i].GetName() == subject)
						{
							  topic = subjects[i];
							  questions = topic.GetQuestions(count);
							  break;
						}
						i++;
				  }
				  if (questions.Count <= 0) questions = topic.GetQuestions(count);
				  scoreKeeper.StartGame(questions.Count);
			}
			internal void SetNextQuestion()
			{
				  citationText.gameObject.SetActive(false);

				  question = questions[Random.Range(0, questions.Count - 1)];
				  questionText.text = question.GetQuestion();

				  answers = question.GetAnswers();
				  correctAnswerIndex = question.GetCorrectAnswerIndex();
				  correctionMessage = question.GetCorrectionText();
				  citation = question.GetCitation();

				  scoreKeeper.IncrementQuestionsAsked();

				  RemoveUsedQuestion();
				  SetUpButtons();
			}
			internal IEnumerator CheckAnswer(int answerIndex)
			{
				  foreach (var button in answerButtonGroup.GetComponentsInChildren<Button>()) button.interactable = false;

				  if (answerIndex < 0) DisplayMessage(0);
				  else if (answerIndex == correctAnswerIndex) DisplayMessage(1);
				  else DisplayMessage(2);
				  yield return null;

				  void DisplayMessage(int a)
				  {
						string correctAnswerMessage = "Congratulations, your answer is correct!";

						string incorrectAnswerMessage = "I'm sorry but, you have selected an incorrect response.\n\n";

						string outOfTimeMessage = "That is too bad, you have run out of time.\n\n";
						answerButtonGroup.transform.GetChild(correctAnswerIndex).GetComponent<Image>().sprite = buttonImages[1];
						switch (a)
						{
							  case 0: // Out of Time
									questionText.text = string.Concat(outOfTimeMessage + correctionMessage);
									citationText.text = citation;
									citationText.gameObject.SetActive(true);
									scoreKeeper.AnsweredIncorrectly();
									break;
							  case 1: // Correct Answer
									questionText.text = correctAnswerMessage;
									scoreKeeper.IncrementCorrectAnswers();
									break;
							  case 2:  // Incorrect Answer
									questionText.text = string.Concat(incorrectAnswerMessage + correctionMessage);
									answerButtonGroup.transform.GetChild(answerIndex).GetComponent<Image>().sprite = buttonImages[2];
									citationText.text = citation;
									citationText.gameObject.SetActive(true);
									scoreKeeper.AnsweredIncorrectly();
									break;
							  default:
									Debug.LogWarning("Answer Index out of Range");
									break;
						}
				  }
			}

			#endregion

			#region Private Methods

			private void RemoveUsedQuestion()
			{
				  if (questions.Contains(question))
				  {
						questions.Remove(question);
						questions.TrimExcess();
				  }
			}
			private void SetUpButtons()
			{
				  if (question.GetTrueOrFalse()) { AlternateButtonSetup(buttons); }
				  else
				  {
						int answerIndex = 0;
						foreach (var button in buttons)
						{
							  button.GetComponentInChildren<Image>().sprite = buttonImages[0];
							  button.interactable = true;
							  button.GetComponentInChildren<Text>().text = answers[answerIndex];
							  answerIndex++;
						}
				  }
			}
			private void AlternateButtonSetup(Button[] buttons)
			{
				  foreach (var button in buttons)
				  {
						button.GetComponentInChildren<Image>().sprite = buttonImages[0];

						if (button == buttons[0])
						{
							  button.GetComponentInChildren<Text>().text = "True";
							  button.interactable = true;
						}
						else if (button == buttons[1])
						{
							  button.GetComponentInChildren<Text>().text = "False";
							  button.interactable = true;
						}
						else
						{
							  button.GetComponentInChildren<Text>().text = "";
							  button.interactable = false;
						}
				  }
			}

			#endregion

			#region Utility Methods

			private void SetReferences()
			{
				  scoreKeeper = FindObjectOfType<ScoreKeeper>();
				  buttons = answerButtonGroup.GetComponentsInChildren<Button>();
			}

			#endregion
	  }
}