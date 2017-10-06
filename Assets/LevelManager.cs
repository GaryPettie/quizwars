using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	/// <summary>
	/// Loads the specified level.
	/// </summary>
	/// <param name="levelIndex">Level index.</param>
	public void loadLevel(int levelIndex) {
		SceneManager.LoadScene(levelIndex);
	}

	/// <summary>
	/// Loads the next scene in the build order, or loops back to the main menu if the last scene is reached.
	/// </summary>
	public void LoadNextLevel () {
		int currentScene = SceneManager.GetActiveScene().buildIndex;
		int nextScene = currentScene + 1;
		if (nextScene < SceneManager.sceneCountInBuildSettings) {

			//reset the players score upon loading the main menu
			Debug.Log(nextScene);
			if (currentScene == 0) {
				GameManager.score = 0;
			}

			SceneManager.LoadScene(nextScene);
		}
		else {
			SceneManager.LoadScene(0);
		}
	}
}
