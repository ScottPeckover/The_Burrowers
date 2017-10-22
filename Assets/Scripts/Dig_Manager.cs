using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dig_Manager : MonoBehaviour {

	public GameObject dirtWarning;
	private string digDirection = "Down";
	[SerializeField] private LayerMask dirtLayer;

	// Movement command variables
	private bool onDirt, enteringDirt;
	public bool digging;
	GameObject player;
	Player_Controller playerController;

	Rigidbody2D rb2d;


	void Start() {
		rb2d = GetComponent<Rigidbody2D> ();

		//Allows script to communicate with Player_Controller.cs
		player = GameObject.Find("Monty");
		playerController = player.GetComponent<Player_Controller> ();
		dirtWarning.SetActive (false);
		onDirt = false; //checks if the player is standing on diggable dirt
		enteringDirt = false; //checks if the player is entering or exiting dirt
		digging = false; //checks if the player is digging
	}


	private void FixedUpdate() {
		
		
		//Detect if Player is on diggable dirt
		DetectDirt ();

		//Starts and Stops digging
		if (onDirt && playerController.onGround) {
			switch (Input.inputString) {
			case "c":
			case "C":
				if (!enteringDirt) 
					StartDigging ();
				else
					StopDigging ();
				break;
			}
		}
			
		//Set up digging movement
		if (digging) {
			float moveHorizontal = Input.GetAxis("Horizontal");
			float moveVertical = Input.GetAxis("Vertical");
			float tempAcceleration = 10f;
			rb2d.velocity = new Vector3(moveHorizontal * tempAcceleration, moveVertical * tempAcceleration, 0);
		}

	}


	private void DetectDirt() {
		//Detects if player is on top of diggable dirt
		Vector2 position = transform.position;
		RaycastHit2D hitUp = Physics2D.Raycast(position, Vector2.up, 1.0f, dirtLayer);
		RaycastHit2D hitDown = Physics2D.Raycast(position, Vector2.down, 1.0f, dirtLayer);
		RaycastHit2D hitRight = Physics2D.Raycast(position, Vector2.right, 1.0f, dirtLayer);
		RaycastHit2D hitLeft = Physics2D.Raycast(position, Vector2.left, 1.0f, dirtLayer);

		if (hitDown.collider != null || hitLeft.collider != null || hitRight.collider != null || hitUp.collider != null) {
			onDirt = true;
			playerController.onGround = true;
			dirtWarning.SetActive (true);
			if (hitDown.collider != null)
				digDirection = "Down";
			else if (hitLeft.collider != null)
				digDirection = "Left";
			else if (hitRight.collider != null)
				digDirection = "Right";
			else if (hitUp.collider != null)
				digDirection = "Up";
		} else {
			onDirt = false;
			dirtWarning.SetActive (false);
		}
	}

	private void StartDigging() {
		//makes sure the ! above the character is disabled
		dirtWarning.SetActive (false);

		//moves player into the dirt
		Vector2 pos = transform.position; 
		switch(digDirection) {
		case "Up":
			pos.y += 1f;
			break;
		case "Right":
			pos.x += 1f;
			break;
		case "Down":
			pos.y -= 1f;
			//rb2d.velocity = new Vector2(rb2d.velocity.x, 5);
			break;
		case "Left":
			pos.x -= 1f;
			break;
		}
		rb2d.transform.position = pos;

		//turns off gravity for Monty's dig movement
		rb2d.gravityScale = 0;

		enteringDirt = true;
		digging = true;
		onDirt = false;
		playerController.onGround = false;
	}

	private void StopDigging() {
		//makes sure the ! above the character is disabled
		dirtWarning.SetActive (false);

		//moves player into the dirt
		Vector2 pos = transform.position; 
		switch(digDirection) {
		case "Up":
			pos.y += 1f;
			break;
		case "Right":
			pos.x += 1.2f;
			break;
		case "Down":
			pos.y -= 1f;
			//rb2d.velocity = new Vector2(rb2d.velocity.x, 5);
			break;
		case "Left":
			pos.x -= 1.2f;
			break;
		}
		rb2d.transform.position = pos;

		//turns gravity back on
		rb2d.gravityScale = playerController.gravity;

		enteringDirt = false;
		digging = false;
	}
		

	public bool IsDigging() {
		return digging;	
	}

	void OnCollisionEnter2D (Collision2D col) {
		switch (col.gameObject.tag) {
		case "DigZoneUp":
			digDirection = "Up";
			onDirt = true;
			playerController.onGround = true;
			dirtWarning.SetActive (true);
			Debug.Log ("UP zone triggered");
			break;
		case "DigZoneRight":
			digDirection = "Right";
			break;
		case "DigZoneDown":
			digDirection = "Down";
			break;
		case "DigZoneLeft":
			digDirection = "Left";
			break;

		}
	}
}
