using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dig_Manager : MonoBehaviour {


	[HideInInspector] public GameObject dirtWarning;
	[HideInInspector] public bool digging;

	//SerializeField allows private variables to be accessed on inspector
	[SerializeField] private LayerMask dirtLayer;
	private string digDirection = "Down";
	private bool onDirt;

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
		digging = false; //checks if the player is digging
	}

	private void FixedUpdate() {
		//Detect if Player is on diggable dirt
		DetectDirt ();
	}

	private void Update() {
		//Input for digging
		if (onDirt && playerController.onGround) {
			switch (Input.inputString) {
			case "c":
			case "C":
				if (!digging) 
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
		Vector2 position = transform.position;
		RaycastHit2D hitUp = Physics2D.Raycast(position, Vector2.up, 1.0f, dirtLayer);
		RaycastHit2D hitDown = Physics2D.Raycast(position, Vector2.down, 1.0f, dirtLayer);
		RaycastHit2D hitRight = Physics2D.Raycast(position, Vector2.right, 1.0f, dirtLayer);
		RaycastHit2D hitLeft = Physics2D.Raycast(position, Vector2.left, 1.0f, dirtLayer);

		//true when there is diggable dirt nearby
		if (hitDown.collider != null || hitLeft.collider != null || hitRight.collider != null || hitUp.collider != null) {
			onDirt = true;
			playerController.onGround = true;
			dirtWarning.SetActive (true);

			//finds which direction the dirt is, relative to monty
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
		Vector2 pos = transform.position; 

		//moves player into the dirt
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
		digging = true;
		onDirt = false;
		playerController.onGround = false;
		//turns off gravity for monty
		rb2d.gravityScale = 0;
		//makes sure the '!' above monty is disabled
		dirtWarning.SetActive (false);
	}
		
	private void StopDigging() {
		Vector2 pos = transform.position;

		//moves player out of the dirt
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
		digging = false;
		//turns gravity back on
		rb2d.gravityScale = playerController.gravity;
		//makes sure the '!' above monty is disabled
		dirtWarning.SetActive (false);
	}
		

	public bool IsDigging() {
		return digging;	
	}

	public bool IsOnDirt() {
		return onDirt;
	}


}
