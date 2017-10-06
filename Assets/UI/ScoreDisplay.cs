using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour {

	[SerializeField] Text scoreText;

	void Start () {
		//Displays the players score on screen.
		scoreText.text = GameManager.score.ToString();
	}
}
