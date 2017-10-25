using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slab_Script : MonoBehaviour {
    Rigidbody2D rb2d;
	private int speed, direction;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		speed = 1; direction = -1;
        rb2d.velocity = new Vector2(speed * direction, 0);
    }
	
	// Update is called once per frame
	void OnCollisionEnter2D (Collision2D col) {
		if (col.gameObject.layer != LayerMask.NameToLayer("Player"))
			direction = direction * -1;
        rb2d.velocity = new Vector2(speed * direction, 0);
    }

}
