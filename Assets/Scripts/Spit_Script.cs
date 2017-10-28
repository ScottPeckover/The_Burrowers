using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spit_Script : MonoBehaviour {

	public Transform target;
	private Vector2 direction;
	private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
		// direction = target.position - transform.position;
		rb2d = GetComponent<Rigidbody2D> ();
		// rb2d.AddForce (direction);
	}
	
	// Update is called once per frame
	void Update () {
		// rb2d.AddForce (direction);
	}

	void OnCollisionEnter2D(Collision2D col) {
		if( col.gameObject.layer == LayerMask.NameToLayer("Ground") || col.gameObject.layer == LayerMask.NameToLayer("Player") )
			Destroy (col.otherCollider.gameObject);
	}
}
