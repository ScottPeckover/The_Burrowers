using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spit_Script : MonoBehaviour {
	private Vector2 direction;
	private Rigidbody2D rb2d;
    public float damage;
    

	void OnCollisionEnter2D(Collision2D col) {
		if( col.gameObject.layer == LayerMask.NameToLayer("Ground") || col.gameObject.layer == LayerMask.NameToLayer("Player") )
			Destroy (col.otherCollider.gameObject);
	}
}
