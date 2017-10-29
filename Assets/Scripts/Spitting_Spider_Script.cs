using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spitting_Spider_Script : Enemy {

	public GameObject spit, target;
	public float spitSpeed;
    public Animator anim;

    public int distanceToShoot;
	private Vector2 vectorToTarget;
	private float angle;
	private Quaternion qt;
	private float 
		spitTimer = 0f,
		SPIT_TIME_MAX = 2.0f;

	public Spitting_Spider_Script() {
		health = 3.0f;
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
			transform.rotation = Quaternion.Slerp(transform.rotation, qt, Time.deltaTime * 8f);
			flash ();
			ShootSpits (playerIsClose());
		} else {
			// Do Nothing
		}

	}

	void ShootSpits(bool shoot) {
		spitTimer += Time.deltaTime;
		if (shoot && spitTimer>SPIT_TIME_MAX) {
            anim.Play("Attack");
			spitTimer = 0f;
			GameObject spitClone = Instantiate (spit, transform.position, Quaternion.identity);
            Quaternion rotation = transform.rotation;
            spitClone.transform.rotation.Set(rotation.x,rotation.y,rotation.z + 180, rotation.w);
			spitClone.GetComponent<Rigidbody2D> ().velocity = (vectorToTarget*spitSpeed);
		}
	}

	bool playerIsClose() {
		bool farAway = true;
		if (vectorToTarget.x>-distanceToShoot && vectorToTarget.x< distanceToShoot && vectorToTarget.y>-distanceToShoot && vectorToTarget.y< distanceToShoot) {
			farAway = false;
		}
		return !farAway;
	}
}
