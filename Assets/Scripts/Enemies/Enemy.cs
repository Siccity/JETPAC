using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {
	public Rigidbody2D body { get; private set; }
	protected SpriteRenderer sprite;
	protected Collider2D[] colliders;
	public virtual void Awake() {
		body = GetComponent<Rigidbody2D>();
		sprite = GetComponentInChildren<SpriteRenderer>();
		colliders = GetComponentsInChildren<Collider2D>();
	}

	protected void SetColliders(bool on) {
		foreach (Collider2D c in colliders) {
			c.enabled = on;
		}
	}

	public virtual void Die(float respawnTimer = 1) {
		if (!gameObject.activeSelf) return;
		sprite.enabled = false;
		SetColliders(false);
		if (respawnTimer >= 0) Invoke("Spawn", respawnTimer);
	}

	public virtual void OnCollisionEnter2D(Collision2D col) {
		Player player = col.collider.GetComponent<Player>();
		if (player != null) {
			player.Die();
		}
		Die();
	}

	public Vector2 GetVelocity(float angleDeg) {
		float rad = Mathf.Deg2Rad * angleDeg;
		return new Vector2((float) Mathf.Cos(rad), (float) Mathf.Sin(rad));
	}

	public abstract void Spawn();
}