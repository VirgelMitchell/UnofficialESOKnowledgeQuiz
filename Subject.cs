using System.Collections.Generic;
using UnityEngine;

namespace core
{
	  [System.Serializable]
	  public struct Subject
	  {
			[SerializeField] string subjectName;
			[SerializeField] List<QuestionSO> questions;

			public Subject(string name)
			{
				  subjectName = name;
				  questions = new List<QuestionSO>();
			}
			public Subject(int length)
			{
				  subjectName = "New Subject";
				  questions = new List<QuestionSO>(length);
			}
			public Subject(string name, int length)
			{
				  subjectName = name;
				  questions = new List<QuestionSO>(length);
			}

			public string GetName() { return subjectName; }

			public List<QuestionSO> GetQuestions(int count = 0)
			{
				  if (count == 0) return questions;
				  else
				  {
						List<QuestionSO> newQuestionList = new List<QuestionSO>();
						List<QuestionSO> workingList = questions;

						newQuestionList.Clear();

						for(int i = 0;i<count;i++)
						{
							  QuestionSO question = workingList[Random.Range(0, workingList.Count - 1)];
							  newQuestionList.Add(question);
							  if (workingList.Contains(question)) workingList.Remove(question);
							  workingList.TrimExcess();
							  i++;
						}
						return newQuestionList;
				  }
			}
	  }
}