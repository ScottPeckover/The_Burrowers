using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour {

    public float acceleration, 
        maxSpeed;
    public LayerMask groundLayer;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

	// Movement command variables
	private bool onGround, isAttacking;
	private float attackTimer = 0.0f,
			ATTACK_TIME_MAX = 0.5f,
			health = 10.0f;

    Rigidbody2D rb2d;
    
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D> ();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

		onGround = true;
		isAttacking = false;
    }

    private void FixedUpdate()
    {
        //GroundChecking
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 1.0f;

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
        if (hit.collider != null)
        {
            onGround = true;
        } else
            onGround = false;

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (onGround)
        {
            if (Mathf.Abs(rb2d.velocity.x) > 0.1)
            {
                animator.SetFloat("movement_speed", (Mathf.Abs(rb2d.velocity.x) + maxSpeed) / maxSpeed);
                if (!isAttacking) animator.SetInteger("movement_state",1);
            }
                
            else
                if (!isAttacking) animator.SetInteger("movement_state", 0);
        }
        else
        {
            if (rb2d.velocity.y < 0)
                if (!isAttacking) animator.SetInteger("movement_state", 3);
        }
        if (rb2d.velocity.x > 1)
            spriteRenderer.flipX = true;
            
        if (rb2d.velocity.x < -1)
            spriteRenderer.flipX = false;



        //Limits speed of player
        if (!isAttacking)
        {
            if (!(Mathf.Abs(rb2d.velocity.x) > maxSpeed))
            {
                float moveHorizontal = Input.GetAxis("Horizontal");
                Vector2 movement = new Vector2(moveHorizontal, 0);
                rb2d.velocity = new Vector2((moveHorizontal * acceleration) + rb2d.velocity.x, rb2d.velocity.y);
            }

            // Jump Command
            switch (Input.inputString)
            {
                case "z":
                case "Z": // Jump
                    if (onGround)
                    {
                        rb2d.velocity = new Vector2(rb2d.velocity.x, 15);
                        if (!isAttacking) animator.SetInteger("movement_state", 2);
                    }
                    break;

                case "x":
                case "X": // Attack
                    animator.SetInteger("movement_state", 4);
                    isAttacking = true;
                    rb2d.velocity = new Vector2((spriteRenderer.flipX) ? 7 : -7, rb2d.velocity.y);
                    break;
            }
        }
        else stopAttack();
    }

	private void stopAttack () {
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
				Destroy (col.gameObject);
			else
				health -= col.gameObject.GetComponent<Meerkat_Moves>().damage;
			break;
		}
	}

	void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 20), "Player x_velocity:");
        GUI.Label(new Rect(10, 30, 100, 20), rb2d.velocity.x + "");
        GUI.Label(new Rect(10, 50, 200, 20), "Player y_velocity:");
        GUI.Label(new Rect(10, 70, 100, 20), rb2d.velocity.y + "");
        GUI.Label(new Rect(10, 90, 200, 20), "isAttacking: ");
        GUI.Label(new Rect(10, 110, 100, 20), isAttacking + "");
    }
}
