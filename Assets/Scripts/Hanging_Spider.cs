using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hanging_Spider : MonoBehaviour {

	public Rigidbody2D rb2d;
	private float direction;

	// Use this for initialization
	void Start () {
		direction = 1f;
	}
	
	// Update is called once per frame
	void Update () {
		rb2d.AddForce (new Vector2 (0f, direction));
	}

	void OnCollisionEnter2D(Collision2D coll) {
		direction = direction*-1;

		if( coll.gameObject.tag == "Player" ){
			Physics2D.IgnoreCollision(coll.collider, coll.otherCollider);
			Debug.Log ("AAAAAAHHHH!!");
			this.gameObject.GetComponent<SpriteRenderer> ().color = Color.red;
		}
	}

}
