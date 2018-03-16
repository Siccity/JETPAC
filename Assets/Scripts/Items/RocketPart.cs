using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketPart : Pickup {

	public bool secondPart;

	private Rigidbody2D body;
	private Collider2D[] cols;
	private AudioSource pickupSound;

	void Awake() {
		cols = GetComponentsInChildren<Collider2D>();
		body = GetComponent<Rigidbody2D>();
		pickupSound = GetComponent<AudioSource>();
	}

	public override void OnPickup(Player player) {
		if (player.rocketPart != null) return;
		if (secondPart && Rocket.rocket.constructionStage == Rocket.ConstructionStage.Stage1) return;
		SetColliders(false);

		body.velocity = Vector2.zero;
		body.isKinematic = true;
		transform.parent = player.transform;
		transform.localPosition = Vector3.zero;
		player.rocketPart = this;
		pickupSound.Play();
		if (player.playerNum == Player.PlayerNum.PlayerOne) {
			GameManager.instance.p1Score += 100;
		} else {
			GameManager.instance.p2Score += 100;
		}
	}

	public void SetColliders(bool enabled) {
		foreach(Collider2D col in cols) col.enabled = enabled;
	}

	public void Drop() {
		SetColliders(true);
		body.isKinematic = false;
		transform.parent = null;
	}
}