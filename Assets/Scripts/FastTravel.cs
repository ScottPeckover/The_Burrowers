using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class FastTravel : MonoBehaviour {

	LevelManager gm;
	public int levelToLoad;
	public Button levelSelectBtn;

	public Image black;
	public Animator anim;


	void Start () {
		gm = FindObjectOfType<LevelManager> ();
		Button btn = levelSelectBtn.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void OnTriggerEnter2D(Collider2D col) {
		//Right Way
		StartCoroutine(Fading());
		//or
		//StartCoroutine("Fading");

		//Wont work
		//Fading();
	}

	IEnumerator Fading() {
		anim.SetBool ("Fade", true);
		yield return new WaitUntil (() => black.color.a == 1);
		SceneManager.LoadScene (levelToLoad);
	}

	void TaskOnClick() {
		StartCoroutine (Fading ());
	}
}
