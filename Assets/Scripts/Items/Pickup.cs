using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour {
	public abstract void OnPickup(Player player);

	void OnTriggerStay2D(Collider2D collider) {
		Player player = collider.GetComponent<Player>();
		if (player != null) {
			OnPickup(player);
		}
	}
}