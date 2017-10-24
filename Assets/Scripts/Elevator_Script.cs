using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator_Script : MonoBehaviour {

	[SerializeField] private LayerMask groundLayer;
	private Rigidbody2D rb2d;
	private int direction, speed;
	private RaycastHit2D groundHit;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		direction = 1; 
			speed = 0;
		rb2d.velocity = new Vector2 (0f, direction*speed);
	}
	
	// Update is called once per frame
	void Update () {
		groundHit = Physics2D.Raycast ((Vector3)transform.position, Vector2.down, 0.8f, groundLayer);
		rb2d.velocity = new Vector2 (0f, direction*speed);

		if (groundHit.collider != null) {
			direction = direction * -1;
			speed = 0;
			rb2d.velocity = new Vector2 (0f, direction * speed);
		}
	}

	// On Broadcast Listener
	void OnElevatorMove() {
		Debug.Log ("Broadcast Received");
		speed=2;
		/*
		rb2d.constraints = RigidbodyConstraints2D.None;
		rb2d.constraints = RigidbodyConstraints2D.FreezePositionX;
		rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
		*/
		Debug.Log ("After Broadcast: "+speed+":"+direction);
	}

	void OnCollisionEnter2D(Collision2D coll) {
		Debug.Log ("Collided with "+coll.gameObject.name);
		rb2d.velocity = new Vector2 (0f, direction*speed);
		if (coll.gameObject.tag == "Player") 
			return;

		Debug.Log ("Collided with "+coll.gameObject.name);
		direction = direction * -1;
		speed = 0;
		Debug.Log ("After Collision: "+speed+":"+direction);
	}

	void OnGUI(){
		GUI.Label(new Rect(10, 150, 70, 20), "Speed: "+speed);
		GUI.Label(new Rect(80, 150, 70, 20), "Direction: "+direction);
		GUI.Label(new Rect(170, 150, 140, 20), "Velocity: "+rb2d.velocity);
		GUI.Label(new Rect(310, 150, 100, 20), "onGround: "+(groundHit.collider!=null));
	}
}
