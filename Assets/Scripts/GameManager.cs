using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	public LevelManager levelManager;
	public Player.PlayerNum sessionPlayers;
	public GameObject gameOver;
	public int p1Score { get { return _p1Score; } set { _p1Score = value; if (value > hiScore) hiScore = value; PlayerPrefs.SetInt("jetpac_hs", hiScore); PlayerPrefs.Save(); } }
	private int _p1Score;
	public int p2Score { get { return _p2Score; } set { _p2Score = value; if (value > hiScore) hiScore = value; PlayerPrefs.SetInt("jetpac_hs", hiScore); PlayerPrefs.Save(); } }
	private int _p2Score;
	public int p1Lives;
	public int p2Lives;
	public AudioSource dieSound;
	[HideInInspector] public int hiScore;

	// Use this for initialization
	void Awake() {
		instance = this;
		hiScore = PlayerPrefs.GetInt("jetpac_hs");
	}

	void Start() {
		GotoMenu();
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) GotoMenu();
	}
	public void GotoMenu() {
		gameOver.SetActive(false);
		MainMenu.instance.gameObject.SetActive(true);
		levelManager.UnloadLevel();
	}

	public void StartGame(Player.PlayerNum players) {
		sessionPlayers = players;
		p1Score = 0;
		p2Score = 0;
		p1Lives = 4;
		p2Lives = players == Player.PlayerNum.PlayerTwo ? 4 : -1;
		MainMenu.instance.gameObject.SetActive(false);
		levelManager.LoadLevel(1);
	}

	public void RestartLevel() {
		levelManager.ReloadLevel();
	}

	public void GameOver() {
		levelManager.UnloadLevel();
		gameOver.SetActive(true);
		Invoke("GotoMenu", 3);
	}
}