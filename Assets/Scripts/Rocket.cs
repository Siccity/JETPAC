using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {
	public static Rocket rocket;

	public enum ConstructionStage { Stage1, Stage2, Stage3, Fuel1, Fuel2, Fuel3, Fuel4, Fuel5, Fuel6 }
	public ConstructionStage constructionStage = ConstructionStage.Stage3;
	public RocketPrefabs prefabs;
	public Animator animator;
	public ParticleSystem thruster;

	private float prevP1Pos;
	private float prevP2Pos;
	public GameObject rocketModel;
	public AudioSource onEnter;

	void Awake() {
		rocket = this;
	}

	void Update() {
		// Check if p1 passed rocket
		CheckPlayer(Player.p1, ref prevP1Pos);

		// Check if p2 passed rocket
		CheckPlayer(Player.p2, ref prevP2Pos);

#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.Keypad0)) LevelUp();
#endif
	}

	void Start() {
		if (constructionStage >= ConstructionStage.Stage3) {
			Enemy[] enemies = FindObjectsOfType<Enemy>();
			foreach (Enemy e in enemies) {
				e.Die(-1);
			}
			Player.DespawnPlayers();
			animator.SetTrigger("Land");
			Invoke("SpawnFuel", 2f);
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (constructionStage == ConstructionStage.Fuel6) {
			Player p = col.GetComponent<Player>();
			if (p != null) {
				p.gameObject.SetActive(false);
				animator.SetTrigger("Go");
				thruster.Play();
				onEnter.Play();
				GameManager.instance.p1Lives++;
				if (GameManager.instance.sessionPlayers == Player.PlayerNum.PlayerTwo) GameManager.instance.p2Lives++;
			}
		}
	}

	void CheckPlayer(Player player, ref float playerPos) {
		if (player.rocketPart != null) {
			if (NumberPassed(playerPos, player.transform.position.x, transform.position.x)) {
				player.rocketPart.transform.parent = transform;
				player.rocketPart.SetColliders(false);
				StartCoroutine(EatItem(player.rocketPart));
				player.rocketPart = null;
			}
		}
		playerPos = player.transform.position.x;
	}

	bool NumberPassed(float prev, float next, float cur) {
		if (prev <= cur && next >= cur) return true;
		else if (prev >= cur && next <= cur) return true;
		else return false;
	}

	IEnumerator EatItem(RocketPart part) {
		while (part.transform.localPosition.y > 0) {
			part.transform.localPosition += Vector3.down * Time.deltaTime * 10;
			yield return null;
		}
		LevelUp();
		part.gameObject.SetActive(false);
		if (constructionStage >= ConstructionStage.Stage3 && constructionStage < ConstructionStage.Fuel6) Invoke("SpawnFuel", 2f);
	}

	void LevelUp() {
		constructionStage++;
		Destroy(rocketModel);
		rocketModel = Instantiate(prefabs.GetPrefab(constructionStage), transform);
	}

	void SpawnFuel() {
		float x = Random.Range(-6f, 6f);
		if (x > transform.position.x + 0.5f || x < transform.position.x - 0.5f) {
			Instantiate(prefabs.fuel, new Vector3(x, 5.5f, 0), Quaternion.identity, transform.parent);
		} else Invoke("SpawnFuel", 1f);
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.magenta;
		Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 10);
	}

	public void Land() {
		if (constructionStage != ConstructionStage.Fuel6) {
			StartCoroutine(Player.RespawnPlayers());
		}
	}

	public void NextLevel() {
		if (constructionStage >= ConstructionStage.Fuel6) GameManager.instance.levelManager.LoadNextLevel();
	}
}