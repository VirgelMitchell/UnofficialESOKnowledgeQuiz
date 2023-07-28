using System;
using UnityEngine;
using control;
using UnityEngine.UI;

namespace core
{
	  public class ScoreKeeper : MonoBehaviour
	  {
			#region Variables set in Inspector

			[SerializeField] Text scoreText;
			[SerializeField] Slider progressBar;
			[SerializeField] Text progressText;

			#endregion

			#region Variables set by Script

			int numberOfQuestionsInQuiz = 0;
			int questionsAsked = 0;
			float correctAnswers = 0f;
			string score = "";
			const string suffix = "%";

			GameManager gM;

			#endregion

			#region Public Methods

			internal EndGameInfo GetFinalInfo()
			{
				  return new EndGameInfo(score, questionsAsked, (int)correctAnswers);
			}

			internal void StartGame(int count)
			{
				  numberOfQuestionsInQuiz = count;
				  progressBar.maxValue = count;
				  questionsAsked = 0;
				  scoreText.text = score;
			}

			internal void IncrementQuestionsAsked()
			{
				  questionsAsked++;
				  if (questionsAsked == numberOfQuestionsInQuiz) { gM.SetEndGame(); }
			}

			internal void IncrementCorrectAnswers()
			{
				  correctAnswers++;
				  UpdateScore();
				  UpdateProgress();
			}

			internal void AnsweredIncorrectly()
			{
				  UpdateScore();
				  UpdateProgress();
			}

			#endregion

			#region Private Methods

			private void UpdateScore()
			{
				  if (numberOfQuestionsInQuiz == 0)
				  {
						score = "Need INPUT!";
						scoreText.text = score;
				  }
				  else
				  {
						score = Mathf.Round(correctAnswers / questionsAsked * 100).ToString();
						scoreText.text = string.Concat(score, suffix);
				  }
			}
			private void UpdateProgress()
			{
				  progressBar.value = questionsAsked;
				  progressText.text = string.Concat(questionsAsked.ToString(), " / ", numberOfQuestionsInQuiz.ToString());
			}

			#endregion

			#region Utility Methods

			internal void CheckReferences()
			{
				  if (!scoreText) GameObject.Find("Score Text");
				  if (!progressText) GameObject.Find("Progress Text");
				  if (!progressBar) GameObject.Find("Progress Bar");

				  gM = FindObjectOfType<GameManager>();
			}

			#endregion
	  }
}
