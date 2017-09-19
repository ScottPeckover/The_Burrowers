using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meerkat_Moves : MonoBehaviour {

	public Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
		rb2d.AddForce (new Vector2(1f, 0f));
		rb2d.velocity = new Vector2 (1f, 0f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D (Collision2D col) {
		if (col.gameObject.tag == "Player") {
			Physics2D.IgnoreCollision (col.collider, col.otherCollider);
		}
	}
}
