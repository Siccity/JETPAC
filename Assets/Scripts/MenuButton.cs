using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour {
	public Image image;
	public Text text;
	public bool on { get { return _on; } set { _on = value; trigger.Invoke(value); } }
	public bool _on;

	public BEvent trigger;

	void Reset() {
		image = GetComponentInChildren<Image>();
		text = GetComponentInChildren<Text>();
	}

	void Highlight(bool on) {
		image.enabled = on;
		text.color = on ? Color.black : Color.white;
	}

	void Update() {
		if (on) Highlight(Time.time % 0.5f > 0.25f);
		else Highlight(false);
	}

	[System.Serializable]
	public class BEvent : UnityEvent<bool> { }
}