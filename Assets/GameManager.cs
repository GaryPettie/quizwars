using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	[Header("UI")]
	[SerializeField] Color32 correctAnswerColor = new Color32(85, 255, 85, 255);
	[SerializeField] Color32 incorrectAnswerColor = new Color32(255, 85, 85, 255);
	[SerializeField] Text completionText;
	[SerializeField] Text scoreText;
	[SerializeField] Text questionText;
	[SerializeField] Text[] answerText;

	[Header("Questions")]
	[SerializeField] float waitBetweenQuestions = 1;
	[SerializeField] int pointsPerQuestion = 10;
	[SerializeField] Question[] questions;

	public static List<Question> unansweredQuestions;
	public static int score = 0;

	Question currentQuestion; 
	int correctAnswerIndex;
	float completionPercentage;
	int questionsAsked = 1;
	int totalQuestions;


	ColorBlock colorBlock = new ColorBlock();
	Color32 normalButtonColor = new Color32(245, 208, 191, 255);
	Color32 highlightedButtonColor = new Color32(239, 175, 131, 255);
	Color32 PressedButtonColor = new Color32(200, 200, 200, 255);

	LevelManager levelManager;
	AudioSource audioSource;

	void Start () {
		levelManager = FindObjectOfType<LevelManager>();
		audioSource = GetComponent<AudioSource>();

		unansweredQuestions = questions.ToList<Question>();
		totalQuestions = unansweredQuestions.Count;

		StartNewRound();
	}

	/// <summary>
	/// Prepares the next round of questions.
	/// </summary>
	public void StartNewRound() {
		GetRandomQuestion();
		ShuffleAnswers(currentQuestion);
		UpdateUI();
	}

	/// <summary>
	/// Draws a new question at random from the unansweredQuestions list.
	/// </summary>
	void GetRandomQuestion () {
		int randomIndex = Random.Range (0, unansweredQuestions.Count);
		currentQuestion = unansweredQuestions[randomIndex];
	}

	/// <summary>
	/// Shuffles the answers associated with the currently loaded questions. 
	/// </summary>
	/// <param name="currentQuestion">Current question.</param>
	void ShuffleAnswers (Question currentQuestion) {
		correctAnswerIndex = 0;
		string correctAnswer = currentQuestion.answers[correctAnswerIndex];

		//shuffles the answers
		for (int i = 0; i < currentQuestion.answers.Count; i++) {
			string temp = currentQuestion.answers[i];
			int randomIndex = Random.Range(i, currentQuestion.answers.Count);
			currentQuestion.answers[i] = currentQuestion.answers[randomIndex];
			currentQuestion.answers[randomIndex] = temp;
		}

		//finds the correct answer and tracks its current index
		for (int i = 0; i < currentQuestion.answers.Count; i++) {
			if (currentQuestion.answers[i] == correctAnswer) {
				correctAnswerIndex = i;
			}
		}
	}

	/// <summary>
	/// Updates all the UI elements.
	/// </summary>
	void UpdateUI () {
		//Resets button colors to default
		SetDefaultButtonColor();

		//Show next question
		questionText.text = currentQuestion.question;

		//show next answers
		for (int i = 0; i < answerText.Length; i++) {
			answerText[i].text = currentQuestion.answers[i];
		}

		//show new score
		scoreText.text = score.ToString();

		//show progression
		completionText.text = questionsAsked + " / " + totalQuestions;
	}

	/// <summary>
	/// Links to the buttons on the UI and tracks whether a correct answer was selected. 
	/// </summary>
	/// <param name="answer">Answer.</param>
	public void OnAnswerSelected (Text answer) {
		//plays selection sound
		audioSource.Play();

		//updates button visuals and score
		Button answerButton = answer.GetComponentInParent<Button>();
		if (answer.text == currentQuestion.answers[correctAnswerIndex]) {

			//changes button color to incorrectAnswerColor (and accounts for the Unity bug that affects highlighted color staying active after click)
			colorBlock.normalColor = correctAnswerColor;
			colorBlock.highlightedColor = correctAnswerColor;
			answerButton.colors = colorBlock;

			Debug.Log(answerButton.ToString());

			//add to score
			score += pointsPerQuestion;
		}
		else {
			//changes button color to incorrectAnswerColor (and accounts for the Unity bug that affects highlighted color staying active after click)
			colorBlock.normalColor = incorrectAnswerColor;
			colorBlock.highlightedColor = incorrectAnswerColor;
			answerButton.colors = colorBlock;
		}

		//loads the next question
		StartCoroutine(LoadNextQuestion());
	}

	/// <summary>
	/// Loads the next question after a short delay.
	/// </summary>
	/// <returns>The next question.</returns>
	IEnumerator LoadNextQuestion() {
		unansweredQuestions.Remove(currentQuestion);

		yield return new WaitForSeconds(waitBetweenQuestions);

		if (unansweredQuestions.Count != 0) {
			questionsAsked++;
			StartNewRound();
		}
		else {
			UpdateUI();
			levelManager.LoadNextLevel();
		}
	}

	void SetDefaultButtonColor () {
		Debug.Log(normalButtonColor);
		colorBlock.colorMultiplier = 1;
		colorBlock.normalColor = normalButtonColor;
		colorBlock.highlightedColor = highlightedButtonColor;
		colorBlock.pressedColor = PressedButtonColor;
		foreach (Text answer in answerText) {
			answer.GetComponentInParent<Button>().colors = colorBlock;
		}
	}
}