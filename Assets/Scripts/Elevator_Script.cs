using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator_Script : MonoBehaviour {

	[SerializeField] private LayerMask groundLayer;
	private int direction, speed;
	private Rigidbody2D rb2d;
	private RaycastHit2D groundHit;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		direction = -1; 
			speed = 0;
		rb2d.velocity = new Vector2 (0f, direction*speed);
	}
	
	// Update is called once per frame
	void Update () {
		groundHit = Physics2D.Raycast ((Vector3)transform.position, (direction>0)?Vector2.up:Vector2.down, (direction>0)?1.5f:1.5f, groundLayer);
		rb2d.velocity = new Vector2 (0f, direction*speed);

		if (groundHit.collider != null && speed!=0) {
			direction = direction * -1;
			speed = 0;
		} 
	}

	// On Broadcast Listener
	void OnElevatorMove() {
		speed=2;
	}


}
