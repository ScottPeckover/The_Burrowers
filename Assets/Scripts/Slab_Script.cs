using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slab_Script : MonoBehaviour {

	[SerializeField] private LayerMask groundLayer;
	Rigidbody2D rb2d;
	private int speed, direction;
	private RaycastHit2D groundHit;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		speed = 1; direction = -1;
	}
	
	// Update is called once per frame
	void Update () {
		groundHit = Physics2D.Raycast ((Vector2)transform.position, (direction>0)?Vector2.right:Vector2.left, 1.5f, groundLayer);
		if (groundHit.collider != null)
			direction = direction * -1;
		rb2d.velocity = new Vector2 (speed*direction, 0);
	}

	void OnGUI(){
		GUI.Label(new Rect(10, 170, 70, 20), "Speed: "+speed);
		GUI.Label(new Rect(80, 170, 70, 20), "Direction: "+direction);
		GUI.Label(new Rect(170, 170, 140, 20), "Velocity: "+rb2d.velocity);
		GUI.Label(new Rect(310, 170, 100, 20), "onGround: "+(groundHit.collider!=null));
	}
}
