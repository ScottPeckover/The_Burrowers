using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour {

    public float acceleration;
    public float maxSpeed;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    Rigidbody2D rb2d;
    
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D> ();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
    }
}
