using System;
using UnityEngine;
using UnityEngine.UI;

namespace core
{
	  public class EndScreenControl : MonoBehaviour
	  {
			[SerializeField] Text correctAnswers;
			[SerializeField] Text questionsAsked;
			[SerializeField] Text finalScore;

			internal void SetUpDisplay(EndGameInfo egi)
			{
				  correctAnswers.text = egi.correctAnswers;
				  questionsAsked.text = egi.questionsAsked;
				  finalScore.text = egi.score;
			}
	  }
}
