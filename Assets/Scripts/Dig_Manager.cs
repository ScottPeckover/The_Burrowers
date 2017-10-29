using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dig_Manager : MonoBehaviour {
	/// <summary>
	/// 
	/// Dig Manager handles all digging mechanics.
	/// Currently it also handles script to open Treasure Chests also, to avoid bugs.
	/// 
	/// </summary>

	public GameObject dirtWarning, treasureWarning;
	[HideInInspector] public bool digging, onTreasure;

	//SerializeField allows private variables to be accessed on inspector
	[SerializeField] private LayerMask dirtLayer, elevatorLayer, treasureLayer;
	private string digDirection = "Down";
	private bool onDirt;
	private float chestValue;
    
	Player_Controller playerController;
	private Elevator_Script elevatorScript;
	private Treasure treasureScript;

    [HideInInspector]
	public GameObject chestToOpen;
	Rigidbody2D rb2d;


	void Start() {
		//Allow this script to communicate with the PlayerController script
		playerController = gameObject.GetComponent<Player_Controller> ();

		//Allow this script to communicate with the Elevator script
		elevatorScript = gameObject.GetComponent<Elevator_Script> ();

		//Allow this script to communicate with the Treasure script
		treasureScript = gameObject.GetComponent<Treasure> ();

		rb2d = GetComponent<Rigidbody2D> ();

		dirtWarning.SetActive (false);
		onDirt = false; //checks if the player is standing on diggable dirt
		digging = false; //checks if the player is digging
		onTreasure = false; //checks if the player has found treasure
	}

	private void FixedUpdate() {
		//Finds the position of the player
		Vector2 position = transform.position;

		//Detect if Player is on diggable dirt
		DetectDirt (position);
        
	}

	private void Update() {
		//Inputs for digging 
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

		//Inputs for opening treasure
		if (onTreasure) {
			switch (Input.inputString) {
			case "c":
			case "C":
				OpenTreasure ();
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
		
	//Detects whether player is able to dig
	private void DetectDirt(Vector2 position) {
		//sets up raycasts for finding dirt nearby
		RaycastHit2D hitUp = Physics2D.Raycast(position, Vector2.up, 1.0f, dirtLayer),
		 	hitDown = Physics2D.Raycast(position, Vector2.down, 1.0f, dirtLayer),
		 	hitRight = Physics2D.Raycast(position, Vector2.right, 1.0f, dirtLayer),
		 	hitLeft = Physics2D.Raycast(position, Vector2.left, 1.0f, dirtLayer);

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
			//also checks for nearby elevator or NPC to display a hint
			RaycastHit2D onElevator = Physics2D.Raycast(position, Vector2.down, 1.0f, elevatorLayer);
			if (onElevator.collider != null || playerController.npcWarning) 
				dirtWarning.SetActive (true);
			else
				dirtWarning.SetActive (false);
		}
	}
		
	//Moves player into the dirt and starts digging
	private void StartDigging() {
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
			break;
		case "Left":
			pos.x -= 1f;
			break;
		}
		rb2d.transform.position = pos;
		digging = true;
		onDirt = false;
		playerController.onGround = false;
		//turns off gravity for monty for dig movement
		rb2d.gravityScale = 0;
		//makes sure the dig hint is disabled
		dirtWarning.SetActive (false);
	}
		
	//Stops digging and emerges from the dirt
	private void StopDigging() {
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
			break;
		case "Left":
			pos.x -= 1.2f;
			break;
		}
		rb2d.transform.position = pos;
		digging = false;
		//turns gravity back on
		rb2d.gravityScale = playerController.gravity;
		//makes sure the dig hint is disabled
		dirtWarning.SetActive (false);
	}

	//Opens treasure chest and awards money
	private void OpenTreasure() {
		if (chestToOpen.tag == "ChestSmall")
			chestValue = 23.0f;
		 else if (chestToOpen.tag == "ChestBig") 
			chestValue = 48.0f;
		
		playerController.UpdateMoney (chestValue);
		chestToOpen.SetActive (false);
	}


	public bool IsDigging() {
		return digging;	
	}

	public bool IsOnDirt() {
		return onDirt;
	}

	public void disableHint() {
		dirtWarning.SetActive (false);
	}
}
