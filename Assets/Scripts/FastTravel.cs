using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class FastTravel : MonoBehaviour {
	/// <summary>
	/// 
	/// This script handles the 'fast travel' between scenes
	/// As well as fading in/out effects when changing scenes
	/// 
	/// </summary>

	public int levelToLoad, currentLevel;
	public Button levelSelectBtn;
	public Image black;
	public Animator anim;

	void Start () {
		//enables button script for the fast travel map
		if (currentLevel == 1) 
			levelSelectBtn.onClick.AddListener(TaskOnClick);
	}

	void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            StartCoroutine(Fading());
            col.gameObject.SetActive(false);
        }
		//Note: StartCoroutine must be used - OR - StartCoroutine("Fading");
		//Fading(); - won't work
	}

	//OnClick function for fading when clicking/choosing a level on the Fast Travel Map
	void TaskOnClick() {
		StartCoroutine (Fading ());
	}

	//Fading in/out effect between scenes
	IEnumerator Fading() {
		anim.SetBool ("Fade", true);
		yield return new WaitUntil (() => black.color.a == 1);
		SceneManager.LoadScene (levelToLoad);
	}
}
