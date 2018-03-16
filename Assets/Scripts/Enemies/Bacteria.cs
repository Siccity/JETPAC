using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bacteria : Enemy {

	public float speed = 2;
	public RuntimeAnimatorController[] cols;
	public AudioSource splat;

	public override void Awake() {
		base.Awake();
	}

	void Start() {
		Spawn();
	}

	public override void Spawn() {
		sprite.flipX = Random.value > 0.5f;
		sprite.flipY = Random.value > 0.5f;
		GetComponent<Animator>().runtimeAnimatorController = cols[Random.Range(0,cols.Length)];
		sprite.enabled = true;
		SetColliders(true);
		transform.position = new Vector3(6.6f, Random.Range(-2.5f, 4f), 0);
		float angle = 45 + (Random.Range(0, 4) * 90);
		body.velocity = GetVelocity(angle) * speed;
	}

	public override void OnCollisionEnter2D(Collision2D col) {
		Player player = col.collider.GetComponent<Player>();
		if (player != null) {
			player.Die();
			Die();
		} else {
			//Debug.Log(col);
			//ContactPoint2D contact = col.contacts[0];
			//body.velocity = Vector2.Reflect(body.velocity, contact.normal).normalized * speed;
		}
	}
}