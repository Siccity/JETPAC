using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public static Player p1, p2;

	public enum PlayerNum { PlayerOne, PlayerTwo }
	public PlayerNum playerNum;
	public PlayerKeybinds keybinds;
	public PlayerBalance balance;
	public RocketPart rocketPart;

	private Animator animator;
	private Rigidbody2D body;
	private float vertVel;
	private bool facingLeft;

	private float lastFire;
	private Vector3 spawnPoint;
	public LaserCharge[] laserCharges;
	public bool isGrounded { get { return groundedOn != null; } }
	private GameObject groundedOn;
	private ParticleSystem thrusterPsys;
	public AudioSource pickupPoints;

	[System.Serializable]
	public class LaserCharge {
		public float lastFire;
		public Material mat;
	}

	void Awake() {
		thrusterPsys = GetComponentInChildren<ParticleSystem>();
		animator = GetComponent<Animator>();
		spawnPoint = transform.position;
		body = GetComponent<Rigidbody2D>();
		if (playerNum == PlayerNum.PlayerOne) {
			p1 = this;
		} else {
			p2 = this;
			gameObject.SetActive(GameManager.instance.sessionPlayers == PlayerNum.PlayerTwo);
		}
	}

	void Start() {
		if (GameManager.instance.sessionPlayers == PlayerNum.PlayerTwo) {
			if (playerNum == PlayerNum.PlayerOne) {
				GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.5f, 0.5f);
			} else {
				GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 1.0f);
			}
		}
	}
	void FixedUpdate() {
		Movement();
		Laser();
	}

	void OnCollisionEnter2D(Collision2D col) {
		foreach (ContactPoint2D contact in col.contacts) {
			if (contact.normal.y > 0.9f) {
				groundedOn = contact.collider.gameObject;
				animator.SetBool("Grounded", true);
				thrusterPsys.Stop();
			}
		}
	}

	void OnCollisionExit2D(Collision2D col) {
		if (col.gameObject == groundedOn) {
			groundedOn = null;
			animator.SetBool("Grounded", false);
			thrusterPsys.Play();
		}
	}

	void Movement() {
		if (Input.GetKey(keybinds.left)) {
			transform.localPosition += Vector3.left * (isGrounded ? balance.walkSpeed : balance.flySpeed) * Time.deltaTime;
			facingLeft = true;
			animator.SetBool("Walking", true);
			transform.localScale = new Vector3(-1, 1, 1);
		} else if (Input.GetKey(keybinds.right)) {
			transform.localPosition += Vector3.right * (isGrounded ? balance.walkSpeed : balance.flySpeed) * Time.deltaTime;
			facingLeft = false;
			animator.SetBool("Walking", true);
			transform.localScale = new Vector3(1, 1, 1);
		} else {
			animator.SetBool("Walking", false);
		}
		if (Input.GetKey(keybinds.thrust)) {
			vertVel += balance.vertAccel * Time.deltaTime;
		} else {
			vertVel -= balance.vertAccel * Time.deltaTime;
		}
		vertVel = Mathf.Clamp(vertVel, -balance.fallSpeed, balance.thrustSpeed);
		transform.localPosition += new Vector3(0, vertVel * Time.deltaTime);

		if (transform.localPosition.y > 4.5f) transform.localPosition = new Vector3(transform.localPosition.x, 4.5f, 0f);
	}

	void Laser() {
		if (Input.GetKey(keybinds.shoot)) {
			if (Time.time - lastFire >= 0.1f) {
				for (int i = 0; i < laserCharges.Length; i++) {
					if (Time.time - laserCharges[i].lastFire >= 0.8f) {
						laserCharges[i].lastFire = Time.time;
						lastFire = Time.time;
						Laser laser = Instantiate(balance.laser, transform.position + (Vector3.up * Random.value * 0.1f), Quaternion.identity);
						Laser laserL = Instantiate(balance.laser, transform.position + (Vector3.up * Random.value * 0.1f) + new Vector3(6.6f*2, 0, 0), Quaternion.identity);
						Laser laserR = Instantiate(balance.laser, transform.position + (Vector3.up * Random.value * 0.1f) + new Vector3(-6.6f*2, 0, 0), Quaternion.identity);
						laser.GetComponent<AudioSource>().pitch = 1 + (Random.value * 0.2f);
						laserL.GetComponent<AudioSource>().enabled = false;
						laserR.GetComponent<AudioSource>().enabled = false;
						laser.trail.material = laserCharges[i].mat;
						laserL.trail.material = laserCharges[i].mat;
						laserR.trail.material = laserCharges[i].mat;
						laser.owner = playerNum;
						laserL.owner = playerNum;
						laserR.owner = playerNum;
						if (facingLeft) {
							laser.dist = -laser.dist;
							laserL.dist = -laserL.dist;
							laserR.dist = -laserR.dist;
						}
						break;
					}
				}
			}
		}
	}

	public void Die() {
		if (!gameObject.activeSelf) return;

		if (playerNum == PlayerNum.PlayerOne) GameManager.instance.p1Lives--;
		else GameManager.instance.p2Lives--;
		if (rocketPart != null) rocketPart.Drop();
		rocketPart = null;
		gameObject.SetActive(false);
		GameManager.instance.dieSound.Play();
		if (GameManager.instance.p1Lives == -1 && GameManager.instance.p2Lives == -1) {
			Invoke("GameOver", 1);
		} else {
			if (GameManager.instance.sessionPlayers == PlayerNum.PlayerOne) GameManager.instance.StartCoroutine(RespawnPlayers());
			else if (!p1.gameObject.activeSelf && !p2.gameObject.activeSelf) {
				GameManager.instance.StartCoroutine(RespawnPlayers());
			}
		}
	}

	public static void DespawnPlayers() {
		p1.gameObject.SetActive(false);
		p2.gameObject.SetActive(false);
	}

	public static IEnumerator RespawnPlayers() {
		yield return new WaitForSeconds(1);
		Enemy[] enemies = FindObjectsOfType<Enemy>();
		foreach (Enemy e in enemies) {
			e.Die(1);
		}
		yield return new WaitForSeconds(1);
		if (GameManager.instance.sessionPlayers == PlayerNum.PlayerOne) {
			p1.Respawn();
		} else {
			if (GameManager.instance.p1Lives >= 0) p1.Respawn();
			if (GameManager.instance.p2Lives >= 0) p2.Respawn();
		}
	}

	void Respawn() {
		gameObject.SetActive(true);
		transform.position = spawnPoint;
	}

	void GameOver() {
		GameManager.instance.GameOver();
	}
}