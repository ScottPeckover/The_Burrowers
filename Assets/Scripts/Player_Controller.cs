using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Globalization;
using System;

public class Player_Controller : MonoBehaviour {
	
	[HideInInspector] public float gravity;
	[HideInInspector] public bool allPaused, onGround, npcWarning;
    public float acceleration, maxSpeed, friction;
    
	//SerializeField allows private variables to be accessed on inspector
	[SerializeField] private LayerMask 
										groundLayer,
										platformLayer,
										dirtLayer,
										elevatorLayer;
    private Animator animator;
    public Animator blackOutAnim;
    public Image black;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private Collider2D platformCollider;
    private Dig_Manager digManager;
    private Rigidbody2D movingPlatformRB;
    private Talking npc;

    public Slider healthSlider;
    public Text moneyDisplay;

    private Vector3 lastPosition;
    private Quaternion originalRotation;

    private bool onPlatform, platformDrop, isAttacking, isFlashing, onMovingPlatform;
	private float attackTimer = 0.0f,
			ATTACK_TIME_MAX = 0.5f,
			health = 100.0f,
			attackHit = 1.0f,
            money = 0.0f;
    
    private float flashTimer = 0f, FLASH_TIME_MAX = 1.0f, moveX = 0f, moveY = 0f;
    Rigidbody2D rb2d;

    private const int
    STANDING = 0,
    WALKING = 1,
    JUMPING = 2,
    FALLING = 3,
    ATTACKING = 4,
    DIGGING = 5;

	private const string
	MOVEMENT_STATE = "movement_state",
	MOVEMENT_SPEED = "movement_speed";

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D> ();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        boxCollider.sharedMaterial.friction = friction;
		onGround = true;
		isAttacking = false;
		allPaused = false;
		gravity = rb2d.gravityScale;
        lastPosition = transform.position;
        originalRotation = transform.rotation;
        digManager = gameObject.GetComponent<Dig_Manager> ();
        if (healthSlider != null)
            healthSlider.value = health;
    }

    private void Update()
    {
        flash();
        if (!digManager.digging)
        {
            transform.rotation = originalRotation;
            //Check if player is on ground
            Vector3 position = transform.position;
            Vector2 positionLeft = new Vector2(position.x - 0.4f, position.y);
            Vector2 positionRight = new Vector2(position.x + 0.4f, position.y);
            Vector2 direction = Vector2.down;
            float distance = 0.8f;
            RaycastHit2D 
				groundHitLeft = Physics2D.Raycast(positionLeft, direction, distance, groundLayer),
                groundHitRight = Physics2D.Raycast(positionRight, direction, distance, groundLayer),
                platformHit = Physics2D.Raycast(position, direction, distance, platformLayer),
                dirtHit = Physics2D.Raycast(position, direction, distance, dirtLayer),
                onElevator = Physics2D.Raycast(position, direction, distance, elevatorLayer);
            
			onPlatform = platformHit.collider != null;
            if (groundHitLeft.collider != null || groundHitRight.collider != null || (onPlatform & rb2d.velocity.y <= 0.05f & !platformDrop) || dirtHit.collider != null || onElevator.collider != null)
            {
                onGround = true;
                boxCollider.sharedMaterial.friction = friction;
            }
            else
            {
                onGround = false;
                boxCollider.sharedMaterial.friction = 0;
            }
                

            //Quitting the Game
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();

            //Sets sprite animations
			if (onGround)
            {
                if (Mathf.Abs(rb2d.velocity.x) > 0.1)
                {
                    //walking
					animator.SetFloat(MOVEMENT_SPEED, (Mathf.Abs(rb2d.velocity.x) + maxSpeed) / maxSpeed);
                    if (!isAttacking)
                    {
                        if (onMovingPlatform)
                        {
                            if (movingPlatformRB.velocity.x != rb2d.velocity.x)
                                animator.SetInteger(MOVEMENT_STATE, WALKING);
                            else animator.SetInteger(MOVEMENT_STATE, STANDING);
                        }
                        else
                            animator.SetInteger(MOVEMENT_STATE, WALKING);
                    }
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
            float xVelocity = rb2d.velocity.x;
            if (onMovingPlatform && xVelocity == movingPlatformRB.velocity.x)
            {
                xVelocity = 0;
            }
            if (xVelocity > 1)
                spriteRenderer.flipX = true;

            if (xVelocity < -1)
                spriteRenderer.flipX = false;


            
            if (!isAttacking)
            {   //Limits speed of player
                float moveHorizontal = Input.GetAxis("Horizontal");
                if (moveHorizontal != 0)
                {
                    rb2d.velocity = new Vector2(Mathf.Clamp((moveHorizontal * acceleration + rb2d.velocity.x),-maxSpeed,maxSpeed), rb2d.velocity.y);
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
                                rb2d.velocity = new Vector2(rb2d.velocity.x, 12);
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
					    case "C": // Start Elevator / talking
                            if (npcWarning)
                            {
                                npc.toggleTalk();
                            }
						    if (onElevator.collider != null) 
							    foreach (GameObject go in FindObjectsOfType(typeof(GameObject)))
								    go.SendMessage ("OnElevatorMove", SendMessageOptions.DontRequireReceiver);
							break;
                    }
                }

            }
            else stopAttack();
        } else
        {
            //Digging rotation
            if (spriteRenderer.flipX == true)
                spriteRenderer.flipX = false;
            animator.SetInteger(MOVEMENT_STATE, DIGGING);
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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "NPC")
        {
            npcWarning = true;
            npc = collider.gameObject.GetComponent<Talking>();
            Debug.Log("Wombo!");
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
		if (collider.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            platformDrop = false;
            collider.isTrigger = false;
        }
        else if (collider.gameObject.tag == "NPC")
        {
            npcWarning = false;
            npc.talk(false);
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (!isFlashing)
        {
            switch (col.gameObject.tag)
            {
                case "Enemy":
                    if (isAttacking)
                        col.gameObject
                            .GetComponent<Enemy>()
                            .reduceHealth(attackHit);
                    else
                    {
                        isFlashing = true;
                        UpdateHealth(-col.gameObject.GetComponent<Enemy>().getDamage());
                    }
                    //Debug.Log(((isAttacking)?"Attacked: ":"Collision: ")+col.gameObject.GetComponent<Enemy>().getName()+"("+col.gameObject.GetComponent<Enemy>().getHealth()+")");
                    break;
                case "Spikes":
                    isFlashing = true;
                    UpdateHealth(-col.gameObject.GetComponent<Spikes>().damage);
                    rb2d.velocity = new Vector2(0, 15);
                    break;
                case "Lava":
                    isFlashing = true;
                    UpdateHealth(-20);
                    break;
            }
        }
    }
        void OnCollisionEnter2D (Collision2D col) {
        if (col.gameObject.layer == LayerMask.NameToLayer("Platform"))
            platformCollider = col.collider;
        else if (col.gameObject.tag == "MovingPlatform")
        {
            onMovingPlatform = true;
            movingPlatformRB = col.gameObject.GetComponent<Rigidbody2D>();
        }
        else if(col.gameObject.tag == "Poison")
        {
            isFlashing = true;
            UpdateHealth(-15);
        }
        
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "MovingPlatform")
            onMovingPlatform = false;
    }

    void UpdateHealth(float value)
    {
        health += value;
        healthSlider.value = health;
        if (health <= 0)
        {
            rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
            StartCoroutine(Fading());
        }
            
    }

    void UpdateMoney(float value)
    {
        money += value;
        moneyDisplay.text = money.ToString("C", CultureInfo.CurrentCulture);
    }

    public void flash()
    {
        if (!isFlashing)
        {
            spriteRenderer.color = Color.white;
            return;
        }

        spriteRenderer.color = Color.red;
        flashTimer += Time.deltaTime;
        if (flashTimer >= FLASH_TIME_MAX)
        {
            isFlashing = false;
            flashTimer = 0f;
            spriteRenderer.enabled = false;
        }
        spriteRenderer.enabled = !spriteRenderer.enabled;
    }

    IEnumerator Fading()
    {
        blackOutAnim.SetBool("Fade", true);
        yield return new WaitUntil(() => black.color.a == 1);
        LevelManager.ReloadLevel();
    }

    void OnGUI()
    {
    //    GUI.Label(new Rect(10, 10, 200, 20), "Player x_velocity:");
    //    GUI.Label(new Rect(10, 30, 100, 20), rb2d.velocity.x + "");
    //    GUI.Label(new Rect(10, 50, 200, 20), "Player y_velocity:");
    //    GUI.Label(new Rect(10, 70, 100, 20), rb2d.velocity.y + "");
    //    GUI.Label(new Rect(10, 90, 200, 20), "OnGround: ");
    //    GUI.Label(new Rect(10, 110, 100, 20), onGround + "");
    }
}
