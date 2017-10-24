using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator_Script : MonoBehaviour {

	private Rigidbody2D rb2d;
	private int direction, speed;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		direction = 1; 
			speed = 0;
		rb2d.velocity = new Vector2 (0f, direction*speed);
	}
	
	// Update is called once per frame
	void Update () {
		rb2d.AddForce (new Vector2 (0f, direction*speed));
	}

	// On Broadcast Listener
	void OnElevatorMove() {
		Debug.Log ("Broadcast Received");
		speed++;
		/*
		rb2d.constraints = RigidbodyConstraints2D.None;
		rb2d.constraints = RigidbodyConstraints2D.FreezePositionX;
		rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
		*/
		Debug.Log ("After Broadcast: "+speed+":"+direction);
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Player") {
//			rb2d.constraints = RigidbodyConstraints2D.FreezePositionY;
			return;
		}

		Debug.Log ("Collided with "+coll.gameObject.name);
		direction = direction * -1;
		speed = 0;
		Debug.Log ("After Collision: "+speed+":"+direction);
	}

	void OnGUI(){
		GUI.Label(new Rect(10, 150, 50, 20), "Speed: "+speed);
		GUI.Label(new Rect(60, 150, 50, 20), "Direction: "+direction);
		GUI.Label(new Rect(110, 150, 50, 20), "Velocity: "+rb2d.velocity);
//		GUI.Label(new Rect(160, 150, 50, 20), "Force: "+rb2d.for);
	}
}
