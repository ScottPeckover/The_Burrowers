using System.Collections;
using UnityEngine;

public class Meerkat_Moves : Enemy {


	public Meerkat_Moves() {
		health = 7.0f;
		damage = 1.5f;
		height = 110;

		name = "Meerkat";
	}

	// Use this for initialization
	void Start () {
//		damage = 1.5f;
//		speed = 0.5f;
//		rb2d.velocity = new Vector2 (getSpeed()*getDirection(), 0f);
        rb2d.AddForce(new Vector2(getSpeed() * getDirection(), 0f));
        setSpriteRenderer (GetComponent<SpriteRenderer> ());
//		height = (int) gameObject.GetComponent<BoxCollider2D>().size.y;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isPaused()) {
//			rb2d.velocity = new Vector2 (getSpeed()*getDirection()*rb2d.velocity.x, 0f);
			rb2d.AddForce (new Vector2 (getSpeed () * getDirection (), 0f));
			flipX ((rb2d.velocity.x > 0.2));
			flash ();
		} else {
			rb2d.velocity = new Vector2 (0f, 0f);
		}
	}

}
