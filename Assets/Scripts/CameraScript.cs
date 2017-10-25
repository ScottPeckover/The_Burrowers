using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
    public Transform playerTransform;

	// Update is called once per frame
	void Update () {
		transform.position = new Vector2(playerTransform.position.x, playerTransform.position.y);
    }
}
