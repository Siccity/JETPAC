using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {
	public float dist;
	public float time;
	public AnimationCurve curve;
	public TrailRenderer trail;
	[HideInInspector] public Player.PlayerNum owner;

	private float startPos;
	private float spawnTime;
	private bool hasHit;

	void Start() {
		startPos = transform.position.x;
		spawnTime = Time.time;
	}

	void Update () {
		if (hasHit) return;

		Vector3 pos = transform.position;
		float t = (Time.time - spawnTime) / time;
		if (t > 1) t = 1;
		pos.x = Mathf.Lerp(startPos, startPos + dist, curve.Evaluate(t));
		transform.position = pos;

		if (Time.time - spawnTime > 3) Destroy(gameObject);
	}

	void OnTriggerEnter2D(Collider2D col) {
		Enemy enemy = col.GetComponent<Enemy>();
		if (enemy != null) {
			if (enemy is Bacteria) {
				Bacteria b = enemy as Bacteria;
				b.splat.Play();
			}
			enemy.Die();
			if (owner == Player.PlayerNum.PlayerOne) {
				GameManager.instance.p1Score += 25;
			} else {
				GameManager.instance.p2Score += 25;
			}
		}
		hasHit = true;
	}
}
