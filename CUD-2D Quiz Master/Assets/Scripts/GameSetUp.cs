using UnityEngine;
using UnityEngine.UI;

namespace control
{
	  public class GameSetUp : MonoBehaviour
	  {
			#region Variables set in Inspector

			[Header("Subject Titles")]
			[SerializeField] string[] subjectNames;

			[Header("Text Boxes")]
			[SerializeField] Text quizLengthText;
			[SerializeField] Text estematedTime;

			[Header("Other Items")]
			[SerializeField] Dropdown subjectSelector;
			[SerializeField] int shortQuizLength = 10;
			[SerializeField] int mediumQuizLength = 20;
			[SerializeField] int longQuizLength = 30;

			#endregion

			#region Variables set by script

			GameManager gM;

			int questionsToAsk = 0;
			string subject;

			#endregion

			#region Interaction Methods

			public void OnClick()
			{
				  if (string.IsNullOrEmpty(subject))
				  {
						PopUpMessage("You must select a subject to start the game.");
						return;
				  }
				  if (questionsToAsk < 1)
				  {
						PopUpMessage("You must select a quiz length to play the game!");
						return;
				  }

				  StartCoroutine(gM.StartGame(subject, questionsToAsk));
			}
			public void OnSubjectChange()
			{
				  subject = subjectNames[subjectSelector.value];
			}
			public void OnQuizLength()
			{
				  var quizLengthValue = GetComponentInChildren<Slider>().value;

				  string quizLength;

				  switch (quizLengthValue)
				  {
						case 1:
							  quizLength = "Short";
							  questionsToAsk = shortQuizLength;
							  break;
						case 2:
							  quizLength = "Medium";
							  questionsToAsk = mediumQuizLength;
							  break;
						case 3:
							  quizLength = "Long";
							  questionsToAsk = longQuizLength;
							  break;
						default:
							  quizLength = "Random";
							  questionsToAsk = 0;
							  break;
				  }

				  quizLengthText.text = quizLength;

				  if (questionsToAsk == 0) estematedTime.text =
							  "Unable to calculate an estimated time to completion for a random length quiz.";
				  else
				  {
						float et2c = questionsToAsk * 0.5f;
						string leader = "The estimated time to complete this quiz is: ";
						estematedTime.text = string.Concat(leader, et2c.ToString(), " minutes.");
				  }
			}

			#endregion

			#region Utility Methods

			private void Start()
			{
				  gM = FindObjectOfType<GameManager>();

				  if (!subjectSelector) subjectSelector = FindObjectOfType<Dropdown>();
				  OnQuizLength();
				  OnSubjectChange();
			}

			private void PopUpMessage(string message)
			{
				  Debug.Log("Method not Implemented.");

				  //TODO: write method
				  /// This method needs to pop up a message box to display a message that
				  /// will tell the player that they need to select both a "Subject" and
				  /// a "Quiz Length" in order to start the game.  In the mean time, I
				  /// have set up the startup  routine to automatically set the first
				  /// subject and a medium length quiz as default.
			}

			#endregion
	  }
}