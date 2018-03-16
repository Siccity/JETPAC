using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraPoints : Pickup {

	public Color[] colors;
	private SpriteRenderer sprite;

	public override void OnPickup(Player player) {
		player.pickupPoints.Play();
		if (player.playerNum == Player.PlayerNum.PlayerOne) {
			GameManager.instance.p1Score += 250;
		} else {
			GameManager.instance.p2Score += 250;
		}
		Destroy(gameObject);
	}

	void Awake() {
		sprite = GetComponent<SpriteRenderer>();
		sprite.color = colors[Random.Range(0, colors.Length)];
	}
}