using System.Collections;
using UnityEngine;

public class Meerkat_Moves : Enemy {


	public Meerkat_Moves() {
		health = 3.0f;
		damage = 7f;
		height = 110;

		name = "Meerkat";
	}

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = 3.0f;
        damage = 10f;
        height = 110;
        direction = -1;
        speed = 4;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.direction == 1)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;
        rb2d.velocity = new Vector2(this.direction * speed, rb2d.velocity.y);
        flash();
        Vector2 direction = this.direction == 1 ? Vector2.right : Vector2.left;
        float distance = 0.7f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance);
        if (hit.collider != null)
            this.direction = this.direction * -1;
    }

}
