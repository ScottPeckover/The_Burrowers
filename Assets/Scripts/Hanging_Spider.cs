using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hanging_Spider : Enemy {

	public Hanging_Spider() {
		health = 1.0f;
		damage = 5f;

		name = "Spiderling";
	}

	// Use this for initialization
	void Start () {
		setSpriteRenderer (GetComponent<SpriteRenderer> ());
	}
	
	// Update is called once per frame
	void Update () {
		if (!isPaused()) {
			rb2d.AddForce (new Vector2 (0f, getDirection () * getSpeed ()));
			flash ();
		} else {
			rb2d.velocity = new Vector2 (0f, 0f);
		}
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        direction = direction * -1;

    }
}
