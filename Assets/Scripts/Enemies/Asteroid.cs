using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : Enemy {

	public float speed = 2;
	public Color[] cols;
	private float rot;
	private ParticleSystem psys;
	public AudioSource boom;
	public GameObject shatterPrefab;
	public override void Awake() {
		base.Awake();
		psys = GetComponentInChildren<ParticleSystem>();
	}

	public override void Die(float respawnTimer = 1) {
		base.Die();
		psys.Stop();
		Instantiate(shatterPrefab, transform.position, transform.rotation);
	}
	void Start() {
		Spawn();
	}

	public override void OnCollisionEnter2D(Collision2D col) {
		Player player = col.collider.GetComponent<Player>();
		if (player != null) {
			player.Die();
		} else boom.Play();
		Die();
	}

	public override void Spawn() {
		rot = Random.value * 3f;
		ParticleSystem.MainModule main = psys.main;
		Color col = cols[Random.Range(0, cols.Length)];
		col.a = main.startColor.color.a;
		main.startColor = col;

		sprite.enabled = true;
		psys.Play();
		SetColliders(true);
		// 0 = right, 90 = up, 180 = left, 270 = down
		bool fromLeft = Random.value > 0.5f;

		transform.position = new Vector3(fromLeft ? -6.6f : 6.6f, Random.Range(-2.5f, 4f), 0);

		float angle = fromLeft ? -20f : 180f;
		angle = Random.Range(angle, angle + 20);

		body.velocity = GetVelocity(angle) * speed;
		transform.rotation = Quaternion.Euler(0, 0, angle);
	}

	void FixedUpdate() {
		sprite.transform.Rotate(0, 0, rot);
	}
}