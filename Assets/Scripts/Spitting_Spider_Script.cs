using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spitting_Spider_Script : Enemy {

	public GameObject spit, target;
	public float spitSpeed;

	private Vector2 vectorToTarget;
	private Vector3 rotation;
	private float angle;
	private Quaternion qt;
	private float 
		spitTimer = 0f,
		SPIT_TIME_MAX = 2.0f;

	public Spitting_Spider_Script() {
		health = 10.0f;
		damage = 2.0f;

		name = "Spitting Spider";
	}

	// Use this for initialization
	void Start () {
		setSpriteRenderer (GetComponent<SpriteRenderer> ());
	}
	
	// Update is called once per frame
	void Update () {
		if (!isPaused()) {
			vectorToTarget = target.transform.position - transform.position;
			angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) - 90;
			qt = Quaternion.AngleAxis(angle, Vector3.forward);
			transform.rotation = Quaternion.Slerp(transform.rotation, qt, Time.deltaTime * 5f);
			flash ();
			ShootSpits (playerIsClose());
		} else {
			// Do Nothing
		}

	}

	void ShootSpits(bool shoot) {
		spitTimer += Time.deltaTime;
		if (shoot && spitTimer>SPIT_TIME_MAX) {
			spitTimer = 0f;
			GameObject spitClone = (GameObject)Instantiate (spit, transform.position, Quaternion.Euler (rotation));
			spitClone.GetComponent<Rigidbody2D> ().AddForce (vectorToTarget*spitSpeed);
			Debug.Log ("SPITTED !!");
		}
	}

	bool playerIsClose() {
		bool farAway = true;
		if (vectorToTarget.x>-6 && vectorToTarget.x<6 && vectorToTarget.y>-6 && vectorToTarget.y<6) {
			farAway = false;
		}
		return !farAway;
	}
}
