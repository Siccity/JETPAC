using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
	[HideInInspector] private Scene currentScene;
	private bool busy;

	void Update() {
		#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.Keypad1)) LoadNextLevel();
		#endif
	}
	public void LoadLevel(int level) {
		StartCoroutine(LoadLevelCo("Level" + level.ToString("D2")));
	}

	public void ReloadLevel() {
		StartCoroutine(LoadLevelCo(currentScene.name));
	}

	public void UnloadLevel() {
		StartCoroutine(LoadLevelCo(null));
	}

	public void LoadNextLevel() {
		int curlevel = 1;
		int.TryParse(currentScene.name.Replace("Level",""), out curlevel);
		curlevel++;
		LoadLevel(curlevel);
	}

	IEnumerator LoadLevelCo(string newLevel) {
		if (busy) yield break;
		busy = true;
		if (currentScene.IsValid()) {
			AsyncOperation async = SceneManager.UnloadSceneAsync(currentScene);
			yield return async;
		}
		if (string.IsNullOrEmpty(newLevel)) {
			currentScene = new Scene();
		} else if (!Application.CanStreamedLevelBeLoaded(newLevel)) {
			GameManager.instance.GameOver();
		} else {
			AsyncOperation async2 = SceneManager.LoadSceneAsync(newLevel, LoadSceneMode.Additive);
			yield return async2;
			currentScene = SceneManager.GetSceneByName(newLevel);
		}
		busy = false;
	}
}