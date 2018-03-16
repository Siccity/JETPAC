using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour {

	public ExtraPoints[] pickups;
	private ExtraPoints current;
	private float next;

	void Start() {
		next = Time.time + Random.Range(2, 20);
	}

	void Update() {
		if (Time.time > next) {
			if (current == null) {
				float x = Random.Range(-6f, 6f);
				current = Instantiate(pickups[Random.Range(0, pickups.Length)], new Vector3(x, 5.5f, 0), Quaternion.identity, transform);
			}
			next = Time.time + Random.Range(2, 20);
		}
	}
}