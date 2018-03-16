using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerKeybinds : ScriptableObject {

	public KeyCode left = KeyCode.A;
	public KeyCode right = KeyCode.D;
	public KeyCode thrust = KeyCode.W;
	public KeyCode shoot = KeyCode.S;
}
