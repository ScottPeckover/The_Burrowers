using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocker : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Physics2D.IgnoreLayerCollision(gameObject.layer,LayerMask.NameToLayer("Player"));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
