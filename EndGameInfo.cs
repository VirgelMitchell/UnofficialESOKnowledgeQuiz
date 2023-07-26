namespace core
{
	  struct EndGameInfo
	  {
			public string score;
			public string questionsAsked;
			public string correctAnswers;

			public EndGameInfo(string rawScore, int questionCount, int correctAnswerCount)
			{
				  score = rawScore;
				  questionsAsked = questionCount.ToString();
				  correctAnswers = correctAnswerCount.ToString();
			}
	  }
}