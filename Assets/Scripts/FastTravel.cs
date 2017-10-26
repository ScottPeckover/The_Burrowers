using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class FastTravel : MonoBehaviour {

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
        //correct way
		//OR - StartCoroutine("Fading");
		//Fading(); - won't work
	}

	void TaskOnClick() {
		StartCoroutine (Fading ());
	}

	IEnumerator Fading() {
		anim.SetBool ("Fade", true);
		yield return new WaitUntil (() => black.color.a == 1);
		SceneManager.LoadScene (levelToLoad);
	}
}
