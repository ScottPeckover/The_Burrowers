using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour {
	/// <summary>
	/// 
	/// Simple script to handle treasure chest values
	/// 
	/// </summary>

	[SerializeField] private float value;

	public float getValue() {
		return value;
	}
}
