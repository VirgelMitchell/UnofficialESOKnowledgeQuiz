using UnityEngine;
using core;
using System.Collections;

namespace control
{
	  public class GameManager : MonoBehaviour
	  {
			#region Variables set in Inspector

			[Header("Control Scripts")]
			[SerializeField] Fader fader = null;
			[SerializeField] Timer timer = null;
			[SerializeField] ScoreKeeper scoreKeeper = null;
			[SerializeField] Quiz quizControl = null;
			[SerializeField] GameSetUp setUp = null;
			[SerializeField] EndScreenControl endScreenControl = null;

			[Header("Screens")]
			[SerializeField] Canvas startScreen = null;
			[SerializeField] Canvas quizScreen = null;
			[SerializeField] Canvas endScreen = null;

			[Header("Other Variables")]
			[SerializeField] float timeToDisplayAnswer = 5f;
			[SerializeField] float waitTime = 1f;

			#endregion

			#region Variables set by Script

			const string startScreenCanvasName = "Start Screen Canvas";
			const string quizScreenCanvasName = "Quiz Screen Canvas";
			const string endScreenCanvasName = "End Screen Canvas";

			bool endGameFlag = false;
			bool gameOver = false;

			#endregion

			#region Public Methods

			internal void DisplayTimeExpired() { StartCoroutine(AskNextQuestion()); }
			internal void SetEndGame() { endGameFlag = true; }
			public IEnumerator StartGame(string subject, int questionsToAsk)
			{
				  // fader.FadeOut();
				  // yield return new WaitForSeconds(waitTime);
				  yield return StartCoroutine(fader.FadeToBlack(waitTime));

				  startScreen.gameObject.SetActive(false);
				  quizScreen.gameObject.SetActive(true);
				  scoreKeeper.CheckReferences();
				  quizControl.LoadQuestions(subject, questionsToAsk);
				  quizControl.SetNextQuestion();

				  // fader.FadeIn();
				  // yield return new WaitForSeconds(waitTime);
				  yield return StartCoroutine(fader.FadeToTransparent(waitTime));

				  Debug.Log("Back from Fade Operation");

				  Debug.Log(this + "Setting waitingForAnswer");
				  timer.WaitingForAnswer(true);

				  Debug.Log(this + "Starting Timer");
				  timer.StartTimer(quizControl.GetAllottedTime());

				  yield return null;
			}
			public void OnAnswerSelection(int index)
			{
				  timer.SetQuestionAnswered();
				  StartCoroutine(quizControl.CheckAnswer(index));
				  timer.StartTimer(timeToDisplayAnswer);
			}

			#endregion

			#region Private Methods

			private IEnumerator AskNextQuestion()
			{
				  if (endGameFlag) { yield return StartCoroutine(EndGame()); }
				  else
				  {
						timer.TimeExpired(false);
						fader.FadeOut();
						yield return new WaitForSeconds(waitTime);

						quizControl.SetNextQuestion();

						fader.FadeIn();
						yield return new WaitForSeconds(waitTime);

						timer.WaitingForAnswer(true);
						timer.StartTimer(quizControl.GetAllottedTime());
				  }

				  yield return null;
			}
			private IEnumerator EndGame()
			{
				  gameOver = true;
				  fader.FadeOut();
				  yield return new WaitForSeconds(waitTime);
				  quizScreen.gameObject.SetActive(false);
				  endScreen.gameObject.SetActive(true);
				  endScreenControl.SetUpDisplay(FindObjectOfType<ScoreKeeper>().GetFinalInfo());
				  yield return new WaitForSeconds(waitTime);
				  fader.FadeIn();
				  yield return null;
			}

			#endregion

			#region Utility Methods
			private void Start()
			{
				  ProtectAgainstNullRef();

				  fader.SetSortLevel();
				  
				  startScreen.gameObject.SetActive(true);
				  quizScreen.gameObject.SetActive(false);
				  endScreen.gameObject.SetActive(false);

				  fader.FadeIn();
				  fader.ToggleRaycast();
			}
			private void Update()
			{
				  while (!timer.TimeExpired() || gameOver) return;
				  if (timer.WaitingForAnswer()) OnAnswerSelection(-1);
				  else DisplayTimeExpired();
			}
			private void ProtectAgainstNullRef()
			{
				  if (!fader) fader = FindObjectOfType<Fader>();
				  if (!timer) timer = FindObjectOfType<Timer>();
				  if (!quizControl) quizControl = FindObjectOfType<Quiz>();
				  if (!setUp) setUp = FindObjectOfType<GameSetUp>();
				  if (!endScreenControl) endScreenControl = FindObjectOfType<EndScreenControl>();
				  if (!scoreKeeper) scoreKeeper = FindObjectOfType<ScoreKeeper>();

				  foreach (var canvas in FindObjectsOfType<Canvas>())
				  {
						switch (canvas.name)
						{
							  case startScreenCanvasName:
									if (!startScreen) startScreen = canvas;
									continue;
							  case quizScreenCanvasName:
									if (!quizScreen) quizScreen = canvas;
									continue;
							  case endScreenCanvasName:
									if (!endScreen) endScreen = canvas;
									continue;
							  default:
									continue;
						}
				  }
			}

			#endregion
	  }
}