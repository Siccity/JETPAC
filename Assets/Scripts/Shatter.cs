using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shatter : MonoBehaviour {
	public List<Rigidbody2D> bodies;
	// Use this for initialization
	void Start() {
		foreach (Rigidbody2D body in bodies) {
			body.velocity = Random.insideUnitCircle;
			body.angularVelocity = (Random.value - 0.5f) * 20f;
		}
		StartCoroutine(Cleanup());
	}

	IEnumerator Cleanup() {
		while (bodies.Count > 0) {
			yield return new WaitForSeconds(0.1f);
			int index = Random.Range(0, bodies.Count);
			Destroy(bodies[index].gameObject);
			bodies.RemoveAt(index);
		}
		Destroy(gameObject);
	}
}