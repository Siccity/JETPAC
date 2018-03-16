using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
	public static MainMenu instance;

	public MenuButton btn1player;
	public MenuButton btn2player;
	public MenuButton btnKeyboard;

	private Player.PlayerNum players;

	void Awake() {
		instance = this;
	}

	void Start() {
		btn1player.on = true;
		btn2player.on = false;
		players = Player.PlayerNum.PlayerOne;
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			btn1player.on = true;
			btn2player.on = false;
			players = Player.PlayerNum.PlayerOne;
		} else if (Input.GetKeyDown(KeyCode.Alpha2)) {
			btn1player.on = false;
			btn2player.on = true;
			players = Player.PlayerNum.PlayerTwo;
		} else if (Input.GetKeyDown(KeyCode.Alpha3)) {
			btnKeyboard.on = !btnKeyboard.on;
		} else if (Input.GetKeyDown(KeyCode.Alpha5)) {
			GameManager.instance.StartGame(players);
		}else if (Input.GetKeyDown(KeyCode.Alpha0)) {
			Application.Quit();
		}
	}
}