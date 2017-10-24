using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour {
	
	[HideInInspector] public float gravity;
	[HideInInspector] public bool allPaused, onGround;
    public float acceleration, maxSpeed;
    
	//SerializeField allows private variables to be accessed on inspector
	[SerializeField] private LayerMask 
										groundLayer,
										platformLayer,
										dirtLayer,
										elevatorLayer;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private Collider2D platformCollider;
    private Dig_Manager digManager;

    private Vector3 lastPosition;
    private Quaternion originalRotation;

    private bool onPlatform, platformDrop, isAttacking;
	private float attackTimer = 0.0f,
			ATTACK_TIME_MAX = 0.5f,
			health = 10.0f,
			attackHit = 1.0f;

    private float moveX = 0f, moveY = 0f;
    Rigidbody2D rb2d;

	private const int
	STANDING = 0,
	WALKING = 1,
	JUMPING = 2,
	FALLING = 3,
	ATTACKING = 4;

	private const string
	MOVEMENT_STATE = "movement_state",
	MOVEMENT_SPEED = "movement_speed";

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
        lastPosition = transform.position;
        originalRotation = transform.rotation;
    }

    private void Update()
    {
		//Allows script to communicate with Dig Manager.cs
		digManager = gameObject.GetComponent<Dig_Manager> ();
        if (!digManager.digging)
        {
            transform.rotation = originalRotation;
            //Check if player is on ground
            Vector3 position = transform.position;
            Vector2 direction = Vector2.down;
            float distance = 0.8f;
            RaycastHit2D 
				groundHit = Physics2D.Raycast(position, direction, distance, groundLayer),
            	platformHit = Physics2D.Raycast(position, direction, distance, platformLayer),
				onElevator = Physics2D.Raycast(position, direction, distance, elevatorLayer);
            
			onPlatform = platformHit.collider != null;
            if (groundHit.collider != null || (onPlatform & rb2d.velocity.y <= 0.05f & !platformDrop) || digManager.IsOnDirt())
            {
                onGround = true;
            }
            else
                onGround = false;

            //Quitting the Game
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();

            //Sets sprite animations
			if (onGround || onElevator.collider!=null)
            {
                if (Mathf.Abs(rb2d.velocity.x) > 0.1)
                {
                    //walking
					animator.SetFloat(MOVEMENT_SPEED, (Mathf.Abs(rb2d.velocity.x) + maxSpeed) / maxSpeed);
					if (!isAttacking) animator.SetInteger(MOVEMENT_STATE, WALKING);
                }
                else
					if (!isAttacking) animator.SetInteger(MOVEMENT_STATE, STANDING);
            }
            else
            {
                if (rb2d.velocity.y < 0)
                {
                    //falling
					if (!isAttacking) animator.SetInteger(MOVEMENT_STATE, FALLING);
                }
                if (rb2d.velocity.y > 0.05f)
                {
                    Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Platform"), LayerMask.NameToLayer("Player"), true);
                }
                else if (!platformDrop)
                    Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Platform"), LayerMask.NameToLayer("Player"), false);
            }
            //Flips player sprite depending on movement direction
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
                    //Vector2 movement = new Vector2(moveHorizontal, 0);
                    rb2d.velocity = new Vector2((moveHorizontal * acceleration) + rb2d.velocity.x, rb2d.velocity.y);
                }
                //Dropping through platforms
                if (Input.GetKeyDown(KeyCode.Z) & Input.GetKey(KeyCode.DownArrow) & onPlatform)
                {
                    platformDrop = true;
                    platformCollider.isTrigger = true;
                }

                //Inputs
                if (!digManager.IsDigging())
                {
                    switch (Input.inputString)
                    {
                        case "z":
                        case "Z": // Jump
                            if (	onGround && !platformDrop || // onGround
									digManager.IsOnDirt() && !platformDrop || // onDirt
									onElevator.collider!=null && !platformDrop	) { // onElevator
                                rb2d.velocity = new Vector2(rb2d.velocity.x, 15);
								animator.SetInteger(MOVEMENT_STATE, JUMPING);
                            }
                            break;
                        case "x":
                        case "X": // Attack
							animator.SetInteger(MOVEMENT_STATE, ATTACKING);
                            isAttacking = true;
                            rb2d.velocity = new Vector2((spriteRenderer.flipX) ? 7 : -7, rb2d.velocity.y);
                            break;

						case "p":
						case "P": // Pause Game
							foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
								go.SendMessage("OnPausedGame", SendMessageOptions.DontRequireReceiver);
							break;

					case "c":
					case "C": // Elevator Movement
						if( onElevator.collider!=null )
							foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
								go.SendMessage ("OnElevatorMove", SendMessageOptions.DontRequireReceiver);
						break;
                    }
                }

            }
            else stopAttack();
        } else
        {
            Vector3 moveDirection = gameObject.transform.position - lastPosition;
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.UpArrow))
            {
                moveX = Input.GetAxis("Horizontal");
                moveY = Input.GetAxis("Vertical");

            }
            if (moveDirection != Vector3.zero)
            {
                float angle = Mathf.Atan2(moveY, moveX) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            lastPosition = gameObject.transform.position;
        }
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
