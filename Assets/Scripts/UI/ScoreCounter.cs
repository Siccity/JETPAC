using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ScoreCounter : MonoBehaviour {

	[TextArea]
	public string template;
	public enum Score { P1, P2, Hi, P1Lives, P2Lives }
	public Score scoreType;
	private Text text;
	void Awake() {
		text = GetComponent<Text>();
	}

	void Update() {
		if (scoreType == Score.P1Lives) {
			int lives = Mathf.Max(GameManager.instance.p1Lives, 0);
			text.text = lives.ToString();
		} else if (scoreType == Score.P2Lives) {
			int lives = Mathf.Max(GameManager.instance.p2Lives, 0);
			text.text = lives.ToString();
		} else {
			int score;
			switch (scoreType) {
				case Score.P1:
					score = GameManager.instance.p1Score;
					break;
				case Score.P2:
					score = GameManager.instance.p2Score;
					break;
				default:
					score = GameManager.instance.hiScore;
					break;
			}
			text.text = string.Format(template, score.ToString("D6"));
		}
	}
}