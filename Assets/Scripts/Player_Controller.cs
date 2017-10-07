using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour {

    public float 
			acceleration,
			maxSpeed;

	public bool allPaused;
    
	private Animator animator;
    private SpriteRenderer spriteRenderer;

	// Movement command variables
	private bool onGround, powerJump, isAttacking;
	private float 
			attackTimer = 0.0f,
			ATTACK_TIME_MAX = 3.0f,
			health = 10.0f,
			attackHit = 1.0f;

    Rigidbody2D rb2d;
    
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D> ();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

		onGround = true;
		isAttacking = false;
		allPaused = false;
//		powerJump = false; // in case we want to add the power jump ability
    }

    void Update()
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
		switch (Input.inputString) {
			/* 	in case we want to add the power jump ability
			if (!onGround && powerJump){
				int speed = 3;
				int direction = (spriteRenderer.flipX)?1:-1;
				rb2d.velocity = new Vector2 ((direction*speed), 7);
				powerJump = false;
			}
			*/
		case "z":
		case "Z": // Jump
			if (onGround) {
				powerJump = true;
				rb2d.velocity = new Vector2 (0, 7);
			}
			break;

		case "x":
		case "X": // Attack
			isAttacking = true;
			rb2d.velocity = new Vector2((spriteRenderer.flipX) ? 7 : -7, 0);
			break;

		case "p": // Pause
			Object[] objects = FindObjectsOfType (typeof(GameObject));
			foreach (GameObject go in objects)
				go.SendMessage ("OnPausedGame", SendMessageOptions.DontRequireReceiver);
			break;
		}
		if (position.y < -1.5) 
			onGround = true;
		else 
			onGround = false;
		// EO Jump Command

		stopAttack ();
    }

	private void stopAttack () {
		if (!isAttacking)
			return;

		attackTimer += Time.deltaTime;
		if (attackTimer >= ATTACK_TIME_MAX) {
			attackTimer = 0.0f;
			isAttacking = false;
		}
	}

	void OnCollisionEnter2D (Collision2D col) {
		switch (col.gameObject.tag) {
		case "Enemy":
			if (isAttacking)
				col
						.gameObject
					.GetComponent<Enemy>()
					.reduceHealth(attackHit);
			else
				health -= col.gameObject.GetComponent<Enemy>().getDamage();
			Debug.Log(((isAttacking)?"Attacked: ":"Collision: ")+col.gameObject.GetComponent<Enemy>().getName()+"("+col.gameObject.GetComponent<Enemy>().getHealth()+")");
			break;
		}
	}

	void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 20), "Player x_velocity:");
        GUI.Label(new Rect(10, 30, 100, 20), rb2d.velocity.x + "");
        GUI.Label(new Rect(10, 50, 200, 20), "Player y_velocity:");
        GUI.Label(new Rect(10, 70, 100, 20), rb2d.velocity.y + "");
    }
}
