using System;
using UnityEngine;
using UnityEngine.UI;

namespace core
{
	  public class Timer : MonoBehaviour
	  {
			#region Variables set in Inspector

			[SerializeField] Text timerText;
			[SerializeField] Image timerDisplay;

			#endregion

			#region Variables set by Script

			float allottedTime;
			float timeRemaining = 0f;

			bool timerIsRunning = false;
			bool waitingForAnswer = false;
			bool timeExpired = false;

			#endregion

			#region Public Methods

			internal bool TimeExpired() => timeExpired;
			internal bool WaitingForAnswer() => waitingForAnswer;

			internal void SetQuestionAnswered() => QuestionAnswered();
			internal void StartTimer(float t) => ResetandStartTimer(t);

			internal void TimeExpired(bool state) { timeExpired = state; }
			internal void WaitingForAnswer(bool state) { waitingForAnswer = state; }
			
			#endregion
			
			#region Private Methods

			private void UpdateTimer()
			{
				  timeRemaining -= Time.deltaTime;
				  timerDisplay.fillAmount = timeRemaining / allottedTime;
				  timerText.text = Math.Truncate(timeRemaining).ToString();
			}
			private void ResetandStartTimer(float t)
			{
				  allottedTime = t;
				  timeRemaining = t;
				  timeExpired = false;
				  timerIsRunning = true;
			}
			private void QuestionAnswered()
			{
				  timerIsRunning = false;
				  waitingForAnswer = false;
			}

			#endregion

			#region Utility Methods

			private void Update()
			{
				  if (timerIsRunning)
				  {
						UpdateTimer();
						if (timeRemaining <= Mathf.Epsilon)
						{
							  timerIsRunning = false;
							  timeExpired = true;
						}
				  }
			}

			#endregion
	  }
}
