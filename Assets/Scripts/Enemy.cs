using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [HideInInspector]
	public SpriteRenderer spriteRenderer;
	private bool 
				isFlashing, 
				paused;
	private float 
	flashTimer = 0f,
	FLASH_TIME_MAX = 1.0f;

	protected float
				health,
				damage;
	protected string
				name;
	protected int 
				direction,
				speed,
				height;

	public Rigidbody2D rb2d;

	public Enemy() {
		health = 10.0f;
		damage = 15.0f;

		name = "Enemy";

		direction = -1;
		speed = 1;
		height = 50;

		paused = false;
	}

	public void setSpriteRenderer(SpriteRenderer renderer) {
		spriteRenderer = renderer;
	}

	public float getDamage() {
		return damage;
	}

	public void reduceHealth(float hit) {
		if (isFlashing)
			return;
		health -= hit;
		isFlashing = true;
        if (health <= 0)
            Destroy(gameObject);
    }

	public int getDirection() {
		return direction;
	}

	public int getSpeed() {
		return speed;
	}

	public float getHealth() {
		return health;
	}

	public string getName() {
		return name;
	}

	public void flipX(bool toRight){
		spriteRenderer.flipX = toRight;
	}

	public void flash(){
        if (!isFlashing)
        {
            spriteRenderer.color = Color.white;
            return;
        }

        spriteRenderer.color = Color.red;
        flashTimer += Time.deltaTime;
		if (flashTimer >= FLASH_TIME_MAX) {
			isFlashing = false;
			flashTimer = 0f;
			spriteRenderer.enabled = false;
		}
		spriteRenderer.enabled = !spriteRenderer.enabled;
	}

	public bool isPaused() {
		return paused;
	}

	void OnPausedGame () {
		paused = !paused;
	}

	//void OnGUI () {
	//	Vector2 targetPosition;
	//	targetPosition = Camera.main.WorldToScreenPoint (transform.position);

//		GUI.Box (new Rect(targetPosition.x - (health*5), Screen.height - targetPosition.y - height, health*10, 20), ""+health);
	//}

}