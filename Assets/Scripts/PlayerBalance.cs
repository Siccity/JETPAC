using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerBalance : ScriptableObject {

	public float walkSpeed = 1;
	public float flySpeed = 1;
	public float thrustSpeed = 1;
	public float fallSpeed = 1;
	public float vertAccel = 1;
	public Laser laser;
}
