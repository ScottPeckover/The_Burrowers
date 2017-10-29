using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : Enemy {
	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = 2.0f;
        damage = 10.0f;
        direction = 1;
        speed = 4;
    }
	
	// Update is called once per frame
	void Update () {
        if (this.direction == 1)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;
        rb2d.velocity = new Vector2(this.direction* speed, rb2d.velocity.y);
        flash();
        Vector2 direction = this.direction == 1 ? Vector2.right : Vector2.left;
        float distance = 1.5f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance);
        if (hit.collider != null)
            this.direction = this.direction * -1;
    }
}
