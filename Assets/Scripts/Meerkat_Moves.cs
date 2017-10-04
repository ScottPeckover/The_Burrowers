using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meerkat_Moves : MonoBehaviour {

	public Rigidbody2D rb2d;
	public float damage;
	private SpriteRenderer spriteRenderer;
	private float speed;

	// Use this for initialization
	void Start () {
		damage = 1.5f;
		speed = 0.5f;
		rb2d.AddForce (new Vector2(speed, 0f));
		rb2d.velocity = new Vector2 (speed, 0f);
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if (rb2d.velocity.x > 0)
			spriteRenderer.flipX = true;
		else if (rb2d.velocity.x < 0)
			spriteRenderer.flipX = false;
	}

	void OnCollisionEnter2D (Collision2D col) {
		if (col.gameObject.tag == "Player") {
			Physics2D.IgnoreCollision (col.collider, col.otherCollider);
		}
	}
}
