using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour {

    public float acceleration, 
		maxSpeed,
		gravity;
    
	public bool allPaused, onGround;
    
	//SerializeField allows variables to be private - but visible in the inspector
	[SerializeField] private LayerMask groundLayer;
	[SerializeField] private LayerMask platformLayer;
	[SerializeField] private LayerMask dirtLayer;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private Collider2D platformCollider;

    // Movement command variables
    private bool onPlatform, platformDrop, isAttacking;
	private float attackTimer = 0.0f,
			ATTACK_TIME_MAX = 0.5f,
			health = 10.0f,
			attackHit = 1.0f;

    Rigidbody2D rb2d;
    
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D> ();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

		onGround = true;
		isAttacking = false;
		allPaused = false;
		gravity = rb2d.gravityScale;
    }

    private void Update()
    {
        //GroundChecking
        Vector3 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 0.8f;

        RaycastHit2D groundHit = Physics2D.Raycast(position, direction, distance, groundLayer);
        RaycastHit2D platformHit = Physics2D.Raycast(position, direction, distance, platformLayer);
        onPlatform = platformHit.collider != null;
        if (groundHit.collider != null | (onPlatform & rb2d.velocity.y <= 0.05f & !platformDrop))
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
            {
                if (!isAttacking) animator.SetInteger("movement_state", 3);
            }
            if (rb2d.velocity.y > 0.05f)
            {
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Platform"), LayerMask.NameToLayer("Player"), true);
            } else if (!platformDrop)
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Platform"), LayerMask.NameToLayer("Player"), false);
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
            //Dropping through platforms
            if (Input.GetKeyDown(KeyCode.Z) & Input.GetKey(KeyCode.DownArrow) & onPlatform)
            {
                platformDrop = true;
                platformCollider.isTrigger = true;
            }
            
            //Inputs
            switch (Input.inputString)
            {
		        case "z":
		        case "Z": // Jump
                    if (onGround & !platformDrop)
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

		        case "p": // Pause
			        Object[] objects = FindObjectsOfType (typeof(GameObject));
			        foreach (GameObject go in objects)
				        go.SendMessage ("OnPausedGame", SendMessageOptions.DontRequireReceiver);
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

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            platformDrop = false;
            collider.isTrigger = false;
        }
    }

    void OnCollisionEnter2D (Collision2D col) {
        if (col.gameObject.layer == LayerMask.NameToLayer("Platform"))
            platformCollider = col.collider;
        else
        {
            switch (col.gameObject.tag)
            {
                case "Enemy":
                    if (isAttacking)
                        col
                                .gameObject
                            .GetComponent<Enemy>()
                            .reduceHealth(attackHit);
                    else
                        health -= col.gameObject.GetComponent<Enemy>().getDamage();
                    //Debug.Log(((isAttacking)?"Attacked: ":"Collision: ")+col.gameObject.GetComponent<Enemy>().getName()+"("+col.gameObject.GetComponent<Enemy>().getHealth()+")");
                    break;
            }
        }
	}

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 20), "Player x_velocity:");
        GUI.Label(new Rect(10, 30, 100, 20), rb2d.velocity.x + "");
        GUI.Label(new Rect(10, 50, 200, 20), "Player y_velocity:");
        GUI.Label(new Rect(10, 70, 100, 20), rb2d.velocity.y + "");
        GUI.Label(new Rect(10, 90, 200, 20), "OnGround: ");
        GUI.Label(new Rect(10, 110, 100, 20), onGround + "");
    }
}
