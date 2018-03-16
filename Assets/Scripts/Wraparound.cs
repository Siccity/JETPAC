using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wraparound : MonoBehaviour {

	// Update is called once per frame
	void Update() {
		if (transform.position.x > 6.6f) transform.position -= new Vector3(6.6f * 2f, 0f, 0f);
		else if (transform.position.x < -6.6f) transform.position += new Vector3(6.6f * 2f, 0f, 0f);
	}
}