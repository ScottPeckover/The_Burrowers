using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour {

    public float acceleration;
    public float maxSpeed;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

	// Jump command variables
	private bool onGround, powerJump;

    Rigidbody2D rb2d;
    
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D> ();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

		onGround = true;
//		powerJump = false; // in case we want to add the power jump ability
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        if (rb2d.velocity.x != 0)
            animator.SetInteger("movement_state",1);
        if (rb2d.velocity.x > 0)
            spriteRenderer.flipX = true;
        else if (rb2d.velocity.x < 0)
            spriteRenderer.flipX = false;
        else animator.SetInteger("movement_state", 0);

        animator.SetFloat("movement_speed", (Mathf.Abs(rb2d.velocity.x) + maxSpeed) / maxSpeed);
        //Limits speed of player
        if (!(Mathf.Abs(rb2d.velocity.x) > maxSpeed))
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            Vector2 movement = new Vector2(moveHorizontal, 0);
            //rb2d.AddForce(movement * acceleration);
            rb2d.velocity = new Vector2((moveHorizontal * acceleration) + rb2d.velocity.x, rb2d.velocity.y);
        }

		// Jump Command
		Vector2 position = transform.position; // Get Position
		if (Input.GetKeyDown (KeyCode.Space)) {
			/* 	in case we want to add the power jump ability
			if (!onGround && powerJump){
				int speed = 3;
				int direction = (spriteRenderer.flipX)?1:-1;
				rb2d.velocity = new Vector2 ((direction*speed), 7);
				powerJump = false;
			}
			*/
			if (onGround) {
				powerJump = true;
				rb2d.velocity = new Vector2 (0, 7);
			} 
		}
		if (position.y < -1.5) 
			onGround = true;
		else 
			onGround = false;

		// EO Jump Command
    }

    void FixedUpdate ()
    {
        
        
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 20), "Player x_velocity:");
        GUI.Label(new Rect(10, 30, 100, 20), rb2d.velocity.x + "");
        GUI.Label(new Rect(10, 50, 200, 20), "Player y_velocity:");
        GUI.Label(new Rect(10, 70, 100, 20), rb2d.velocity.y + "");

		GUI.Label(new Rect(10, 100,200, 20), "onGround: "+onGround);
		GUI.Label(new Rect(10, 120,200, 20), "powerJump: "+powerJump);
    }
}
